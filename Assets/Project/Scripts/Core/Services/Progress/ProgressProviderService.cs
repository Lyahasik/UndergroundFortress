using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.Core.Services.Progress
{
    public partial class ProgressProviderService : IProgressProviderService
    {
        private const string KEY_LOCAL_PROGRESS = "local_progress";
        
        private readonly IStaticDataService _staticDataService;
        private readonly IGameStateMachine _gameStateMachine;

        private CharacterStats _playerStats;
        private ProgressData _progressData;

        private List<IReadingProgress> _progressReaders;
        private List<IWritingProgress> _progressWriters;

        public CharacterStats PlayerStats => _playerStats;
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
            _playerStats = LoadingBaseStats();
            
            _progressData = LoadData(PlayerPrefs.GetString(KEY_LOCAL_PROGRESS)) ?? CreateNewProgress();

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
                    { ItemType.Sword, new List<int> { 7, 9, 10 } },
                    { ItemType.TwoHandedWeapon, new List<int> { 8 } },
                    { ItemType.Dagger, new List<int> { 2 } },
                    { ItemType.Mace, new List<int> { 4 } },
                    { ItemType.Shield, new List<int> { 6 } },
                    { ItemType.Chest, new List<int> { 1, 11 } },
                    { ItemType.Pants, new List<int> { 5 } },
                    { ItemType.Boots, new List<int> { 0 } },
                    { ItemType.Gloves, new List<int> { 3 } },
                    { ItemType.Consumable, new List<int> { 1003, 1004 } }
                },
                Bag = new List<CellData>()
            };
            
            for (int i = 0; i < ConstantValues.BASE_SIZE_BAG; i++) 
                progressData.Bag.Add(new CellData());
            
            SaveProgress();

            return progressData;
        }

        public void SaveProgress()
        {
            if (_progressData == null)
                return;
            
            ReadProgress();
            
            string json = JsonConvert.SerializeObject(_progressData, new JsonSerializerSettings());
            PlayerPrefs.SetString(KEY_LOCAL_PROGRESS, json);
        }

        public void Register(IReadingProgress progressReader)
        {
            _progressReaders.Add(progressReader);
            
            if (_progressData != null)
                progressReader.ReadProgress(ProgressData);
        }

        public void Register(IWritingProgress progressWriter)
        {
            Register(progressWriter as IReadingProgress);

            _progressWriters.Add(progressWriter);
        }

        public void SetWalletValues(int money1, int money2)
        {
            _progressData.Wallet.Money1 = money1;
            _progressData.Wallet.Money2 = money2;
            
            SaveProgress();
        }

        private CharacterStats LoadingBaseStats()
        {
            CharacterStaticData characterStaticData = _staticDataService.ForPlayer();
            CharacterStats characterStats = new CharacterStats();
            characterStats.Initialize(characterStaticData);
            
            return characterStats;
        }

        private void ReadProgress()
        {
            foreach (IReadingProgress progressReader in _progressReaders)
                progressReader.ReadProgress(_progressData);
        }
    }
}