using System.Collections.Generic;
using System.Linq;
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

        private List<ItemStaticData> _items;

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
            
            _items = Resources
                .LoadAll<ItemStaticData>(ConstantPaths.ITEMS_DATA_PATH)
                .ToList();
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
        
        public List<ItemStaticData> ForItems() => 
            _items;
    }
}