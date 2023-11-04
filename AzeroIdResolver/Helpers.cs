using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;

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

        public static string RemoveTld(string name)
        {
            return name.Split(".")[0];
        }

        public static string AddTld(string name, string tld)
        {
            return name.Contains(".") ? name : name + tld;
        }

        public static async Task<SubstrateClient> GetSubstrateClient(string wssUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            SubstrateClient client = new SubstrateClient(new Uri(wssUrl), ChargeAssetTxPayment.Default());

            await client.ConnectAsync(cancellationToken);

            return client;
        }
    }
}

