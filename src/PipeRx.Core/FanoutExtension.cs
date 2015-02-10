using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace PipeRx.Core
{
    /// <summary>
    /// Extension for fanning out a pipeline into multiple branches.
    /// </summary>
    public static class FanoutExtension
    {
        /// <summary>
        /// This will take a single pipeline and branch it out into several pipelines
        /// </summary>
        /// <typeparam name="TIn">the input type of the pipeline.</typeparam>
        /// <typeparam name="TOut">the output type of each segment.</typeparam>
        /// <param name="inlet">The incoming pipeline</param>
        /// <param name="segments">The list of outgoing segments.</param>
        /// <returns>an enumerable of all of the new segments.</returns>
        public static IEnumerable<IObservable<TOut>> Fanout<TIn, TOut>(this IObservable<TIn> inlet, params ISubject<TIn, TOut>[] segments)
        {
            if (inlet == null)
                throw new ArgumentNullException("inlet");

            if (segments == null)
                throw new ArgumentNullException("segments");

            if (segments.Length == 0)
                throw new ArgumentException("You must supply at least one segment to fanout.", "segments");

            foreach (var segment in segments)
            {
                inlet.Subscribe(segment);
            }

            return segments;
        }
    }
}
