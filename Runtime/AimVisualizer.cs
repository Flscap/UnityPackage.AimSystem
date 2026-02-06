using UnityEngine;

namespace Flscap.AimSystem
{
    public abstract class AimVisualizer : MonoBehaviour
    {
        internal abstract void InitializeInternal(IAimMode aimMode);

        public virtual void Cleanup()
        {
            Destroy(gameObject);
        }
    }

    public abstract class AimVisualizer<TAimMode> : AimVisualizer
        where TAimMode : IAimMode
    {
        protected TAimMode AimMode;

        internal override void InitializeInternal(IAimMode aimMode)
        {
            AimMode = (TAimMode)aimMode;
            OnInitialized();
        }

        protected virtual void OnInitialized() { }
    }
}