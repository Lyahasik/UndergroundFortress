namespace UndergroundFortress.Scripts.Core.Services.Scene
{
    public interface ISceneProviderService : IService
    {
        void LoadMainScene();
        void LoadLevel(string sceneName);
    }
}