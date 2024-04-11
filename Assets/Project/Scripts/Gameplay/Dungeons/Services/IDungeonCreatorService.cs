using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public interface IDungeonCreatorService : IService
    {
        public void Initialize();
        public void SuccessDungeonLevel(int idDungeon, int idLevel);
    }
}