namespace dummy_compressor.Utils
{
    public class LZ77Metadata
    {
        public int Offset { get; set; }
        public int Length { get; set; }

        public string? Value { get; set; }

        public LZ77Metadata(string? value, int offset, int length)
        {
            Value = value;
            Offset = offset;
            Length = length;
        }
    }
}
