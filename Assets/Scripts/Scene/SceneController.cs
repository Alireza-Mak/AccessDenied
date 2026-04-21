using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    public int KeyCards { get; private set; } = 0;
    public int MAX_KEY_CARDS = 3;
    public bool isTimerRunning = true;
    private float timeElapsed = 0f;
    private int score = 0;

    private void Awake()
    {
        //Messenger<int>.AddListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger<int>.AddListener(GameEvent.PICKUP_KEYCARD, OnKeyCardChanged);
        Messenger.AddListener(GameEvent.ENEMY_DEAD, OnEnemeyDead);
        Messenger.AddListener(GameEvent.PICKUP_FLOPPY, OnWinGame);
        Messenger<WeaponType>.AddListener(GameEvent.ATTACK, OnAttackEnemy);
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        timeElapsed += Time.deltaTime;
        uiManager.UpdateTimerUI(timeElapsed);
    }

    public void OnPlaySfx(AudioClip clip)
    {
        SoundManager.Instance.PlaySfx(clip);
    }

    void OnKeyCardChanged(int value)
    {
        KeyCards += value;
        uiManager.UpdateKeyCard(KeyCards / (float)MAX_KEY_CARDS);

    }

    private void OnWinGame()
    {
        if (KeyCards == MAX_KEY_CARDS)
        {
            isTimerRunning = false;
            uiManager.ShowWinPopup(timeElapsed, KeyCards, score);
        }

    }

    private void OnPlayerDead()
    {
        isTimerRunning = false;
        uiManager.ShowGameOverPopup(timeElapsed, KeyCards, score);
    }

    private void OnRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnEnemeyDead()
    {
        score++;
        uiManager.UpdateScore(score);
    }

    void OnAttackEnemy(WeaponType wt)
    {
        switch (wt)
        {
            case WeaponType.Sniper:
                OnPlaySfx(SoundLibrary.Instance.sfxSniper);
                break;
            case WeaponType.Pistol:
                OnPlaySfx(SoundLibrary.Instance.sfxPistol);
                break;
            case WeaponType.Knife:
                OnPlaySfx(SoundLibrary.Instance.sfxKnife);
                break;

        }
    }

    private void OnDestroy()
    {
        //Messenger<int>.RemoveListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger<int>.RemoveListener(GameEvent.PICKUP_KEYCARD, OnKeyCardChanged);
        Messenger.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemeyDead);
        Messenger.RemoveListener(GameEvent.PICKUP_FLOPPY, OnWinGame);
        Messenger<WeaponType>.RemoveListener(GameEvent.ATTACK, OnAttackEnemy);
    }
}
