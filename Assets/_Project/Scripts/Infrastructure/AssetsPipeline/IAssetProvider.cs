using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.AssetsPipeline
{
    public interface IAssetProvider
    {
        public UniTask<T> LoadFromResourcesAsync<T>(string path) where T : Object;
    }
}