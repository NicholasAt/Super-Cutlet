namespace CodeBase.Services.Analytic
{
    public interface IAnalytics : IService
    {
        void Leave(string levelName, float time);
        void LevelCompleted(string levelName, float time);
    }
}