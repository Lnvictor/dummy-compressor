using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dummy_compressor.Algorithms
{
    public interface ICompressor
    {
        byte[] Compress(byte[] data);

        byte[] Decompress(byte[] data);
    }
}
