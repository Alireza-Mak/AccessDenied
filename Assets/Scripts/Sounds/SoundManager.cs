using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    //// STATIC VARS
    static public SoundManager Instance { get; private set; } = null;

    //// MEMBER VARS
    [SerializeField] private AudioSource sfxSource;     // for playing sfx
    [SerializeField] private AudioSource musicSource;   // for playing music
    [SerializeField] private AudioMixer mixer;

    private float sfxVolume = 1.0f;     // for tracking sfx volume
    private float musicVolume = 1.0f;   // for tracking music volume

    public static string PP_MUSIC_VOL = "MusicVol";
    public static string PP_SFX_VOL = "SfxVol";
    public static string PP_MUSIC_INX = "MusicIndex";

    //// MEMBER PROPERTIES
    //// a property to get/set sfx volume
    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            mixer.SetFloat("SfxVolume", LinearToLog(sfxVolume));
        }
    }

    //// a property to get/set music volume
    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            mixer.SetFloat("MusicVolume", LinearToLog(musicVolume));
        }
    }

    private void Awake()
    {
        if (Instance == null)                    // if Awake() has never been called before
        {
            Instance = this;                    // remember this as our (one & only) SM
            DontDestroyOnLoad(this.gameObject); // don't destroy this gameObject when a new scene loads
            Init();                             // initialize the SM
        }
        else
        {                           // else we already have a SM that exists.
            Destroy(gameObject);    // destroy the SM that was about to be built
        }
    }
    private void Start()
    {
        int musicIndex = PlayerPrefs.GetInt(PP_MUSIC_INX, 0);
        AudioClip[] musics =
{
        SoundLibrary.Instance.music1,
        SoundLibrary.Instance.music2,
        SoundLibrary.Instance.music3
    };
        PlayMusic(musics[musicIndex]);
        Init();
    }

    private void Init()
    {
        // Restore volume slider values [0..1] from PlayerPrefs
        MusicVolume = PlayerPrefs.GetFloat(PP_MUSIC_VOL, 1f);   // if not found, use 1
        SfxVolume = PlayerPrefs.GetFloat(PP_SFX_VOL, 1f);       // if not found, use 1
    }

    // Play a sfx clip (fire & forget)
    public void PlaySfx(AudioClip clip, float volume = 1.0f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    // Play a music clip (capable of being stopped)
    public void PlayMusic(AudioClip clip, float volume = 1.0f)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    // Stop the music!
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // convert from linear to logarithmic scale ([0...1] to decibels)
    private float LinearToLog(float value)
    {
        return Mathf.Log10(value) * 20;
    }
}
