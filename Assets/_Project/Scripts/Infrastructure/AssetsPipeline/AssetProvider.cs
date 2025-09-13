using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Infrastructure.AssetsPipeline
{
    public class AssetProvider : IAssetProvider
    {
        public async UniTask<T> LoadFromResourcesAsync<T>(string path) where T : Object =>
            await Resources.LoadAsync<T>(path).ToUniTask() as T;
    }
}