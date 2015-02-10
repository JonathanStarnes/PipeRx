using System;

namespace PipeRx.Core
{
    /// <summary>
    /// Extension for terminating a pipeline.
    /// </summary>
    public static class SinkExtension
    {
        /// <summary>
        /// Ends the pipeline with a final action. This may be kind of redundant, but it keeps the pipeline theme going...
        /// </summary>
        /// <typeparam name="T">The type of the last segment in the pipeline.</typeparam>
        /// <param name="inlet">The pipeline</param>
        /// <param name="sink">The final action to be applied to this pipeline.</param>
        /// <returns></returns>
        public static IDisposable Sink<T>(this IObservable<T> inlet, Action<T> sink)
        {
            if (inlet == null)
                throw new ArgumentNullException("inlet");

            if (sink == null)
                throw new ArgumentNullException("sink");

            return inlet.Subscribe(sink);
        }
    }
}
