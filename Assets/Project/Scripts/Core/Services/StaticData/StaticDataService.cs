using UnityEngine;

using UndergroundFortress.Scripts.Constants;
using UndergroundFortress.Scripts.Gameplay.StaticData;
using UndergroundFortress.Scripts.MainMenu.StaticData;
using UndergroundFortress.Scripts.UI.StaticData;

namespace UndergroundFortress.Scripts.Core.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private UIStaticData _ui; 
            
        private MainMenuStaticData _mainMenu;
        private PlayerStaticData _player;

        private LevelStaticData _level;
        private EnemyStaticData _enemy;

        public void Load()
        {
            _ui = Resources
                .Load<UIStaticData>(ConstantPaths.UI_DATA_PATH);
            
            _mainMenu = Resources
                .Load<MainMenuStaticData>(ConstantPaths.MAIN_MENU_DATA_PATH);
            _player = Resources
                .Load<PlayerStaticData>(ConstantPaths.PLAYER_DATA_PATH);
            
            _level = Resources
                .Load<LevelStaticData>(ConstantPaths.LEVEL_DATA_PATH);
            _enemy = Resources
                .Load<EnemyStaticData>(ConstantPaths.ENEMY_DATA_PATH);
        }

        public UIStaticData ForUI() => 
            _ui;

        public MainMenuStaticData ForMainMenu() => 
            _mainMenu;
        public PlayerStaticData ForPlayer() => 
            _player;

        public LevelStaticData ForLevel() => 
            _level;
        public EnemyStaticData ForEnemy() => 
            _enemy;
    }
}