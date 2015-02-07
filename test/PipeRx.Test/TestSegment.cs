using System.Collections.Generic;
using PipeRx.Core;

namespace PipeRx.Test
{
    public class TestSegment : PipelineSegment<string, string>
    {
        protected override IEnumerable<string> ProcessInput(string value)
        {
            yield return value;
        }
    }
}