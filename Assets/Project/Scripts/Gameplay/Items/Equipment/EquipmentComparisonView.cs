using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;

namespace UndergroundFortress.Gameplay.Items.Equipment
{
    public class EquipmentComparisonView : MonoBehaviour
    {
        [SerializeField] private EquipmentView equipmentView1;
        [SerializeField] private EquipmentView equipmentView2;

        public void Initialize(IStaticDataService staticDataService)
        {
            equipmentView1.Construct(staticDataService);
            equipmentView2.Construct(staticDataService);
        }

        public void Show(ItemData equipmentData1, ItemData equipmentData2)
        {
            equipmentView1.Show(equipmentData1);
            equipmentView2.Show(equipmentData2);
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            equipmentView1.Hide();
            equipmentView2.Hide();
        }
    }
}
