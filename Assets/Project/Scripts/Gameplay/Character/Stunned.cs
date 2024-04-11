using System;
using UnityEngine;

namespace UndergroundFortress.Gameplay.Character
{
    public class Stunned : MonoBehaviour
    {
        private float _lifetime;

        private Action _onRemoveHitEffect;

        public void Initialize(Action onRemoveHitEffect)
        {
            _onRemoveHitEffect = onRemoveHitEffect;
        }

        private void Update()
        {
            TryFinish();
        }

        public void Activate(float duration)
        {
            _lifetime = duration;
            enabled = true;
        }

        private void TryFinish()
        {
            if (IsAlive())
                return;

            _lifetime -= Time.deltaTime;

            if (IsAlive())
            {
                enabled = false;
                _onRemoveHitEffect?.Invoke();
            }
        }

        private bool IsAlive() => 
            _lifetime <= 0f;
    }
}