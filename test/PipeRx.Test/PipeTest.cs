using System;
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
