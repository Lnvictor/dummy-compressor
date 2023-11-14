namespace dummy_compressor.Algorithms
{
    public interface ICompressor
    {
        string Compress(string data);

        string Decompress(string data);
    }
}
