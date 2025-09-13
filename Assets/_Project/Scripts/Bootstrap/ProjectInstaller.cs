using _Project.Scripts.Infrastructure.CoroutineProvider;
using _Project.Scripts.Infrastructure.LogService;
using _Project.Scripts.Infrastructure.SceneLoader;
using _Project.Scripts.Infrastructure.TimeCounter;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindCoreMonoComponents();
            BindInputActions();
            BindServices();
            BindEntryPoint();
        }

        private void BindCoreMonoComponents()
        {
            Container.BindInterfacesTo<CoroutineProvider>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallCoroutine)
                .WithGameObjectName("Coroutine runner")
                .UnderTransform(transform)
                .AsSingle();
        }

        private void InstallCoroutine(DiContainer subContainer)
        {
            subContainer.Bind<CoroutineProvider>().AsSingle();

            subContainer.Bind<CoroutineRunner>()
                .FromNewComponentOnRoot()
                .AsSingle();
        }

        private void BindInputActions()
        {
            Container.Bind<InputSystemActions>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<LogService>().AsSingle();
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<TimerFactory>().AsSingle();
        }

        private void BindEntryPoint() =>
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();
    }
}