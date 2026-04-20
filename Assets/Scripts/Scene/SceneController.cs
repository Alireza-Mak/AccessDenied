using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class SceneController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    private int keyCards = 0;
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

    }

    private void OnDestroy()
    {
        //Messenger<int>.RemoveListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
        Messenger<int>.RemoveListener(GameEvent.PICKUP_KEYCARD, OnKeyCardChanged);
        Messenger.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemeyDead);
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        timeElapsed += Time.deltaTime;
        uiManager.UpdateTimerUI(timeElapsed);
    }
    void OnKeyCardChanged(int value)
    {
        keyCards += value;
        uiManager.UpdateKeyCard(keyCards / (float)MAX_KEY_CARDS);
        if (keyCards == MAX_KEY_CARDS)
        {
            OnWinGame();
        }
    }
    private void OnWinGame()
    {
        isTimerRunning = false;
        uiManager.ShowWinPopup(timeElapsed, keyCards);
    }
    private void OnPlayerDead()
    {
        isTimerRunning = false;
        uiManager.ShowGameOverPopup(timeElapsed, keyCards);
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
}
