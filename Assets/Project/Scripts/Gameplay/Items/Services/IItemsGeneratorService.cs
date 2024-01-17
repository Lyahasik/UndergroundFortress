using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Gameplay.Items.Services
{
    public interface IItemsGeneratorService : IService
    {
        public void GenerateResource();
    }
}