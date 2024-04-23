using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Rewards;

namespace UndergroundFortress.UI.Information
{
    public class AccumulatedRewardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberCoinsText;

        [Space]
        [SerializeField] private Button confirmButton;

        private IAccumulationRewardsService _accumulationRewardsService;

        public void Construct(IAccumulationRewardsService accumulationRewardsService)
        {
            _accumulationRewardsService = accumulationRewardsService;
        }
        
        public void Initialize(UnityAction onClose)
        {
            confirmButton.onClick.AddListener(onClose);
            confirmButton.onClick.AddListener(ClaimRewards);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateValues(RewardsData rewardsData)
        {
            numberCoinsText.text = rewardsData.NumberCoins.ToString();
        }

        private void ClaimRewards()
        {
            _accumulationRewardsService.ClaimRewards();
        }
    }
}