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
            var lookaheadBuffer = new List<char>("bcadsaioj".ToCharArray());

            LZ77Metadata output = Compressor.FindMatch(searchBuffer, lookaheadBuffer);
            Assert.IsNotNull(output);
            Assert.That(output.Length, Is.EqualTo(6));
            Assert.That(output.Offset, Is.EqualTo(8));
        }

        [Test]
        public void ShouldReturnNoMatch()
        {
            var searchBuffer = new List<char>("abcadsada".ToCharArray());
            var lookaheadBuffer = new List<char>("lkopkpioj".ToCharArray());

            LZ77Metadata output = Compressor.FindMatch(searchBuffer, lookaheadBuffer);
            Assert.IsNotNull(output);
            Assert.That(output.Length, Is.EqualTo(0));
            Assert.That(output.Offset, Is.EqualTo(0));
        }

        [Test]
        public void ShouldCompressTooLongText()
        {
            string input = "refCgSqexBhBSLtQ7u3G5XeU30DPkv7C1p64xMdHdunEzrZRXpwNuRzxu3J9BhBSLtQ74cWr1ZpWAQg1R8ihie5V4yr3GSXQ13Hb";
            string output = Compressor.Compress(input);
            string expectedOutput = "refCgSqexBhBSLtQ7u3G5XeU30DPkv7C1p64xMdHdunEzrZRXpwNuRzxu3J9(51,8)4cWr1ZpWAQg1R8ihie5V4yr3GSXQ13Hb";

            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void TestUsingFile()
        {
            string fileContent;

            using (StreamReader reader = new StreamReader(".\\..\\..\\..\\TestFiles\\TestEncoding.txt"))
            {
                fileContent = reader.ReadToEnd();
            }

            string compressed = Compressor.Compress(fileContent);

            using (StreamWriter writer = new StreamWriter(".\\..\\..\\..\\TestFiles\\output.txt"))
            {
                writer.Write(compressed);
            }

            string decompressed = Compressor.Decompress(compressed);

            using (StreamWriter writer = new StreamWriter(".\\..\\..\\..\\TestFiles\\output_2.txt"))
            {
                writer.Write(decompressed);
            }

            Assert.That(decompressed.Length, Is.GreaterThanOrEqualTo(compressed.Length));
        }
    }
}