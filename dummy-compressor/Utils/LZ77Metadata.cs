using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dummy_compressor.Utils
{
    public class LZ77Metadata
    {
        public char NextChar { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }

        public LZ77Metadata(char nextChar, int offset, int length) 
        {
            NextChar = nextChar;
            Offset = offset;
            Length = length;
        }
    }
}
