using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.CoroutineProvider
{
    public interface ICoroutineProvider
    {
        Coroutine ExecuteCoroutine(IEnumerator enumerator);
        void TerminateCoroutine(IEnumerator enumerator);
        void TerminateCoroutine(Coroutine coroutine);
    }
}