using StringProcessingWebAPI.Handlers;

namespace TestStringProcessing
{
    [TestFixture]
    public class StringHandlerTest
    {
        private StringProcessHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new StringProcessHandler();
        }

        [Test]
        public void ProcessString_EvenLengthString_ReturnsReversedHalves()
        {
            var input = "abcdef";
            var result = _handler.ProcessString(input);
            Assert.That(result, Is.EqualTo("cbafed"));
        }

        [Test]
        public void ProcessString_OddLengthString_ReturnsReversedAndAppended()
        {
            var input = "abc";
            var result = _handler.ProcessString(input);
            Assert.That(result, Is.EqualTo("cbaabc"));
        }

        [Test]
        public void CheckIfValidString_ValidString_ReturnsTrue()
        {
            var input = "abcdef";
            var result = _handler.CheckIfValidString(input);
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckIfValidString_InvalidString_ReturnsFalse()
        {
            var input = "ABC123";
            var result = _handler.CheckIfValidString(input);
            Assert.IsFalse(result);
        }

        [Test]
        public void CountCharacterOccurrences_ValidString_ReturnsCorrectCount()
        {
            var input = "aabbcc";
            var result = _handler.CountCharacterOccurrences(input);
            Assert.That(result['a'], Is.EqualTo(2));
            Assert.That(result['b'], Is.EqualTo(2));
            Assert.That(result['c'], Is.EqualTo(2));
        }

        [Test]
        public void FindLongestVowelSubstring_ValidString_ReturnsCorrectSubstring()
        {
            var input = "hhhhfaeiouyhhhhhh";
            var result = _handler.FindLongestVowelSubstring(input);
            Assert.That(result, Is.EqualTo("aeiouy"));
        }
    }
}