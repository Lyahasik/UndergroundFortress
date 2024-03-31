using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.Information.Services;

namespace UndergroundFortress.UI.Skills
{
    [RequireComponent(typeof(Button))]
    public class SkillButtonView : MonoBehaviour, IReadingProgress
    {
        [SerializeField] protected int id;
        [SerializeField] private int unlockId;
        
        [Space]
        [SerializeField] private Image icon;

        [Space]
        [SerializeField] protected GameObject noActiveImage;
        [SerializeField] private GameObject lockImage;

        protected IStaticDataService _staticDataService;
        protected IInformationService _informationService;
        protected SkillsType _skillsType;
        protected SkillData _skillData;

        public int Id => id;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ShowInformation);
        }

        public void Construct(IStaticDataService staticDataService,
            IInformationService informationService,
            SkillsType skillsType,
            SkillData skillData)
        {
            _staticDataService = staticDataService;
            _informationService = informationService;
            _skillsType = skillsType;
            _skillData = skillData;
        }

        public void Initialize(IProgressProviderService progressProviderService)
        {
            icon.sprite = _skillData.icon;
            
            Register(progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            UpdateProgress(progress);
        }

        public virtual void UpdateProgress(ProgressData progress)
        {
            bool isActive = progress.ActiveSkills[_skillsType].Contains(id);
            bool isUnlocked = progress.ActiveSkills[_skillsType].Contains(unlockId);
            
            noActiveImage.SetActive(isUnlocked && !isActive);
            lockImage.SetActive(!isUnlocked);
        }

        protected virtual void ShowInformation()
        {
            _informationService.ShowSkill(_skillsType, _skillData, isCanUpgrade: noActiveImage.activeSelf);
        }
    }
}