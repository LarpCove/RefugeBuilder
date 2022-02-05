using System;
using System.IO;
using CreateAndFake.Fluent;
using RefugeBuilder.Terminal;
using Xunit;

namespace RefugeBuilder.TerminalTests
{
    public static class ProgramTests
    {
        [Fact]
        internal static void Main_Runs()
        {
            using StringReader input = new(string.Join(Environment.NewLine,
                "test",
                null,
                "quit"));
            Console.SetIn(input);

            using StringWriter output = new();
            Console.SetOut(output);
            Program.Main();

            string result = output.ToString();
            result.Assert().Contains("Hello");
            result.Assert().Contains("test");
        }
    }
}
