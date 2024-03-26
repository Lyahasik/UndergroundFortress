using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Core.Services.Progress
{
    public partial class ProgressProviderService : IProgressProviderService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameStateMachine _gameStateMachine;

        private ProgressData _progressData;

        private List<IReadingProgress> _progressReaders;
        private List<IWritingProgress> _progressWriters;

        public ProgressData ProgressData => _progressData;

        public ProgressProviderService(IStaticDataService staticDataService,
            IGameStateMachine gameStateMachine)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public void Initialization()
        {
            _progressReaders ??= new List<IReadingProgress>();
            _progressWriters ??= new List<IWritingProgress>();
        }

        public void LoadProgress()
        {
            Debug.Log("Loaded progress.");
            
            _progressData = LoadData(PlayerPrefs.GetString(ConstantValues.KEY_LOCAL_PROGRESS)) ?? CreateNewProgress();
            
            foreach (IReadingProgress progressReader in _progressReaders)
                progressReader.LoadProgress(_progressData);

            _gameStateMachine.Enter<LoadSceneState>();
        }
        
        private ProgressData LoadData(string json)
        {
            ProgressData progressData = null;
                
            if (json != null)
                progressData = JsonConvert.DeserializeObject<ProgressData>(json);

            return progressData;
        }

        private ProgressData CreateNewProgress()
        {
            ProgressData progressData = new ProgressData
            {
                Level = 3,
                MainStats = new Dictionary<StatType, float>(),
                
                Wallet = new WalletData(500, 50),
                Equipment = new List<CellData>
                {
                    new(null, 1),
                    new(null, 1),
                    new(null, 1),
                    new(null, 1),
                    new(null, 1),
                    new(null, 1),
                    new(null, 1),
                    new(null, 1),
                    new(null, 1)
                },
                ActiveRecipes = new Dictionary<ItemType, List<int>>
                {
                    { ItemType.Sword, new List<int>() },
                    { ItemType.TwoHandedWeapon, new List<int>() },
                    { ItemType.Dagger, new List<int>() },
                    { ItemType.Mace, new List<int>() },
                    { ItemType.Shield, new List<int>() },
                    { ItemType.Chest, new List<int>() },
                    { ItemType.Pants, new List<int>() },
                    { ItemType.Boots, new List<int>() },
                    { ItemType.Gloves, new List<int>() },
                    { ItemType.Consumable, new List<int>() }
                },
                Bag = new List<CellData>(),
                FilledNumberBag = 0
            };

            foreach (StatType type in (StatType[]) Enum.GetValues(typeof(StatType)))
                progressData.MainStats.Add(type, 0f);
            
            for (int i = 0; i < ConstantValues.BASE_SIZE_BAG; i++) 
                progressData.Bag.Add(new CellData());
            
            SaveProgress();

            return progressData;
        }

        public void SaveProgress()
        {
            if (_progressData == null)
                return;
            
            UpdateProgress();
            
            string json = JsonConvert.SerializeObject(_progressData, new JsonSerializerSettings());
            PlayerPrefs.SetString(ConstantValues.KEY_LOCAL_PROGRESS, json);
        }

        public void Register(IReadingProgress progressReader)
        {
            _progressReaders.Add(progressReader);
            
            if (_progressData != null)
                progressReader.LoadProgress(ProgressData);
        }

        public void Register(IWritingProgress progressWriter)
        {
            Register(progressWriter as IReadingProgress);

            _progressWriters.Add(progressWriter);
        }

        private void UpdateProgress()
        {
            foreach (IReadingProgress progressReader in _progressReaders)
                progressReader.UpdateProgress(_progressData);
        }
    }
}