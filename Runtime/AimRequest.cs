using System;

namespace Flscap.AimSystem
{
    public abstract class AimRequest
    {
        internal abstract void Commit(object rawData);
    }

    public class AimRequest<TContext, TAimData> : AimRequest 
        where TContext : class
        where TAimData : class
    {
        public readonly TContext _context;
        private readonly Action<TContext, TAimData> _onCommitted;

        public AimRequest(TContext context, Action<TContext, TAimData> onCommitted)
        {
            _context = context;
            _onCommitted = onCommitted;
        }

        internal override void Commit(object rawData)
        {
            if (rawData is not TAimData result)
                throw new InvalidOperationException(
                    $"AimMode returned {rawData?.GetType().Name}, " +
                    $"but AimRequest expects {typeof(TAimData).Name}"
                );

            _onCommitted(_context, result);
        }
    }
}