using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Dungeons.Services;

namespace UndergroundFortress
{
    public class LevelDungeonProgressBar : MonoBehaviour
    {
        [SerializeField] private List<LevelDungeonProgressStepView> progressSteps;

        public void Subscribe(ProgressDungeonService progressDungeonService)
        {
            progressDungeonService.OnUpdateSteps += UpdateSteps;
        }

        private void UpdateSteps(int numberStep)
        {
            for (int i = 0; i < progressSteps.Count; i++) 
                progressSteps[i].SetSuccess(i < numberStep);
        }
    }
}
