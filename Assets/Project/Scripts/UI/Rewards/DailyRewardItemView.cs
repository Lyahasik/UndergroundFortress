using TMPro;
using UnityEngine;

namespace UndergroundFortress.UI.Information
{
    public class DailyRewardItemView : RewardItemView
    {
        [Space]
        [SerializeField] private TMP_Text numberDayText;
        
        [Space]
        [SerializeField] private GameObject activeMarker;
        [SerializeField] private GameObject actualMarker;
        
        public void UpdateValues(int numberDay)
        {
            numberDayText.text = numberDay.ToString();
        }

        public void UpdateState(bool isActive, bool isActual)
        {
            activeMarker.SetActive(isActive);
            actualMarker.SetActive(isActual);
        }
    }
}