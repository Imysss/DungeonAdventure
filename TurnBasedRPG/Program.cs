namespace TurnBasedRPG
{
    public enum SceneType
    {
        GameStartScene,
        GameMainScene,
        PlayerInfoScene,
        DunjeonScene,
        GameClearScene,
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Manager.Instance.Init();
            Manager.Instance.Scene.LoadScene(SceneType.GameStartScene);
        }
    }
}