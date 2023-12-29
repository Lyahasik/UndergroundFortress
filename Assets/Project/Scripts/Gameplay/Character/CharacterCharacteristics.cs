using UndergroundFortress.Scripts.Gameplay.Characteristics;

namespace UndergroundFortress.Scripts.Gameplay.Character
{
    public class CharacterCharacteristics
    {
        private MainCharacteristics _mainCharacteristics;
        private RealtimeCharacteristics _realtimeCharacteristics;

        public CharacterCharacteristics(MainCharacteristics mainCharacteristics,
            RealtimeCharacteristics realtimeCharacteristics)
        {
            _mainCharacteristics = mainCharacteristics;
            _realtimeCharacteristics = realtimeCharacteristics;
        }

        public MainCharacteristics MainCharacteristics => _mainCharacteristics;
        public RealtimeCharacteristics RealtimeCharacteristics => _realtimeCharacteristics;
    }
}