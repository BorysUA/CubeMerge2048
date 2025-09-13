using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.CoroutineProvider
{
    public class CoroutineRunner : MonoBehaviour
    {
        public Coroutine ExecuteCoroutine(IEnumerator enumerator) =>
            StartCoroutine(enumerator);

        public void TerminateCoroutine(IEnumerator enumerator) =>
            StopCoroutine(enumerator);

        public void TerminateCoroutine(Coroutine coroutine) =>
            StopCoroutine(coroutine);
    }
}