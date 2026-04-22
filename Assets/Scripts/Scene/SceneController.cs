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
    public static string PP_DIFICULTY = "Difficulty";

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger<int>.AddListener(GameEvent.PICKUP_KEYCARD, OnKeyCardChanged);
        Messenger.AddListener(GameEvent.ENEMY_DEAD, OnEnemeyDead);
        Messenger.AddListener(GameEvent.PICKUP_FLOPPY, OnWinGame);
        Messenger<WeaponType>.AddListener(GameEvent.ATTACK, OnAttackEnemy);
        Messenger<RaycastHit>.AddListener(GameEvent.PLAYER_BULLET_HIT, OnPlayerBulletHit);
    }

    private void Start()
    {
        OnDifficultyChanged(PlayerPrefs.GetInt(PP_DIFICULTY, 1));
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        timeElapsed += Time.deltaTime;
        uiManager.UpdateTimerUI(timeElapsed);
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
    public void OnPlayerBulletHit(RaycastHit hit) { }
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
                SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxSniper);
                break;
            case WeaponType.Pistol:
                SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxPistol);
                break;
            case WeaponType.Knife:
                SoundManager.Instance.PlaySfx(SoundLibrary.Instance.sfxKnife);
                break;

        }
    }

    private void OnDifficultyChanged(int value)
    {

        BaseExplosive[] explosives = GameObject.FindObjectsByType<BaseExplosive>(FindObjectsSortMode.None);
        for (int i = 0; i < explosives.Length; i++)
        {
            BaseExplosive e = explosives[i];
            e.SetExplosionRadius(value);
        }
        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy e = enemies[i];
            e.SetExplosionRadius(value);
        }
    }
    public int GetDifficulty()
    {
        return PlayerPrefs.GetInt(PP_DIFICULTY, 1);
    }
    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger<int>.RemoveListener(GameEvent.PICKUP_KEYCARD, OnKeyCardChanged);
        Messenger.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemeyDead);
        Messenger.RemoveListener(GameEvent.PICKUP_FLOPPY, OnWinGame);
        Messenger<WeaponType>.RemoveListener(GameEvent.ATTACK, OnAttackEnemy);
        Messenger<RaycastHit>.RemoveListener(GameEvent.PLAYER_BULLET_HIT, OnPlayerBulletHit);
    }
}
