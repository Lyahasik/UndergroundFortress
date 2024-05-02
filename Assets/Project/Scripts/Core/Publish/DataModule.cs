namespace UndergroundFortress.Core.Publish
{
    public abstract class DataModule
    {
        public abstract void StartLoadData();
        public abstract void SaveData(string data);
        public abstract void SetLeaderBoard(int value);
    }
}
