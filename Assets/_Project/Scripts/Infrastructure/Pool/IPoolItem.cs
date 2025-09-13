using R3;

namespace _Project.Scripts.Infrastructure.Pool
{
    public interface IPoolItem<in TParam>
    {
        Observable<Unit> Deactivated { get; }
        void Activate(TParam param);
    }
}