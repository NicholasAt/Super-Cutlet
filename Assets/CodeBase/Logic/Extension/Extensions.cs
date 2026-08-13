namespace CodeBase.Logic.Extension
{
    public static class Extensions
    {
        public static string ToLevelTime(this float seconds) =>
            $"{seconds * 100f:0:00}";
    }
}