using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Core.Services.Characters
{
    public interface IPlayerDressingService : IService
    {
        public void PutOnAnItem(ItemData itemData);
        public void RemoveAnItem(ItemData itemData);
    }
}