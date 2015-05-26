namespace WorldOfWords.Web.Common
{
    public static class Config
    {
        public const string Language = "bg";

        public const int MaxNumberOfEmptyBoards = 3;
        public const int FillingPercent = 70;

        // board size
        public const int MinSize = 5;
        public const int MaxSize = 10;

        // board life
        public const int MinDurationInMinutes = 5;
        // 1 day = 60 min * 24 h
        public const int MaxDurationInMinutes = 15;

        // Bonus coefficients for filling boards
        public const int FirstBonusLevelCoefficient = 2;
        public const int SecondBonusLevelCoefficient = 3;
        public const int ThirdBonusLevelCoefficient = 4;

        // Filling percents for each bonus level
        public const int FirstBonusLevel = 60;
        public const int SecondBonusLevel = 70;
        public const int ThirdBonusLevel = 85;
    }
}