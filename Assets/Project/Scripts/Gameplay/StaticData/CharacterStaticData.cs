using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Characteristics;

namespace UndergroundFortress.Scripts.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Static data/Character")]
    public class CharacterStaticData : ScriptableObject
    {
        public MainCharacteristics mainCharacteristics;
    }
}