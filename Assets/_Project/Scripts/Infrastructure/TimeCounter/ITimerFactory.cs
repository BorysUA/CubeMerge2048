namespace _Project.Scripts.Infrastructure.TimeCounter
{
    public interface ITimerFactory
    {
        public ITimer Create(bool autoStart = true);
        public void DisposeTimer(ITimer timer);
    }
}