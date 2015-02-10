using System;
using System.Reactive.Linq;
using PipeRx.Core;
using Xunit;
using Xunit.Should;

namespace PipeRx.Test
{
    public class SinkTest
    {
        [Fact]
        public void null_inlet_should_throw_exception()
        {
            var pipeline = (IObservable<string>)null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                pipeline.Sink(s => { });
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: inlet");
        }

        [Fact]
        public void null_sink_should_thow_exception()
        {
            Action<string> sink = null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                Observable.Return("exception").Sink(sink);
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: sink");
        }

        [Fact]
        public void items_should_flow_into_sink()
        {
            Observable.Return("Item").Sink(s => s.ShouldBe("Item"));
        }
    }
}
