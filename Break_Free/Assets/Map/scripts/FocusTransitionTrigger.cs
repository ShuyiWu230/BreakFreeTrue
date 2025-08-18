using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FocusTransitionTrigger2D : MonoBehaviour
{
    public string nextSceneName;
    public Transform focusTarget;     // 聚焦点（通常是自己）
    public string playerTag = "Player";

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
        if (focusTarget == null) focusTarget = transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        Camera cam = Camera.main;
        if (cam == null) { Debug.LogWarning("No MainCamera found for FocusTransitionTrigger2D."); return; }

        Vector3 worldPos = focusTarget.position;
        FocusTransitionController.Instance?.PlayFocusTransition(nextSceneName, worldPos, cam, null);
    }
}
