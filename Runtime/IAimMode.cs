namespace Flscap.AimSystem
{
    public interface IAimMode
    {
        void UpdateState(float deltaTime);
        bool TryProjectIntent(out object aimData);
    }

    public interface IAimMode<TAimData> : IAimMode
    {
        TAimData Data { get; }
    }
}
