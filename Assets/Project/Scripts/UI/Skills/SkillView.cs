using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private TMP_Text statValue;

        [Space]
        [SerializeField] private Button confirmButton;

        private ISkillsUpgradeService _skillsUpgradeService;
        
        protected SkillsType _currentSkillsType;
        protected SkillData _currentSkillData;

        public void Construct(ISkillsUpgradeService skillsUpgradeService)
        {
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
            
            icon.sprite = skillData.icon;
            nameSkill.text = skillData.name;
            description.text = skillData.description;
            statValue.text = skillData.statType.IncreaseIndicatorToString(skillData.value);
            
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
            statValue.text = string.Empty;
        }
    }
}