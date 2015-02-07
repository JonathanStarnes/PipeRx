using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace PipeRx.Core
{
    /// <summary>
    /// A segment in an observable pipeline. When implimenting this class you should override the ProcessInput method. 
    /// </summary>
    /// <typeparam name="TIn">The type of input into this segment.</typeparam>
    /// <typeparam name="TOut">The type of output out of this segment.</typeparam>
    public abstract class PipelineSegment<TIn, TOut> : ISubject<TIn, TOut>
    {
        readonly Subject<TOut> _outlet = new Subject<TOut>();

        /// <summary>
        /// Override this method to process the input and determine what the output of this segment should be.
        /// </summary>
        /// <param name="value">The input to this stage from <see cref="OnNext"/></param>
        /// <returns></returns>
        protected abstract IEnumerable<TOut> ProcessInput(TIn value);

        /// <summary>
        /// Called when a new item has entered this segment of the pipeline.
        /// </summary>
        /// <param name="value">The current input.</param>
        public void OnNext(TIn value)
        {
            Parallel.ForEach(ProcessInput(value), output => _outlet.OnNext(output));
        }

        /// <summary>
        /// Called when an error occurs earlier in the pipeline.
        /// </summary>
        /// <param name="error">The error.</param>
        public void OnError(Exception error)
        {
            _outlet.OnError(error);
        }

        /// <summary>
        /// Called when the previous stages are done producing input.
        /// </summary>
        public void OnCompleted()
        {
            _outlet.OnCompleted();
        }

        /// <summary>
        /// Called to subscribe to this pipeline.
        /// </summary>
        /// <param name="observer">The observer of this pipeline.</param>
        /// <returns>A disposable for this object.</returns>
        public IDisposable Subscribe(IObserver<TOut> observer)
        {
            return _outlet.Subscribe(observer);
        }
    }
}
