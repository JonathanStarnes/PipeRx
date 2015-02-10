using PipeRx.Core;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xunit;
using Xunit.Should;

namespace PipeRx.Test
{
    public class FanoutTest
    {
        [Fact]
        public void input_should_be_recieved_by_each_segment_in_the_fanout()
        {
            var source = Observable.Return("Test");
            var pipea = new TestSegment();
            var pipeb = new TestSegment();

            source.Fanout(pipea, pipeb);

            pipea.Sink(s => s.ShouldBe("Test"));
            pipeb.Sink(s => s.ShouldBe("Test"));
        }

        [Fact]
        public void null_inlet_should_throw_an_exception()
        {
            var source = (IObservable<string>)null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                source.Fanout(new List<ISubject<string, string>>().ToArray());
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: inlet");
        }

        [Fact]
        public void null_segments_should_throw_an_exception()
        {
            var source = Observable.Return("Test");
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                source.Fanout<string, string>(null);
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: segments");
        }

        [Fact]
        public void zero_segments_should_throw_an_exception()
        {
            var source = Observable.Return("Test");
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                source.Fanout(new List<ISubject<string, string>>().ToArray());
            });

            exception.Message.ShouldBe("You must supply at least one segment to fanout.\r\nParameter name: segments");
        }
    }
}
