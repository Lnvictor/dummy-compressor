using dummy_compressor.Algorithms;
using dummy_compressor.Utils;

namespace CompressTests
{
    public class LZ77Test
    {
        private Lz77 Compressor;

        [SetUp]
        public void Setup()
        {
            Compressor = new Lz77();
        }

        [Test]
        public void ShouldFindLongestMatch()
        {
            var searchBuffer = new List<char>("abcadsada".ToCharArray());
            var lookaheadBuffer = new List<char>("bcopkpioj".ToCharArray());

            LZ77Metadata output = Compressor.findMatch(searchBuffer, lookaheadBuffer);
            Assert.IsNotNull(output);
            Assert.That(output.Length, Is.EqualTo(2));
            Assert.That(output.Offset, Is.EqualTo(7));
            Assert.That(output.NextChar, Is.EqualTo('o'));
        }

        [Test]
        public void ShouldReturnNoMatch()
        {
            var searchBuffer = new List<char>("abcadsada".ToCharArray());
            var lookaheadBuffer = new List<char>("lkopkpioj".ToCharArray());

            LZ77Metadata output = Compressor.findMatch(searchBuffer, lookaheadBuffer);
            Assert.IsNotNull(output);
            Assert.That(output.Length, Is.EqualTo(0));
            Assert.That(output.Offset, Is.EqualTo(0));
            Assert.That(output.NextChar, Is.EqualTo('l'));
        }
    }
}