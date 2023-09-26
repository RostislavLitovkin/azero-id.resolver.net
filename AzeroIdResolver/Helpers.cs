using Substrate.NetApi;

namespace AzeroIdResolver
{
    public class Helpers
    {
        public static string BytesToString(byte[] bytesToDecode)
        {
            int index = 0;
            return BytesToString(bytesToDecode, ref index);
        }

        public static string BytesToString(byte[] bytesToDecode, ref int index)
        {
            int tldLength = CompactInteger.Decode(bytesToDecode, ref index);

            byte[] tldBytes = new byte[tldLength];
            Array.Copy(bytesToDecode, index, tldBytes, 0, tldLength);

            index += tldLength;
            return System.Text.Encoding.UTF8.GetString(tldBytes);
        }
    }
}

