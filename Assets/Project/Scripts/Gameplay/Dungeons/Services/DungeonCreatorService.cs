﻿using System.Collections.Generic;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public class DungeonCreatorService : IDungeonCreatorService, IWritingProgress
    {
        private readonly IProgressProviderService _progressProviderService;

        public DungeonCreatorService(IProgressProviderService progressProviderService)
        {
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

        public void LoadProgress(ProgressData progress) {}

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.SaveProgress();
        }

        public void SuccessDungeonLevel(int idDungeon, int idLevel)
        {
            var dungeons = _progressProviderService.ProgressData.Dungeons;

            if (idLevel == ConstantValues.MAX_DUNGEON_LEVEL_ID)
                dungeons[idDungeon + 1] = new HashSet<int> { 0 };
            else
                dungeons[idDungeon].Add(idLevel + 1);
            
            WriteProgress();
        }
    }
}