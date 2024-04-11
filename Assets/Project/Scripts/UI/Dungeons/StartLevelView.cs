using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Dungeons.Services;

namespace UndergroundFortress.UI.MainMenu
{
    public class StartLevelView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private List<ListLevelsDungeon> listDungeons;

        private IProcessingPlayerStatsService _processingPlayerStatsService;
        private IDungeonLoaderService _dungeonLoaderService;
        
        private int _selectedDungeonId;

        public void Construct(IProcessingPlayerStatsService processingPlayerStatsService,
            IDungeonLoaderService dungeonLoaderService)
        {
            _processingPlayerStatsService = processingPlayerStatsService;
            _dungeonLoaderService = dungeonLoaderService;
        }

        public void Initialize(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            listDungeons.ForEach(data =>
            {
                data.Construct(staticDataService);
                data.Initialize(progressProviderService, UpdateSelectDungeon, StartLevel);
            });
        }
        
        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
        }

        private void UpdateSelectDungeon(int id)
        {
            _selectedDungeonId = id;
            
            listDungeons.ForEach(data => data.UpdateSelect(id));
        }

        private void StartLevel(int idLevel)
        {
            _dungeonLoaderService.LoadLevel(_selectedDungeonId, idLevel);
        }
    }
}