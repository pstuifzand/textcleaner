using NUnit.Framework;
using TextCleaner;

namespace TextCleanerTest
{
    public class AddSuffix
    {
        private Operation operation;

        [SetUp]
        public void Setup()
        {
            operation = new TextCleaner.AddSuffix();
        }

        [Test]
        public void TestInfo()
        {
            Assert.AreEqual("Append Text", operation.Name);
            Assert.AreEqual(new []{"Suffix"}, operation.ArgNames);
        }

        [Test]
        public void AddSuffixToEmptyString()
        {
            var result = operation.Process("", new[] {"", ""});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("", result.Text);
        }

        [Test]
        public void AddSuffixToString()
        {
            var result = operation.Process("hello", new[] {" world", ""});
            Assert.IsTrue(result.Keep);
            Assert.AreEqual("hello world", result.Text);
        }
    }
}
