using CreateAndFake.Fluent;
using RefugeBuilder.Rules;
using Xunit;

namespace RefugeBuilder.RulesTests
{
    public static class DummyRulesTests
    {
        [Fact]
        internal static void Exists_IsTrue()
        {
            DummyRules.Exists.Assert().Is(true);
        }
    }
}
