namespace _Project.Scripts.Infrastructure.Pool
{
    public interface IResettablePoolItem<in TParam> : IPoolItem<TParam>
    {
        void Reset();
    }
}