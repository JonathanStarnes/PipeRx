using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PipeRx.Core;
using Xunit;
using Xunit.Should;

namespace PipeRx.Test
{
    public class PipeTest
    {
        private const string ExpectedValue = "test";

        [Fact]
        public void null_pipe_inlet_should_throw_exception()
        {
            var pipeline = (IObservable<string>)null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                pipeline.Pipe<string, string>((observer, s) => observer.OnNext("Exception"));
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: inlet");
        }

        [Fact]
        public void null_action_should_throw_exception()
        {
            Action<IObserver<string>, string> action = null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                Observable.Return("exception").Pipe(action);
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: segment");
        }

        [Fact]
        public void null_segment_should_throw_exception()
        {
            TestSegment segment = null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                Observable.Return("exception").Pipe(segment);
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: segment");
        }

        [Fact]
        public void input_of_pipe_should_propagate_through_the_action_to_the_output()
        {
            Action<IObserver<string>, string> testSegment = (observer, value) => observer.OnNext(value);

            var inlet = new ReplaySubject<string>();

            inlet
                .Pipe(testSegment)
                .Subscribe(s => s.ShouldBe(ExpectedValue));

            inlet.OnNext(ExpectedValue);
            inlet.OnCompleted();
        }

        [Fact]
        public void input_of_pipe_should_propagate_through_the_segment_to_the_output()
        {
            var output = string.Empty;

            var inlet = new ReplaySubject<string>();

            inlet
                .Pipe(new TestSegment())
                .Subscribe(s =>
                {
                    output = s;
                });

            inlet.OnNext(ExpectedValue);
            inlet.OnCompleted();

            output.ShouldBe(ExpectedValue);
        }

    }
}
