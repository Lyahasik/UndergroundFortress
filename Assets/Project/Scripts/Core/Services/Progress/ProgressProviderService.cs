using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Core.Services.GameStateMachine;
using UndergroundFortress.Core.Services.GameStateMachine.States;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Update;
using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Core.Services.Progress
{
    public partial class ProgressProviderService : IProgressProviderService, IUpdating
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameStateMachine _gameStateMachine;

        private ProgressData _progressData;

        private List<IReadingProgress> _progressReaders;
        private List<IWritingProgress> _progressWriters;
        
        private bool _isWasChange;
        private float _waitingSavingTime;

        public ProgressData ProgressData => _progressData;

        public ProgressProviderService(IStaticDataService staticDataService,
            IGameStateMachine gameStateMachine)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public void Initialization(UpdateHandler updateHandler)
        {
            _progressReaders ??= new List<IReadingProgress>();
            _progressWriters ??= new List<IWritingProgress>();
            
            updateHandler.AddUpdatedObject(this);
        }

        public void Update()
        {
            RegularSave();
        }

        public void LoadProgress()
        {
            Debug.Log("Loaded progress.");
            
            _progressData = LoadData(PlayerPrefs.GetString(ConstantValues.KEY_LOCAL_PROGRESS)) ?? CreateNewProgress();
            
            foreach (IReadingProgress progressReader in _progressReaders)
                progressReader.LoadProgress(_progressData);
            
            _waitingSavingTime = ConstantValues.DELAY_SAVING;

            _gameStateMachine.Enter<LoadSceneState>();
        }

        public void SaveProgress()
        {
            if (_progressData == null)
                return;
            
            UpdateProgress();
            
            string json = JsonConvert.SerializeObject(_progressData, new JsonSerializerSettings());
            PlayerPrefs.SetString(ConstantValues.KEY_LOCAL_PROGRESS, json);
            
            _waitingSavingTime = ConstantValues.DELAY_SAVING;
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

        public void Unregister(IReadingProgress progressReader)
        {
            _progressReaders.Remove(progressReader);
        }

        public void Unregister(IWritingProgress progressWriter)
        {
            Unregister(progressWriter as IReadingProgress);

            _progressWriters.Remove(progressWriter);
        }

        public void ResetActiveSkills()
        {
            _progressData.SkillPointsData.Spent = 0;
            
            _progressData.ActiveSkills.Clear();
            foreach (SkillsType type in (SkillsType[])Enum.GetValues(typeof(SkillsType)))
            {
                if (type == SkillsType.Empty)
                    continue;
            
                var set = new HashSet<int>();
                set.Add(0);
                _progressData.ActiveSkills.Add(type, set);
            }
        }

        public void WasChange()
        {
            _isWasChange = true;
        }

        private void RegularSave()
        {
            _waitingSavingTime -= Time.deltaTime;
            
            if (!_isWasChange
                || _waitingSavingTime > 0)
                return;
            
            SaveProgress();
            _isWasChange = false;
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
                LevelData = new PlayerLevelData(),
                SkillPointsData = new SkillPointsData(),
                MainStats = CreateMainStats(),
                ActiveSkills = CreateActiveSkills(),
                ProgressSkills = CreateProgressSkills(),
                
                Wallet = new WalletData(500, 50),
                Equipment = CreateEquipment(),
                ActiveRecipes = CreateActiveRecipes(),
                Bag = CreateBag(),
                FilledNumberBag = 0,
                
                BonusesLifetime = new Dictionary<BonusType, float>(),
                
                Dungeons = new Dictionary<int, HashSet<int>> { { 0, new HashSet<int> { 0 } } }
            };

            SaveProgress();

            return progressData;
        }

        private Dictionary<StatType, float> CreateMainStats()
        {
            var mainStats = new Dictionary<StatType, float>();
            
            foreach (StatType type in (StatType[]) Enum.GetValues(typeof(StatType)))
                mainStats.Add(type, 0f);
            
            return mainStats;
        }

        private Dictionary<SkillsType, HashSet<int>> CreateActiveSkills()
        {
            var activeSkills = new Dictionary<SkillsType, HashSet<int>>();
            
            foreach (SkillsType type in (SkillsType[])Enum.GetValues(typeof(SkillsType)))
            {
                if (type == SkillsType.Empty)
                    continue;
                
                var set = new HashSet<int>();
                set.Add(0);
                activeSkills.Add(type, set);
            }

            return activeSkills;
        }

        private Dictionary<StatType, ProgressSkillData> CreateProgressSkills()
        {
            var progressSkills = new Dictionary<StatType, ProgressSkillData>();
            
            progressSkills.Add(StatType.Dodge, new ProgressSkillData(0, 0));
            progressSkills.Add(StatType.Accuracy, new ProgressSkillData(0, 0));
            progressSkills.Add(StatType.Crit, new ProgressSkillData(0, 0));
            progressSkills.Add(StatType.Parry, new ProgressSkillData(0, 0));
            progressSkills.Add(StatType.Block, new ProgressSkillData(0, 0));
            progressSkills.Add(StatType.BreakThrough, new ProgressSkillData(0, 0));
            progressSkills.Add(StatType.Stun, new ProgressSkillData(0, 0));
            progressSkills.Add(StatType.Strength, new ProgressSkillData(0, 0));
            
            return progressSkills;
        }

        private List<CellData> CreateEquipment()
        {
            return new List<CellData>
            {
                new(null, 1),
                new(null, 1),
                new(null, 1),
                new(null, 1),
                new(null, 1),
                new(null, 1),
                new(null, 1)
            };
        }

        private Dictionary<ItemType, List<int>> CreateActiveRecipes()
        {
            return new Dictionary<ItemType, List<int>>
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
            };
        }

        private List<CellData> CreateBag()
        {
            var bag = new List<CellData>();
            
            for (int i = 0; i < ConstantValues.BASE_SIZE_BAG; i++) 
                bag.Add(new CellData());
            
            return bag;
        }

        private void UpdateProgress()
        {
            foreach (IReadingProgress progressReader in _progressReaders)
                progressReader.UpdateProgress(_progressData);
        }
    }
}