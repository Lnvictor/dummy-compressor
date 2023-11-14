/**
 * |        _____      ______     ______
 * |            /           /          /
 * |           /           /          /
 * |          /           /          /
 * |_____    /_____      /          /
 * 
 * 
 * LZ77 Compression Algorithm implemented using 60/40 proportion
 * 
 */


using dummy_compressor.Utils;

namespace dummy_compressor.Algorithms
{
    public class Lz77 : ICompressor
    {
        private const int SearchBufferLength = 60;
        private const int LookaheadBufferLength = 40;
        private const int ArraySize = SearchBufferLength + LookaheadBufferLength;


        public string Compress(string data)
        {
            if (data.Length < ArraySize)
            {
                throw new Exception("No Worth to compress, file too small!!");
            }

            List<char> sliddingWindow;
            List<char> searchBuffer;
            List<char> lookaheadBuffer;
            List<char> fullText = new List<char>(data.ToCharArray());

            string output = "";
            int currentPosition = 0;

            sliddingWindow = fullText.GetRange(currentPosition, ArraySize);
            searchBuffer = sliddingWindow.GetRange(0, SearchBufferLength);
            ProcessCanonicalSearchBuffer(ref output, searchBuffer);

            while ((currentPosition + ArraySize) <= fullText.Count)
            {
                sliddingWindow = fullText.GetRange(currentPosition, ArraySize);
                searchBuffer = sliddingWindow.GetRange(0, SearchBufferLength);
                lookaheadBuffer = sliddingWindow.GetRange(searchBuffer.Count, LookaheadBufferLength);
                LZ77Metadata metadata = FindMatch(searchBuffer, lookaheadBuffer);

                // Value will be used when the value is most efficient saving memory than the pair
                if (!(metadata.Value is null))
                {
                    output += metadata.Value;
                    currentPosition += metadata.Value.Length;
                    continue;
                }

                output += $"({metadata.Offset},{metadata.Length})";
                currentPosition += metadata.Length;
            }

            if (currentPosition < fullText.Count - 1)
            {
                currentPosition = currentPosition + SearchBufferLength;
                output += new string(fullText.GetRange(currentPosition, fullText.Count - currentPosition).ToArray());
            }

            return output;
        }

        public string Decompress(string data)
        {
            string fullText = data;
            string actualValue;
            string output = "";

            int currentPosition = 0;
            int length;

            while (currentPosition < fullText.Length)
            {
                if (fullText[currentPosition] == '(')
                {
                    int separatorIndex = fullText.IndexOf(',', currentPosition + 1);
                    int lastChar = fullText.IndexOf(')', currentPosition + 1);
                    int offset = Convert.ToInt32(fullText.Substring(currentPosition + 1, (separatorIndex - currentPosition - 1)));
                    length = Convert.ToInt32(fullText.Substring(separatorIndex + 1, (lastChar - separatorIndex - 1)));

                    actualValue = output.Substring(output.Length - offset, length);
                    output += actualValue;
                    currentPosition += (lastChar + 1) - currentPosition;
                    continue;
                }

                int nextPairIndex = fullText.IndexOf("(", currentPosition + 1);

                if (nextPairIndex < 0)
                {
                    nextPairIndex = fullText.Length;
                }

                length = nextPairIndex - currentPosition;
                actualValue = fullText.Substring(currentPosition, length);
                output += actualValue;
                currentPosition += length;
            }

            return output;
        }

        public LZ77Metadata FindMatch(List<char> searchBuffer, List<char> lookaheadBuffer)
        {
            string matchString = "";
            int lengthCounter = 0;
            int offset = 0;
            int j;
            int i;

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
                    matchString = new string(matchCandidate.ToArray());
                    lengthCounter = k;
                    offset = searchBuffer.Count - matchStartsAt;
                    break;
                }
            }

            string? value = null;

            if ($"({offset},{lengthCounter})".Length > matchString.Length)
            {
                offset = 0;
                lengthCounter = 0;
                value = new string(lookaheadBuffer.ToArray());
            }


            return new LZ77Metadata(value, offset, lengthCounter);
        }

        private void ProcessCanonicalSearchBuffer(ref string output, List<char> searchBuffer)
        {
            string t = new string(searchBuffer.ToArray());
            output += t;
        }
    }
}
