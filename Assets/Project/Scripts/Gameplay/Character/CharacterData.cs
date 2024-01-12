using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Items.Equipment;

namespace UndergroundFortress.Gameplay.Character
{
    public class CharacterData : MonoBehaviour
    {
        [SerializeField] private Stunned stunned;
        
        private CharacterStats _stats;
        private List<EquipmentData> _equipments;

        public Stunned Stunned => stunned;
        public CharacterStats Stats => _stats;
        public List<EquipmentData> Equipments => _equipments;

        public void Construct(CharacterStats stats, List<EquipmentData> equipments)
        {
            _stats = stats;
            _equipments = equipments;
        }
    }
}
