using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace PipeRx.Core
{
    public abstract class PipelineSegment<TIn, TOut> : ISubject<TIn, TOut>
    {
        readonly Subject<TOut> _outlet = new Subject<TOut>();

        protected abstract IEnumerable<TOut> ProcessInput(TIn value);

        public void OnNext(TIn value)
        {
            Parallel.ForEach(ProcessInput(value), output => _outlet.OnNext(output));
        }

        public void OnError(Exception error)
        {
            _outlet.OnError(error);
        }

        public void OnCompleted()
        {
            _outlet.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<TOut> observer)
        {
            return _outlet.Subscribe(observer);
        }
    }
}
