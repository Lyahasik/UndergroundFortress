using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Skills.Services;
using UndergroundFortress.Gameplay.StaticData;
using UnityEngine.Events;

namespace UndergroundFortress.UI.Skills
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameSkill;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text description;

        [Space]
        [SerializeField] private Image statIcon;
        [SerializeField] private TMP_Text statValue;

        [Space]
        [SerializeField] private Button confirmButton;

        protected IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;
        private ISkillsUpgradeService _skillsUpgradeService;

        protected SkillsType _currentSkillsType;
        protected SkillData _currentSkillData;

        public void Construct(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            ISkillsUpgradeService skillsUpgradeService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
            _skillsUpgradeService = skillsUpgradeService;
        }

        public void Initialize(UnityAction onClose)
        {
            confirmButton.onClick.AddListener(onClose);
            confirmButton.onClick.AddListener(ActivationSkill);
        }

        public void Show(SkillsType skillsType, SkillData skillData, bool isCanUpgrade)
        {
            _currentSkillsType = skillsType;
            _currentSkillData = skillData;
            
            icon.sprite = skillData.data.icon;
            nameSkill.text = _localizationService.LocaleSkill(skillData.data.name);
            description.text = _localizationService.LocaleSkill(skillData.data.description);

            statIcon.sprite = _staticDataService.GetStatByType(skillData.data.statType).icon;
            statValue.text = skillData.data.statType.IncreaseIndicatorToString(skillData.data.value);
            
            gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(isCanUpgrade);
            confirmButton.interactable = _skillsUpgradeService.IsEnoughPoints;
        }
        
        public void Hide()
        {
            Reset();
            gameObject.SetActive(false);
        }

        private void ActivationSkill()
        {
            _skillsUpgradeService.ActivationSkill(_currentSkillsType, _currentSkillData.id);
        }

        private void Reset()
        {
            nameSkill.text = "Empty";
            statIcon.sprite = null;
            statValue.text = string.Empty;
        }
    }
}