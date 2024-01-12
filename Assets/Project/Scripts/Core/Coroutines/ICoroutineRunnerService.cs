using System.Collections;
using UnityEngine;

using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Core.Coroutines
{
    public interface ICoroutineRunnerService : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}