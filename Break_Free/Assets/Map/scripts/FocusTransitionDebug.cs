using UnityEngine;

public class FocusTransitionDebug : MonoBehaviour
{
    public KeyCode key = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            var ctrl = FocusTransitionController.Instance;
            if (ctrl == null) { Debug.LogWarning("No FocusTransitionController in scene."); return; }

            // 用当前摄像机中心作为聚焦点，触发一个“假装跳场”的动画：收→开（不换场）
            StartCoroutine(TestPulse(ctrl));
        }
    }

    System.Collections.IEnumerator TestPulse(FocusTransitionController ctrl)
    {
        // 暴露一下材质并强制显示
        var fi = typeof(FocusTransitionController).GetField("_image",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var img = (UnityEngine.UI.Image)fi.GetValue(ctrl);
        img.enabled = true;

        // 收拢
        var fAnimate = typeof(FocusTransitionController).GetMethod("AnimateRadius",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        yield return (System.Collections.IEnumerator)fAnimate.Invoke(ctrl, new object[] { 1.5f, 0f, 0.5f });
        // 展开
        yield return (System.Collections.IEnumerator)fAnimate.Invoke(ctrl, new object[] { 0f, 1.5f, 0.5f });

        img.enabled = false;
    }
}
