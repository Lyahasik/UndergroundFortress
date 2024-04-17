using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Player.Level.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.UI.Hud;
using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public class ProgressDungeonService : IProgressDungeonService, IWritingProgress
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IAttackService _attackService;
        private readonly IStatsRestorationService _statsRestorationService;
        private readonly ICheckerCurrentStatsService _checkerCurrentStatsService;
        private readonly IPlayerUpdateLevelService _playerUpdateLevelService;
        private readonly IItemsGeneratorService _itemsGeneratorService;
        private readonly IWalletOperationService _walletOperationService;

        private DungeonBackground _dungeonBackground;
        private Canvas _gameplayCanvas;
        private AttackArea _attackArea;
        private PlayerData _playerData;

        private TMP_Text _nameLevelText;
        private int _currentLevelId;
        private DungeonStaticData _currentDungeon;

        private int _currentStage;
        private EnemyStaticData _currentEnemyStaticData;
        private EnemyData _currentEnemy;

        private bool _isPause;

        public bool IsPause => _isPause;
        
        public event Action<bool, bool> OnEndLevel;
        public event Action<int> OnUpdateSteps;

        public ProgressDungeonService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IGameplayFactory gameplayFactory,
            IAttackService attackService,
            IStatsRestorationService statsRestorationService,
            ICheckerCurrentStatsService checkerCurrentStatsService,
            IPlayerUpdateLevelService playerUpdateLevelService,
            IItemsGeneratorService itemsGeneratorService,
            IWalletOperationService walletOperationService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _gameplayFactory = gameplayFactory;
            _attackService = attackService;
            _statsRestorationService = statsRestorationService;
            _checkerCurrentStatsService = checkerCurrentStatsService;
            _playerUpdateLevelService = playerUpdateLevelService;
            _itemsGeneratorService = itemsGeneratorService;
            _walletOperationService = walletOperationService;
        }

        public void Initialize(Canvas gameplayCanvas,
            DungeonBackground dungeonBackground,
            HudView hudView,
            PlayerData playerData,
            int dungeonId,
            int levelId)
        {
            _statsRestorationService.ProgressDungeonService = this;
            
            _gameplayCanvas = gameplayCanvas;
            _dungeonBackground = dungeonBackground;
            
            _playerData = playerData;
            _nameLevelText = hudView.NameLevelText;
            UpdateLevel(dungeonId, levelId);
            _dungeonBackground.UpdateBackground(_currentDungeon.background);

            _playerData.OnDead += DeadPlayer;
            
            _attackArea = _gameplayFactory.CreateAttackArea(gameplayCanvas.transform);
            _attackArea.Construct(
                _playerData,
                _checkerCurrentStatsService,
                _attackService);

            hudView.LevelDungeonProgressBar.Subscribe(this);
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

        private void UpdateLevel(int dungeonId, int levelId)
        {
            _currentDungeon = _staticDataService.GetDungeonById(dungeonId);
            _currentLevelId = levelId;

            _nameLevelText.text = $"{_currentDungeon.name} { _currentLevelId + 1 }";
        }

        public void StartBattle()
        {
            _isPause = false;
            _currentStage = 0;
            OnUpdateSteps?.Invoke(_currentStage);
            
            UpdateEnemyData();
            CreateEnemy();
        }

        public void NextLevel()
        {
            _currentLevelId++;

            if (_currentLevelId == _currentDungeon.levels.Count)
            {
                UpdateLevel(_currentDungeon.id + 1, 0);
                _dungeonBackground.UpdateBackground(_currentDungeon.background);
            }
            else
            {
                UpdateLevel(_currentDungeon.id, _currentLevelId);
            }
            
            StartBattle();
        }

        private void CreateEnemy()
        {
            _currentStage++;
            
            var enemyStats = new CharacterStats();
            enemyStats.Initialize(_currentEnemyStaticData);
            
            _currentEnemy = _gameplayFactory.CreateEnemy(_currentEnemyStaticData.enemyData, _gameplayCanvas.transform);
            _currentEnemy.Construct(
                enemyStats,
                _staticDataService,
                _walletOperationService,
                _itemsGeneratorService,
                _statsRestorationService,
                _attackService,
                _checkerCurrentStatsService,
                _playerData,
                StartDeadEnemy,
                DeadEnemy,
                _attackArea.Activate);
            _currentEnemy.Initialize();
            _currentEnemy.transform.SetSiblingIndex(0);
        }

        private void UpdateEnemyData()
        {
            var enemies = _currentDungeon.levels[_currentLevelId].enemies;
            int totalWeight = enemies.Sum(data => data.probabilityWeight);
            int accident = Random.Range(0, totalWeight + 1);

            foreach (SpawnEnemyStaticData spawnEnemyStaticData in enemies)
            {
                accident -= spawnEnemyStaticData.probabilityWeight;
                if (accident < 0)
                {
                    _currentEnemyStaticData = spawnEnemyStaticData.enemyStaticData;
                    return;
                }
            }
            
            Debug.LogWarning($"[ProgressDungeonService] Not found enemy by probability weight.");
            _currentEnemyStaticData = enemies[0].enemyStaticData;
        }

        private void StartDeadEnemy()
        {
            _attackArea.Deactivate();
        }

        private void DeadPlayer()
        {
            Reset();
            OnEndLevel?.Invoke(false, IsLastDungeon());
        }

        private void DeadEnemy()
        {
            if (!_isPause)
                CreateLoot();
            
            _currentEnemy = null;
            
            if (_isPause)
                return;
                
            _playerUpdateLevelService.IncreaseExperience(_currentEnemyStaticData.experience);
            
            OnUpdateSteps?.Invoke(_currentStage);

            if (_currentStage < _currentDungeon.levels[_currentLevelId].numberStages)
            {
                UpdateEnemyData();
                CreateEnemy();
            }
            else
            {
                SuccessDungeonLevel(_currentDungeon.id, _currentLevelId);
                OnEndLevel?.Invoke(true, IsLastDungeon());
            }
        }

        //TODO take it to another place
        private void CreateLoot()
        {
            int priceTimeEnemy = _currentEnemyStaticData.priceTime;
            
            var lootItems = _currentEnemyStaticData.lootItems;
            int totalWeight = lootItems.Sum(data => data.probabilityWeight);
            int accident = Random.Range(0, totalWeight + 1);

            while (priceTimeEnemy > 0)
            {
                int priceTimeItem = int.MaxValue;
                
                foreach (ItemStaticData item in lootItems)
                {
                    accident -= item.probabilityWeight;
                    if (accident < 0)
                    {
                        _currentEnemy.DroppingItem(item);
                        
                        priceTimeItem = item.priceTime;
                        break;
                    }
                }

                priceTimeEnemy -= priceTimeItem;
            }
        }

        private bool IsLastDungeon()
        {
            var dungeons = _staticDataService.ForDungeons();

            return _currentDungeon.id + 1 == dungeons.Count && _currentLevelId + 1 == _currentDungeon.levels.Count;
        }

        private void SuccessDungeonLevel(int idDungeon, int idLevel)
        {
            var dungeons = _progressProviderService.ProgressData.Dungeons;

            if (idLevel == ConstantValues.MAX_DUNGEON_LEVEL_ID)
                dungeons[idDungeon + 1] = new HashSet<int> { 0 };
            else
                dungeons[idDungeon].Add(idLevel + 1);
            
            WriteProgress();
        }

        private void Reset()
        {
            _isPause = true;
            
            _currentEnemy.Dead();

            _currentStage = 0;
            OnUpdateSteps?.Invoke(_currentStage);
        }
    }
}