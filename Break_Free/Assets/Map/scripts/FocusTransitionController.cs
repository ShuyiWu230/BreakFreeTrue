using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FocusTransitionController : MonoBehaviour
{
    public static FocusTransitionController Instance { get; private set; }

    [Header("Mask Settings")]
    public Color maskColor = Color.black;
    public float closeDuration = 0.6f;   // 收拢用时
    public float openDuration = 0.6f;   // 展开用时
    public float feather = 0.15f;        // 边缘柔化（需与材质 _Feather 对应）
    public float startRadius = 1.5f;     // 一开始覆盖屏幕的“开”半径（>1基本就全黑外面）
    public float endRadius = 0.0f;       // 关闭时的目标半径（到点）

    private Canvas _canvas;
    private Image _image;
    private Material _mat;
    private int _idRadius, _idCenter, _idFeather, _idColor;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        BuildOverlay();
    }

    void BuildOverlay()
    {
        // 创建全屏 Canvas + Image（默认隐藏）
        _canvas = new GameObject("FocusTransitionCanvas").AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 9999; // 顶层
        DontDestroyOnLoad(_canvas.gameObject);

        var rt = new GameObject("Mask").AddComponent<RectTransform>();
        rt.SetParent(_canvas.transform, false);
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;

        _image = rt.gameObject.AddComponent<Image>();
        // 需要你把上面 shader 生成一个材质，或在代码里动态创建
        Shader s = Shader.Find("UI/CircularHole");
        _mat = new Material(s);
        _image.material = _mat;
        _image.raycastTarget = false;
        _image.enabled = false;

        _idRadius = Shader.PropertyToID("_Radius");
        _idCenter = Shader.PropertyToID("_Center");
        _idFeather = Shader.PropertyToID("_Feather");
        _idColor = Shader.PropertyToID("_Color");

        _mat.SetFloat(_idFeather, feather);
        _mat.SetColor(_idColor, maskColor);
        _mat.SetVector(_idCenter, new Vector4(0.5f, 0.5f, 0, 0));
        _mat.SetFloat(_idRadius, startRadius);
    }

    /// <summary>
    /// 进行聚焦转场：以 worldPos（来自当前场景目标物体）为圆心收拢，切场后从新中心展开
    /// </summary>
    public void PlayFocusTransition(string nextSceneName, Vector3 worldPos, Camera worldCamera, Vector2? openCenterViewport = null)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("FocusTransitionController not active.");
            return;
        }
        StartCoroutine(CoPlay(nextSceneName, worldPos, worldCamera, openCenterViewport));
    }

    IEnumerator CoPlay(string nextScene, Vector3 worldPos, Camera cam, Vector2? openCenterViewport)
    {
        _image.enabled = true;

        // 1) 收拢：以“当前场景指定物体”的屏幕位置为中心
        Vector3 v = cam.WorldToViewportPoint(worldPos);
        Vector2 center = new Vector2(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y));
        _mat.SetVector(_idCenter, new Vector4(center.x, center.y, 0, 0));
        _mat.SetFloat(_idFeather, feather);
        _mat.SetColor(_idColor, maskColor);

        yield return AnimateRadius(startRadius, endRadius, closeDuration);

        // 2) 异步加载下一场景
        var op = SceneManager.LoadSceneAsync(nextScene);
        while (!op.isDone) yield return null;

        // 3) 展开：可选指定新的圆心（不传则居中）
        Vector2 openCenter = openCenterViewport ?? new Vector2(0.5f, 0.5f);
        _mat.SetVector(_idCenter, new Vector4(openCenter.x, openCenter.y, 0, 0));

        yield return AnimateRadius(endRadius, startRadius, openDuration);

        // 完成后隐藏遮罩
        _image.enabled = false;
    }

    IEnumerator AnimateRadius(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // 不受 Time.timeScale 影响
            float k = Mathf.Clamp01(t / duration);
            // 你可以把 Mathf.SmoothStep 换成更顺滑的缓动
            float r = Mathf.SmoothStep(from, to, k);
            _mat.SetFloat(_idRadius, r);
            yield return null;
        }
        _mat.SetFloat(_idRadius, to);
    }
}
