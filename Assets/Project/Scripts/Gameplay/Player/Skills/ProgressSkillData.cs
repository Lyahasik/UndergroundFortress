namespace UndergroundFortress.Core.Progress
{
    public class ProgressSkillData
    {
        public int CurrentLevel;
        public int CurrentProgress;

        public ProgressSkillData(int level, int progress)
        {
            CurrentLevel = level;
            CurrentProgress = progress;
        }
    }
}