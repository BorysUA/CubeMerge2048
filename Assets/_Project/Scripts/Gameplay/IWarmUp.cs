using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Gameplay.Factory
{
    public interface IWarmUp
    {
        public UniTask WarmUpAsync();
    }
}