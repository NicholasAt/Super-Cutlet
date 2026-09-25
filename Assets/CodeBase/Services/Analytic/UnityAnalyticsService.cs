using CodeBase.Logic;
using Unity.Services.Analytics;
using static CodeBase.AnalyticsConstants;

namespace CodeBase.Services.Analytic
{
    public class UnityAnalyticsService : IAnalytics
    {
        public void LevelCompleted(string levelName, float time)
        {
            CustomEvent @event = new CustomEvent(LevelCompletedEvent)
            {
                { LevelNameParameter, levelName},
                { TimeParameter, time},
                { BrowserParameter, BrowserDetector.IsSafariBrowser()},
            };
            AnalyticsService.Instance.RecordEvent(@event);
        }
        public void Leave(string levelName, float time)
        {
            CustomEvent @event = new CustomEvent(LeaveEvent)
            {
                { LevelNameParameter, levelName},
                { TimeParameter, time},
                { BrowserParameter, BrowserDetector.IsSafariBrowser()},
            };
            AnalyticsService.Instance.RecordEvent(@event);
        }
    }
}