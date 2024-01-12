using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Items.Equipment;

namespace UndergroundFortress.UI.Information
{
    public class InformationView : MonoBehaviour
    {
        [SerializeField] private List<EquipmentView> equipmentQualities;
        [SerializeField] private List<EquipmentView> equipmentSetQualities;
        
        private EquipmentView _currentEquipmentView;
        
        public void Initialize()
        {
            foreach (EquipmentView equipmentView in equipmentQualities) 
                equipmentView.Initialize();
            
            foreach (EquipmentView equipmentView in equipmentSetQualities) 
                equipmentView.Initialize();
        }

        public void ShowEquipment(EquipmentData equipmentData, in Vector3 position)
        {
            _currentEquipmentView?.Hide();

            if (!equipmentData.IsSet)
            {
                _currentEquipmentView = equipmentQualities[(int)equipmentData.Quality - 1];
                equipmentQualities[(int)equipmentData.Quality - 1].Show(equipmentData);
            }
            else
            {
                _currentEquipmentView = equipmentSetQualities[(int)equipmentData.Quality - 1];
                equipmentSetQualities[(int)equipmentData.Quality - 1].Show(equipmentData);
            }

            _currentEquipmentView.transform.position = position;
        }
    }
}