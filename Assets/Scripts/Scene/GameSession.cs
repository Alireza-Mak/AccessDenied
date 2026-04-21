public static class GameSession
{
    public static string PlayerName = "";
    public static bool HasStartedBefore = false;

    public static int Score = 0;
    public static float LastTime = 0f;
    public static int Difficulty = 0;

    public static void ResetSession()
    {
        PlayerName = "";
        HasStartedBefore = false;
        Score = 0;
        LastTime = 0f;
        Difficulty = 0;
    }
}