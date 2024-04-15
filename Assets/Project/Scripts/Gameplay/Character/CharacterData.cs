using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Character
{
    public abstract class CharacterData : MonoBehaviour
    {
        [SerializeField] private Stunned stunned;
        
        private CharacterStats _stats;

        public CharacterStats Stats => _stats;

        public void Construct(CharacterStats stats)
        {
            _stats = stats;
        }

        public virtual void Initialize()
        {
            stunned.Initialize(RemoveHitEffect);
        }

        public virtual void ActivateStun(float duration)
        {
            stunned.Activate(duration);
            _stats.IsFreeze = true;
        }

        public abstract void TakeHitEffect(StatType hitType);
        public abstract void AttackEffect(StatType attackType);

        public virtual void RemoveHitEffect()
        {
            _stats.IsFreeze = false;
        }

        public virtual void StartDead() {}

        public virtual void Dead()
        {
            Destroy(gameObject);
        }
    }
}
