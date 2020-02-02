using NUnit.Framework;
using TextCleaner;

namespace TextCleanerTest
{
    public class AddPrefix
    {
        private Operation operation;

        [SetUp]
        public void Setup()
        {
            operation = new TextCleaner.AddPrefix();
        }

        [Test]
        public void TestInfo()
        {
            Assert.AreEqual("Prepend Text", operation.Name);
            Assert.AreEqual(new []{"Prefix"}, operation.ArgNames);
        }

        [Test]
        public void AddPrefixToEmptyString()
        {
            var result = operation.Process("", new[] {"", ""});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("", result.Text);
        }

        [Test]
        public void AddPrefixToString()
        {
            var result = operation.Process("world", new[] {"hello ", ""});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("hello world", result.Text);
        }
    }
}
