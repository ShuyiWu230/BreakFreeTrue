using UnityEngine;

public class WaterAmbientSound : MonoBehaviour
{
    public Transform hourglass;  // ✅ 改成 public
    public float maxVolume = 0.8f;
    public float fadeSpeed = 2f;
    public float maxDistance = 5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (hourglass == null || audioSource == null) return;

        float distance = Vector2.Distance(hourglass.position, transform.position);
        float t = Mathf.Clamp01(1 - (distance / maxDistance));
        float targetVolume = t * maxVolume;

        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, Time.deltaTime * fadeSpeed);
    }
}
