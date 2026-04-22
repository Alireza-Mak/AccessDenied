using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : BasePopup
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private TextMeshProUGUI difficultyLabel;

    private float previousMusicVolume;
    private float previousSoundVolume;
    private int previousDifficulty;
    private int previousMusicIndex;
    private int currentMusicIndex = 0;
    private AudioClip[] musics;

    private void Awake()
    {
        musics = new AudioClip[]
        {
            SoundLibrary.Instance.music1,
            SoundLibrary.Instance.music2,
            SoundLibrary.Instance.music3
        };
    }
    public override void Open()
    {
        previousMusicVolume = PlayerPrefs.GetFloat(SoundManager.PP_MUSIC_VOL, 0);
        previousSoundVolume = PlayerPrefs.GetFloat(SoundManager.PP_SFX_VOL, 0);
        previousMusicIndex = PlayerPrefs.GetInt(SoundManager.PP_MUSIC_INX, 0);
        previousDifficulty = PlayerPrefs.GetInt(SceneController.PP_DIFICULTY, 0);

        currentMusicIndex = previousMusicIndex;

        musicSlider.value = previousMusicVolume;
        soundSlider.value = previousSoundVolume;
        difficultySlider.value = previousDifficulty;
        difficultyLabel.text = "Difficulty: " + previousDifficulty.ToString();
        base.Open();

    }

    public void OnOKButton()
    {
        PlayerPrefs.SetFloat(SoundManager.PP_MUSIC_VOL, musicSlider.value);
        PlayerPrefs.SetFloat(SoundManager.PP_SFX_VOL, soundSlider.value);
        PlayerPrefs.SetInt(SoundManager.PP_MUSIC_INX, currentMusicIndex);
        PlayerPrefs.SetInt(SceneController.PP_DIFICULTY, (int)difficultySlider.value);

        Messenger<int>.Broadcast(GameEvent.DIFFICULTY_CHANGED, (int)difficultySlider.value);

        OnCloseButton();
    }

    public void OnCancelButton()
    {
        difficultySlider.value = previousDifficulty;
        musicSlider.value = previousMusicVolume;
        soundSlider.value = previousSoundVolume;
        SoundManager.Instance.PlayMusic(musics[previousMusicIndex]);

        OnCloseButton();
    }
    public void OnNextMusic()
    {
        currentMusicIndex = (currentMusicIndex + 1) % musics.Length;
        SoundManager.Instance.PlayMusic(musics[currentMusicIndex]);
    }

    public void OnPreviousMusic()
    {
        currentMusicIndex--;

        if (currentMusicIndex < 0)
        {
            currentMusicIndex = musics.Length - 1;
        }

        SoundManager.Instance.PlayMusic(musics[currentMusicIndex]);
    }

    public void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance.MusicVolume = value;
    }

    public void OnSoundVolumeChanged(float value)
    {
        SoundManager.Instance.SfxVolume = value;
    }

    public void OnDifficultyChanged(float value)
    {
        difficultyLabel.text = "Difficulty: " + ((int)value).ToString();
    }
}