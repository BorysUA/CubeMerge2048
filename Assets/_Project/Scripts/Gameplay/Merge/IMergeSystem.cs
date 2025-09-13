using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Merge
{
    public interface IMergeSystem
    {
        public void TryMerge(int fromId, int toId, Collision collision);
        public Observable<int> Merged { get; }
    }
}