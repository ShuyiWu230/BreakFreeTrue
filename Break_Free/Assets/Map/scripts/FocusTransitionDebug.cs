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

            // �õ�ǰ�����������Ϊ�۽��㣬����һ������װ�������Ķ������ա�������������
            StartCoroutine(TestPulse(ctrl));
        }
    }

    System.Collections.IEnumerator TestPulse(FocusTransitionController ctrl)
    {
        // ��¶һ�²��ʲ�ǿ����ʾ
        var fi = typeof(FocusTransitionController).GetField("_image",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var img = (UnityEngine.UI.Image)fi.GetValue(ctrl);
        img.enabled = true;

        // ��£
        var fAnimate = typeof(FocusTransitionController).GetMethod("AnimateRadius",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        yield return (System.Collections.IEnumerator)fAnimate.Invoke(ctrl, new object[] { 1.5f, 0f, 0.5f });
        // չ��
        yield return (System.Collections.IEnumerator)fAnimate.Invoke(ctrl, new object[] { 0f, 1.5f, 0.5f });

        img.enabled = false;
    }
}
