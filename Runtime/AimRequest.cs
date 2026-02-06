using System;

namespace Flscap.AimSystem
{
    public abstract class AimRequest
    {
        internal abstract void Commit(object rawData);
    }

    public class AimRequest<TContext, TAimResult> : AimRequest 
        where TContext : class
        where TAimResult : class
    {
        public readonly TContext _context;
        private readonly Action<TContext, TAimResult> _onCommitted;

        public AimRequest(TContext context, Action<TContext, TAimResult> onCommitted)
        {
            _context = context;
            _onCommitted = onCommitted;
        }

        internal override void Commit(object rawData)
        {
            _onCommitted(_context, (TAimResult)rawData);
        }


    }
}


