using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Background Music")]
    public AudioClip music;

    [Header("Player SFX")]
    public AudioClip playerJump;
    public AudioClip playerHit;
    public AudioClip playerDeath;

    [Header("Object SFX")]
    public AudioClip doorSound;
    public AudioClip switchSound;
    public AudioClip pressurePlateSound;
    public AudioClip batuSound;
    public AudioClip shurikenSound;

    [Header("Enemy SFX")]
    public AudioClip enemyDeath;
    public AudioClip tawonSound;

    private void Awake()
    {
        // Agar AudioManager tidak dobel saat pindah scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (musicSource != null && music != null)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}