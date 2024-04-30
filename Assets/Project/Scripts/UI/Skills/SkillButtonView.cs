using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Tutorial.Services;
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
        [SerializeField] private Color inactiveColor;

        [Space]
        [SerializeField] protected GameObject frame;

        protected IStaticDataService _staticDataService;
        protected IInformationService _informationService;
        private ProgressTutorialService _progressTutorialService;
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
            icon.sprite = _skillData.data.icon;
            
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
            
            frame.SetActive(isUnlocked && !isActive);
            icon.color = isUnlocked ? Color.white : inactiveColor;
        }

        protected virtual void ShowInformation()
        {
            _informationService.ShowSkill(_skillsType, _skillData, isCanUpgrade: frame.activeSelf, isCapping: !TryCheckTutorial());
        }

        public void ActivateTutorial(ProgressTutorialService progressTutorialService)
        {
            _progressTutorialService = progressTutorialService;
        }

        public void DeactivateTutorial()
        {
            _progressTutorialService = null;
        }

        private bool TryCheckTutorial()
        {
            if (_progressTutorialService == null)
                return false;
            
            _progressTutorialService.SuccessStep();
            return true;
        }
    }
}