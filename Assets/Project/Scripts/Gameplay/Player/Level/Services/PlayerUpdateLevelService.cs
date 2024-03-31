using System;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;

namespace UndergroundFortress.Gameplay.Player.Level.Services
{
    public class PlayerUpdateLevelService : IPlayerUpdateLevelService, IWritingProgress
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;

        private PlayerLevelData _playerLevelData;
        private SkillPointsData _skillPointsData;

        public PlayerUpdateLevelService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _playerLevelData = progress.LevelData;
            _skillPointsData = progress.SkillPointsData;
        }

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.SaveProgress();
        }

        public void IncreaseExperience(int value)
        {
            var levelStaticData = _staticDataService.GetPlayerLevelByCurrent(_playerLevelData.Level);
            
            if (levelStaticData == null)
                return;
            
            int targetExperience = levelStaticData.targetExperience;
            int leftExperience = targetExperience - _playerLevelData.CurrentExperience - value;

            if (leftExperience <= 0)
            {
                _playerLevelData.Level++;
                _playerLevelData.CurrentExperience = Math.Abs(leftExperience);

                _skillPointsData.Received += levelStaticData.numberSkillPoints;
            }
            else
            {
                _playerLevelData.CurrentExperience += value;
            }
            
            WriteProgress();
        }
    }
}