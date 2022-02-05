using CreateAndFake.Fluent;
using RefugeBuilder.PlayerBuilder;
using Xunit;

namespace RefugeBuilder.PlayerBuilderTests
{
    public static class DummyPlayerBuilderTests
    {
        [Fact]
        internal static void Exists_IsTrue()
        {
            DummyPlayerBuilder.Exists.Assert().Is(true);
        }
    }
}
