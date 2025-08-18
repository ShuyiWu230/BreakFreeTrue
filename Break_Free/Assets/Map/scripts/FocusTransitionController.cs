using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FocusTransitionController : MonoBehaviour
{
    public static FocusTransitionController Instance { get; private set; }

    [Header("Mask Settings")]
    public Color maskColor = Color.black;
    public float closeDuration = 0.6f;   // ��£��ʱ
    public float openDuration = 0.6f;   // չ����ʱ
    public float feather = 0.15f;        // ��Ե�ữ��������� _Feather ��Ӧ��
    public float startRadius = 1.5f;     // һ��ʼ������Ļ�ġ������뾶��>1������ȫ�����棩
    public float endRadius = 0.0f;       // �ر�ʱ��Ŀ��뾶�����㣩

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
        // ����ȫ�� Canvas + Image��Ĭ�����أ�
        _canvas = new GameObject("FocusTransitionCanvas").AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 9999; // ����
        DontDestroyOnLoad(_canvas.gameObject);

        var rt = new GameObject("Mask").AddComponent<RectTransform>();
        rt.SetParent(_canvas.transform, false);
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;

        _image = rt.gameObject.AddComponent<Image>();
        // ��Ҫ������� shader ����һ�����ʣ����ڴ����ﶯ̬����
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
    /// ���о۽�ת������ worldPos�����Ե�ǰ����Ŀ�����壩ΪԲ����£���г����������չ��
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

        // 1) ��£���ԡ���ǰ����ָ�����塱����Ļλ��Ϊ����
        Vector3 v = cam.WorldToViewportPoint(worldPos);
        Vector2 center = new Vector2(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y));
        _mat.SetVector(_idCenter, new Vector4(center.x, center.y, 0, 0));
        _mat.SetFloat(_idFeather, feather);
        _mat.SetColor(_idColor, maskColor);

        yield return AnimateRadius(startRadius, endRadius, closeDuration);

        // 2) �첽������һ����
        var op = SceneManager.LoadSceneAsync(nextScene);
        while (!op.isDone) yield return null;

        // 3) չ������ѡָ���µ�Բ�ģ���������У�
        Vector2 openCenter = openCenterViewport ?? new Vector2(0.5f, 0.5f);
        _mat.SetVector(_idCenter, new Vector4(openCenter.x, openCenter.y, 0, 0));

        yield return AnimateRadius(endRadius, startRadius, openDuration);

        // ��ɺ���������
        _image.enabled = false;
    }

    IEnumerator AnimateRadius(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // ���� Time.timeScale Ӱ��
            float k = Mathf.Clamp01(t / duration);
            // ����԰� Mathf.SmoothStep ���ɸ�˳���Ļ���
            float r = Mathf.SmoothStep(from, to, k);
            _mat.SetFloat(_idRadius, r);
            yield return null;
        }
        _mat.SetFloat(_idRadius, to);
    }
}
