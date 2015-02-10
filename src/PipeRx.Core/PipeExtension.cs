using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace PipeRx.Core
{
    /// <summary>
    /// Extension for creating a pipeline out of an observable
    /// </summary>
    public static class PipeExtension
    {
        /// <summary>
        /// Adds a segment to the pipeline based on an ISubject. You can use the <see cref="PipelineSegment"/> class for a simple implmentation of 
        /// the ISubject class.
        /// </summary>
        /// <typeparam name="TIn">The type input for this segment.</typeparam>
        /// <typeparam name="TOut">The type output for this segment.</typeparam>
        /// <param name="inlet">The input into this segment.</param>
        /// <param name="segment">The segment to add to this pipeline.</param>
        /// <returns>The pipeline observable with the new segment.</returns>
        public static IObservable<TOut> Pipe<TIn, TOut>(this IObservable<TIn> inlet, ISubject<TIn, TOut> segment)
        {
            if(inlet == null)
                throw new ArgumentNullException("inlet");

            if(segment == null)
                throw new ArgumentNullException("segment");

            inlet.Subscribe(segment);
            return segment;
        }

        /// <summary>
        /// Adds a segment to the pipeline based on an Action. To move items through the pipeline you should call OnNext with the item.
        /// </summary>
        /// <typeparam name="TIn">The type input for this segment.</typeparam>
        /// <typeparam name="TOut">The type output for this segment.</typeparam>
        /// <param name="inlet">The input into this segment.</param>
        /// <param name="segment">The segment to add to this pipeline.</param>
        /// <returns>The pipeline observable with the new segment.</returns>
        public static IObservable<TOut> Pipe<TIn, TOut>(this IObservable<TIn> inlet, Action<IObserver<TOut>, TIn> segment)
        {
            if (inlet == null)
                throw new ArgumentNullException("inlet");

            if (segment == null)
                throw new ArgumentNullException("segment");

            var outlet = new Subject<TOut>();
            inlet.Subscribe((s) => segment(outlet, s));
            return outlet;
        }

    }
}
