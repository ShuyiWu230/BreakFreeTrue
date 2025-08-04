using UnityEngine;

public class FlipSoundPlayer : MonoBehaviour
{
    public AudioClip flipSound;
    private AudioSource audioSource;

    void Awake()
    {
        // 添加或获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    public void PlayFlipSound()
    {
        if (flipSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(flipSound);
        }
    }
}
