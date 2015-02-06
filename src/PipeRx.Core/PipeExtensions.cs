﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace PipeRx.Core
{
    public static class PipeExtensions
    {
        public static IObservable<TOut> Pipe<TIn, TOut>(this IObservable<TIn> inlet, ISubject<TIn, TOut> segment)
        {
            inlet.Subscribe(segment);
            return segment;
        }

        public static IObservable<TOut> Pipe<TIn, TOut>(this IObservable<TIn> inlet, Action<IObserver<TOut>, TIn> segment)
        {
            var outlet = new Subject<TOut>();
            inlet.Subscribe((s) => segment(outlet, s));
            return outlet;
        }

        public static IObservable<T> Filter<T>(this IObservable<T> inlet, Predicate<T> filter)
        {
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

        public static IObservable<T> Filter<T>(this IObservable<T> inlet, IEnumerable<Predicate<T>> filters)
        {
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
