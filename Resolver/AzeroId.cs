using Newtonsoft.Json.Linq;
using static Substrate.NetApi.Utils;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;

namespace Resolver
{
    public class AzeroId
    {
        /// <summary>
        /// Gets a Primary name for the given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Primary name</returns>
        public static async Task<string> GetPrimaryNameForAddress(string address)
        {
            SubstrateClient client = new SubstrateClient(new Uri("wss://ws.test.azero.dev"), ChargeAssetTxPayment.Default());

            await client.ConnectAsync();

            return await GetPrimaryNameForAddress(client, address);
        }

        /// <summary>
        /// Gets a Primary name for the given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Primary name</returns>
        public static async Task<string> GetPrimaryNameForAddress(SubstrateClient client, string address)
        {
            string rootKey = "0x8f010000";

            /// Actual code logic down here
            List<byte> rootKeyHex = new List<byte>(Utils.HexToByteArray(rootKey));

            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            // concat the rootKey and accountId param
            rootKeyHex.AddRange(accountId.Encode());

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, rootKeyHex.ToArray());

            // query the result
            var temp = await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                Constants.TZeroIdPrefixedStorageKey,
                Utils.Bytes2HexString(finalHash)
            }, CancellationToken.None);
            if (temp == null) return null;

            var result = Utils.HexToByteArray(temp);

            // decode the bytes to UTF-8 string
            return Helpers.BytesToString(result);
        }

        /// <summary>
        /// Returns list of all registered names for a given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns>List of all registered names</returns>
        public static async Task<List<string>> GetNamesForAddress(string address)
        {
            SubstrateClient client = new SubstrateClient(new Uri("wss://ws.test.azero.dev"), ChargeAssetTxPayment.Default());

            await client.ConnectAsync();

            return await GetNamesForAddress(client, address);
        }

        /// <summary>
        /// Returns list of all registered names for a given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns>List of all registered names</returns>
        public static async Task<List<string>> GetNamesForAddress(SubstrateClient client, string address)
        {
            string rootKey = "2d010000";

            /// Actual code logic down here
            List<byte> rootKeyHex = new List<byte>(Utils.HexToByteArray(rootKey));

            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            // concat the rootKey and accountId param
            rootKeyHex.AddRange(accountId.Encode());

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, rootKeyHex.ToArray());

            var keysPaged = await client.InvokeAsync<JArray>("childstate_getKeys", new object[2] {
                Constants.TZeroIdPrefixedStorageKey,
                "0x"
            }, CancellationToken.None);

            var unfilteredKeys = keysPaged.Select(p => p.ToString());

            // TO BE CONTINUED ...
            List<string> names = new List<string>();

            foreach (string key in unfilteredKeys)
            {
                if (key.Contains(rootKey) && key.Contains(Utils.Bytes2HexString(accountId.Encode(), HexStringFormat.Pure)))
                {
                    // query the result
                    var temp = await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                        Constants.TZeroIdPrefixedStorageKey,
                        key
                    }, CancellationToken.None);

                    if (temp == null) return null;

                    var result = Utils.HexToByteArray(temp);

                    // decode the bytes to UTF-8 string
                    names.Add(Helpers.BytesToString(result));
                }
            }

            return names;
        }

        /// <summary>
        /// Gets the TLD
        /// </summary>
        /// <returns>TLD</returns>
        public static async Task<string> GetTld()
        {
            SubstrateClient client = new SubstrateClient(new Uri("wss://ws.test.azero.dev"), ChargeAssetTxPayment.Default());

            await client.ConnectAsync();

            return await GetTld(client);
        }

        /// <summary>
        /// Gets the TLD
        /// </summary>
        /// <returns>TLD</returns>
        public static async Task<string> GetTld(SubstrateClient client)
        {
            string rootKey = "0x00000000";

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, Utils.HexToByteArray(rootKey));

            // query the result
            var result = Utils.HexToByteArray(await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                Constants.TZeroIdPrefixedStorageKey,
                Utils.Bytes2HexString(finalHash)
            }, CancellationToken.None));

            // 2 - 0x (start)
            // 64 - accountId32 (admin)
            // 2/66 - Option<accountId32> (pending_admin)
            // xx - CompactInteger + string (tld)
            // xx - (other)

            // decode the bytes to UTF-8 string
            int index = result[32] == 0 ? 33 : 65;

            return Helpers.BytesToString(result, ref index);
        }
    }
}

