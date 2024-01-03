using UnityEngine;

namespace UndergroundFortress.Scripts.Gameplay.Character
{
    public class CharacterData : MonoBehaviour
    {
        [SerializeField] private Stunned stunned;
        
        private CharacterStats _stats;

        public Stunned Stunned => stunned;
        public CharacterStats Stats => _stats;

        public void Construct(CharacterStats stats)
        {
            _stats = stats;
        }
        
        
    }
}
