using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;

namespace UndergroundFortress.UI.MainMenu
{
    public class ListLevelsDungeon : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private int id;
        [SerializeField] private TMP_Text nameDungeon;
        [SerializeField] private List<LevelDungeonButton> listLevelButton;

        [Space]
        [SerializeField] private Animator animatorShow;
        [SerializeField] private Button button;
        [SerializeField] private GameObject lockCap;

        private int KEY_ANIMATOR_SHOW;
        private int KEY_ANIMATOR_HIDE;

        private IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;

        private Action<int> _onSelectDungeon;
        private bool _isSelected;

        public void Construct(IStaticDataService staticDataService, ILocalizationService localizationService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
        }
        
        public void Initialize(IProgressProviderService progressProviderService,
            Action<int> onSelectDungeon,
            Action<int> onStartLevel)
        {
            KEY_ANIMATOR_SHOW = Animator.StringToHash("Show");
            KEY_ANIMATOR_HIDE = Animator.StringToHash("Hide");
            
            _onSelectDungeon = onSelectDungeon;

            nameDungeon.text = _localizationService.LocaleMain(_staticDataService.GetDungeonById(id).name);
            
            button.onClick.AddListener(SelectDungeon);
            button.interactable = false;
            lockCap.SetActive(true);
            
            listLevelButton.ForEach(data => data.Initialize(onStartLevel));
            
            Register(progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            UpdateProgress(progress);
        }

        public void UpdateProgress(ProgressData progress)
        {
            if (!progress.Dungeons.ContainsKey(id))
                return;

            Unlock(progress.Dungeons);
        }

        public void UpdateSelect(int idDungeon)
        {
            if (idDungeon == id)
            {
                if (_isSelected)
                    return;

                _isSelected = true;
                animatorShow.SetTrigger(KEY_ANIMATOR_SHOW);
            }
            else
            {
                if (!_isSelected)
                    return;

                Reset();
            }
        }

        public void Reset()
        {
            _isSelected = false;
            animatorShow.SetTrigger(KEY_ANIMATOR_HIDE);
        }

        private void SelectDungeon()
        {
            _onSelectDungeon(id);
        }

        private void Unlock(Dictionary<int, HashSet<int>> progressDungeons)
        {
            button.interactable = true;
            lockCap.SetActive(false);
            
            foreach (int idLevel in progressDungeons[id]) 
                listLevelButton[idLevel].Unlock();
        }
    }
}