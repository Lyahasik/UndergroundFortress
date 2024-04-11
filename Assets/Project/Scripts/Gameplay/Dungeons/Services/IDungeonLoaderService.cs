using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public interface IDungeonLoaderService : IService
    {
        public void Initialize();
        public void LoadLevel(int idDungeon, int idLevel);
        public void SuccessDungeonLevel(int idDungeon, int idLevel);
    }
}