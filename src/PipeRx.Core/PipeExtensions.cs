using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace PipeRx.Core
{
    public static class PipeExtensions
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

        /// <summary>
        /// This will add a segment to the pipeline that filters out items based on a predicate.
        /// </summary>
        /// <typeparam name="T">The type of input to the filter.</typeparam>
        /// <param name="inlet">The input into this filter.</param>
        /// <param name="filter">The predicate statement that will determine which items are not included in the output.</param>
        /// <returns>The pipeline observable with the new filter.</returns>
        public static IObservable<T> Filter<T>(this IObservable<T> inlet, Predicate<T> filter)
        {
            if (inlet == null)
                throw new ArgumentNullException("inlet");

            if (filter == null)
                throw new ArgumentNullException("filter");

            var outlet = new Subject<T>();

            inlet.Subscribe((value) =>
            {
                if (!filter(value))
                {
                    outlet.OnNext(value);
                }
            });

            return outlet;
        }

        /// <summary>
        /// This will add a segment to the pipeline that filters out items based on a list of predicates.
        /// </summary>
        /// <typeparam name="T">The type of input to the filter.</typeparam>
        /// <param name="inlet">The input into this filter.</param>
        /// <param name="filters">The list predicate statements that will determine which items are not included in the output.</param>
        /// <returns>The pipeline observable with the new filters.</returns>
        public static IObservable<T> Filter<T>(this IObservable<T> inlet, IEnumerable<Predicate<T>> filters)
        {
            if (inlet == null)
                throw new ArgumentNullException("inlet");

            if (filters == null)
                throw new ArgumentNullException("filters");

            var outlet = new Subject<T>();

            inlet.Subscribe((value) =>
            {
                if (!filters.Any(filter => filter(value)))
                {
                    outlet.OnNext(value);
                }
            });

            return outlet;
        }
    }
}
