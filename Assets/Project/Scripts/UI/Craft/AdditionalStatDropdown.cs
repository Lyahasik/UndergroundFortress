using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.UI.Craft
{
    public class AdditionalStatDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        private List<TMP_Dropdown.OptionData> _options;
        private List<StatType> _statTypes;

        private StatType _currentStatType;

        public StatType CurrentStatType => _currentStatType;
        
        public void Initialize(IStaticDataService staticDataService)
        {
            _options = new List<TMP_Dropdown.OptionData>();
            _statTypes = new List<StatType>();
            List<StatStaticData> stats = staticDataService.ForStats();
            
            AddStat(StatType.Empty);
            foreach (StatStaticData statStaticData in stats)
                AddStat(statStaticData.type);

            dropdown.options = _options;

            dropdown.onValueChanged.AddListener(LocaleSelected);
        }

        private void AddStat(StatType type)
        {
            _options.Add(new TMP_Dropdown.OptionData(type.ToString()));
            _statTypes.Add(type);
        }

        public void LocaleSelected(int id)
        {
            _currentStatType = _statTypes[id];
        }
    }
}