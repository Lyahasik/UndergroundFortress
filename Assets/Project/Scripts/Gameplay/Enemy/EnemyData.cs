using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.Gameplay.Character
{
    public class EnemyData : CharacterData
    {
        [SerializeField] private Image skin;

        public override void TakeHitEffect()
        {
            
        }
        
        public override void MakeHitEffect() {}

        public override void RemoveHitEffect()
        {
            
        }
    }
}