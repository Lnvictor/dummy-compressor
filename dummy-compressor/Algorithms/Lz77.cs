/**
 * LZ77 Compression Algorithm...
 * 
 * I hope I can do it right
 * 
 */


using dummy_compressor.Utils;

namespace dummy_compressor.Algorithms
{
    public class Lz77 : ICompressor
    {
        private const int slideWindowLenth = 60;
        private const int bufferLenth = 40;
        private const int arraySize = slideWindowLenth + bufferLenth;
        
        public byte[] Compress(byte[] data)
        {
            if (data.Length < arraySize)
            {
                throw new Exception("No Worth to compress, file too small!!");
            }

            List<char> searchBuffer;
            List<char> lookaheadBuffer;
            List<char> fullText = new List<char>(data.ToString().ToCharArray());


            // TODO: Iterate to cover whole text
            searchBuffer = fullText.GetRange(0, slideWindowLenth);
            lookaheadBuffer = fullText.GetRange(slideWindowLenth + 1, slideWindowLenth + bufferLenth);
            LZ77Metadata output = findMatch(searchBuffer, lookaheadBuffer);

            return data; // returning to intellisense be happy
        }

        public byte[] Decompress(byte[] data)
        {
            throw new NotImplementedException();
        }

        public LZ77Metadata findMatch(List<char> searchBuffer, List<char> lookaheadBuffer)
        {
            LZ77Metadata output;
            int lengthCounter = 0;
            int offset = 0;
            int j;
            int i = -1;

            for (i = lookaheadBuffer.Count; i > 0; i--)
            {
                List<char> matchCandidate = lookaheadBuffer.GetRange(0, i);
                j = 0;
                int k = 0;
                int matchStartsAt = -1;
                lengthCounter = 0;

                while (j < searchBuffer.Count && k < matchCandidate.Count)
                {
                    if (matchCandidate[k] == searchBuffer[j])
                    {
                        if (k == 0)
                        {
                            matchStartsAt = j;
                        }
                        k++;
                    }
                    else if (k != 0)
                    {
                        k = 0;
                        break;
                    }
                    j++;
                }

                if (k == matchCandidate.Count)
                {
                    lengthCounter = k;
                    offset = searchBuffer.Count - 1 - matchStartsAt;
                    break;
                }
            }


            char nextChar = (offset == 0 && lengthCounter == 0) ? lookaheadBuffer[0] : lookaheadBuffer[i];
            return new LZ77Metadata(nextChar, offset, lengthCounter);
        }
    }
}
