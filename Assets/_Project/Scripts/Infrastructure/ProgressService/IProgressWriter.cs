using _Project.Scripts.Data;

namespace _Project.Scripts.Infrastructure.ProgressService
{
    public interface IProgressWriter
    {
        public void WriteProgress(GameStateData gameStateData);
    }
}