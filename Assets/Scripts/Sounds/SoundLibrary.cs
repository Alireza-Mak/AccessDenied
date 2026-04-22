using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    public static SoundLibrary Instance { get; private set; }
    [SerializeField] public AudioClip sfxPistol;
    [SerializeField] public AudioClip sfxKnife;
    [SerializeField] public AudioClip sfxSniper;
    [SerializeField] public AudioClip sfxCollectAmmo;
    [SerializeField] public AudioClip sfxCollectHealth;
    [SerializeField] public AudioClip sfxCollectKey;
    [SerializeField] public AudioClip sfxCollectFloppy;
    [SerializeField] public AudioClip sfxSwapWeapon;
    [SerializeField] public AudioClip sfxZoom;
    [SerializeField] public AudioClip explosion;
    [SerializeField] public AudioClip beep;
    [SerializeField] public AudioClip sfxHit;
    [SerializeField] public AudioClip sfxEnemyDead;

    [SerializeField] public AudioClip music1;
    [SerializeField] public AudioClip music2;
    [SerializeField] public AudioClip music3;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else { Destroy(this); }
    }
}

