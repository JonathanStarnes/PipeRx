using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PipeRx.Core;
using Xunit;
using Xunit.Should;

namespace PipeRx.Test
{
    public class FilterTest
    {
        [Fact]
        public void null_inlet_should_throw_exception()
        {
            var pipeline = (IObservable<string>)null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                pipeline.Filter(s => true);
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: inlet");
        }

        [Fact]
        public void null_filter_should_throw_exception()
        {
            Predicate<string> filter = null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                Observable.Return("exception").Filter(filter);
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: filter");
        }

        [Fact]
        public void null_filters_should_throw_exception()
        {
            List<Predicate<string>> filters = null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                Observable.Return("exception").Filter(filters);
            });

            exception.Message.ShouldBe("Value cannot be null.\r\nParameter name: filters");
        }


        [Fact]
        public void test_item_should_be_filtered_out()
        {
            var inlet = new ReplaySubject<string>();
            var count = 0;
            inlet
                .Filter(s => s.Contains("filter"))
                .Do(s=> count++)
                .Subscribe(s => s.ShouldNotContain("filter"));

            inlet.OnNext("in output");
            inlet.OnNext("will be filtered out");

            count.ShouldBe(1);
        }

        [Fact]
        public void test_items_should_be_filtered_out()
        {
            var filters = new List<Predicate<string>>
            {
                s=> s.Contains("filter"), // Filter out strings with filter in them
                s=> s.Length < 6 // filter out strings with a length less than 6
            };

            var inlet = new ReplaySubject<string>();
            var count = 0;
            inlet
                .Filter(filters)
                .Do(s => count++)
                .Subscribe(s => s.ShouldBe("in output"));

            inlet.OnNext("in output"); // only this item should be in the output
            inlet.OnNext("will be filtered out"); 
            inlet.OnNext("small");

            count.ShouldBe(1);
        }
    }
}
