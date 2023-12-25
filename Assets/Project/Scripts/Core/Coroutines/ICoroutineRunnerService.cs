using System.Collections;
using UnityEngine;

using UndergroundFortress.Scripts.Core.Services;

namespace UndergroundFortress.Scripts.Core.Coroutines
{
    public interface ICoroutineRunnerService : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}