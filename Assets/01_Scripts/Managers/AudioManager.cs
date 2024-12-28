using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // Arka plan müziği AudioSource
    public AudioClip backgroundClip;    // Arka plan müzik dosyası

    private void Start()
    {
        if (backgroundMusic != null && backgroundClip != null)
        {
            backgroundMusic.clip = backgroundClip;
            backgroundMusic.loop = true; // Müziği sürekli çal
            backgroundMusic.Play();
        }
    }
}
