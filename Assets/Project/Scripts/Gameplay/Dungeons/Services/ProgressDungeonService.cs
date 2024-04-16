using System;
using System.Linq;
using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Services.Factories.Gameplay;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Player.Level.Services;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats.Services;
using UndergroundFortress.UI.Hud;
using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Dungeons.Services
{
    public class ProgressDungeonService : IProgressDungeonService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IAttackService _attackService;
        private readonly IStatsRestorationService _statsRestorationService;
        private readonly ICheckerCurrentStatsService _checkerCurrentStatsService;
        private readonly IPlayerUpdateLevelService _playerUpdateLevelService;

        private Canvas _gameplayCanvas;
        private AttackArea _attackArea;
        private PlayerData _playerData;

        private TMP_Text _nameLevelText;
        private int _currentLevelId;
        private DungeonStaticData _currentDungeon;

        private int _currentStage;
        private EnemyStaticData _currentEnemyStaticData;
        private EnemyData _currentEnemy;

        public event Action OnSuccessLevel;
        public event Action<int> OnUpdateSteps;

        public ProgressDungeonService(IStaticDataService staticDataService,
            IGameplayFactory gameplayFactory,
            IAttackService attackService,
            IStatsRestorationService statsRestorationService,
            ICheckerCurrentStatsService checkerCurrentStatsService,
            IPlayerUpdateLevelService playerUpdateLevelService)
        {
            _staticDataService = staticDataService;
            _gameplayFactory = gameplayFactory;
            _attackService = attackService;
            _statsRestorationService = statsRestorationService;
            _checkerCurrentStatsService = checkerCurrentStatsService;
            _playerUpdateLevelService = playerUpdateLevelService;
        }

        public void Initialize(Canvas gameplayCanvas,
            HudView hudView,
            PlayerData playerData,
            int dungeonId,
            int levelId)
        {
            _gameplayCanvas = gameplayCanvas;
            _playerData = playerData;
            _nameLevelText = hudView.NameLevelText;
            UpdateLevel(dungeonId, levelId);

            _attackArea = _gameplayFactory.CreateAttackArea(gameplayCanvas.transform);
            _attackArea.Construct(
                _playerData,
                _checkerCurrentStatsService,
                _attackService);

            hudView.LevelDungeonProgressBar.Subscribe(this);
        }

        private void UpdateLevel(int dungeonId, int levelId)
        {
            _currentDungeon = _staticDataService.GetDungeonById(dungeonId);
            _currentLevelId = levelId;

            _nameLevelText.text = $"{_currentDungeon.name} { _currentLevelId + 1 }";
        }

        public void StartBattle()
        {
            _currentStage = 0;
            OnUpdateSteps?.Invoke(_currentStage);
            
            UpdateEnemyData();
            CreateEnemy();
        }

        public void NextLevel()
        {
            _currentLevelId++;

            if (_currentLevelId == _currentDungeon.levels.Count) 
                UpdateLevel(_currentDungeon.id + 1, 0);
            else
                UpdateLevel(_currentDungeon.id, _currentLevelId);
            
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

        private void DeadEnemy()
        {
            _playerUpdateLevelService.IncreaseExperience(_currentEnemyStaticData.experience);
            
            _currentEnemy = null;
            OnUpdateSteps?.Invoke(_currentStage);

            if (_currentStage < _currentDungeon.levels[_currentLevelId].numberStages)
            {
                UpdateEnemyData();
                CreateEnemy();
            }
            else
            {
                OnSuccessLevel?.Invoke();
            }
        }
    }
}