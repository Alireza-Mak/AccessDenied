using UnityEditor;

[InitializeOnLoad]
public static class GameSession
{
    public static string PlayerName = "";
    public static string DEFAULT_PLAYER_NAME_VALUE = "Player123";
    public static bool HasStartedBefore = false;


    public static void ResetSession()
    {
        PlayerName = "";
        HasStartedBefore = false;
    }

#if UNITY_EDITOR
    static GameSession()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            ResetSession();
        }
    }
#endif

}
