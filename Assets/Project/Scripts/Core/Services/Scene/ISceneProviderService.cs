namespace UndergroundFortress.Core.Services.Scene
{
    public interface ISceneProviderService : IService
    {
        public void LoadMainScene();
        public void LoadLevel(int idDungeon, int idLevel);
    }
}