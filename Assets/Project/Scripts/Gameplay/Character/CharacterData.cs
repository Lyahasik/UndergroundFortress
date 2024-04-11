using UnityEngine;

namespace UndergroundFortress.Gameplay.Character
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

        public void Initialize()
        {
            stunned.Initialize(RemoveHitEffect);
        }

        public virtual void TakeHitEffect() {}
        
        public virtual void MakeHitEffect() {}

        public virtual void RemoveHitEffect()
        {
            
        }
    }
}
