using System;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json.Linq;
using static Substrate.NetApi.Utils;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;

namespace AzeroIdResolver
{
	public class TzeroId
	{
        private static string tld = Constants.TzeroTld;

        private static string prefixedStorageKey = Constants.TzeroPrefixedStorageKey;

        private static string wssUrl = Constants.TzeroWssUrl;

        private static string graphUrl = Constants.TzeroGraphUrl;

        /// <summary>
        /// Gets a Primary name for the given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Primary name</returns>
        public static async Task<string> GetPrimaryNameForAddress(string address)
        {
            SubstrateClient client = await Helpers.GetSubstrateClient(wssUrl);

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
                prefixedStorageKey,
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
            SubstrateClient client = await Helpers.GetSubstrateClient(wssUrl);

            return await GetNamesForAddress(client, address);
        }

        /// <summary>
        /// Returns list of all registered names for a given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns>List of all registered names</returns>
        public static async Task<List<string>> GetNamesForAddress(SubstrateClient client, string address)
        {
            string rootKey = "0x2d010000";

            /// Actual code logic down here
            List<byte> rootKeyHex = new List<byte>(Utils.HexToByteArray(rootKey));

            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            // concat the rootKey and accountId param
            rootKeyHex.AddRange(accountId.Encode());

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, rootKeyHex.ToArray());

            var keysPaged = await client.InvokeAsync<JArray>("childstate_getKeys", new object[2] {
                prefixedStorageKey,
                "0x"
            }, CancellationToken.None);

            var unfilteredKeys = keysPaged.Select(p => p.ToString());

            List<string> names = new List<string>();

            foreach (string key in unfilteredKeys)
            {
                if (key.Contains(rootKey) && key.Contains(Utils.Bytes2HexString(accountId.Encode(), HexStringFormat.Pure)))
                {
                    // query the result
                    var temp = await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                        prefixedStorageKey,
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
            SubstrateClient client = await Helpers.GetSubstrateClient(wssUrl);

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
                prefixedStorageKey,
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

        /// <summary>
        /// Returns owner address of the registered name
        /// </summary>
        /// <param name="name">registered AZERO.ID name</param>
        /// <returns>Address</returns>
        public static async Task<string?> GetAddressForName(string name)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(
                graphUrl, new NewtonsoftJsonSerializer()
            );

            return await GetAddressForName(client, name);
        }

        /// <summary>
        /// Returns owner address of the registered name
        /// </summary>
        /// <param name="client">GraphQLHttpClient</param>
        /// <param name="name">registered AZERO.ID name</param>
        /// <returns>Address</returns>
        public static async Task<string?> GetAddressForName(GraphQLHttpClient client, string name)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                    query MyQuery ($where: DomainWhereInput) {
                      domains(limit: 1, where: $where) {
                        owner {
                          id
                        }
                      }
                    }",
                OperationName = "MyQuery",
                Variables = new GraphQLVariables
                {
                    where = new { id_eq = Helpers.AddTld(name, tld) },
                },
            };

            var graphQLResponse = await client.SendQueryAsync<DomainResponseType>(request);

            if (graphQLResponse.Errors != null && graphQLResponse.Errors.Length > 0)
            {
                foreach (var error in graphQLResponse.Errors)
                {
                    throw new Exception(error.Message);
                }
            }

            if (graphQLResponse.Data.Domains.Count() == 0)
            {
                return null;
            }

            return graphQLResponse.Data.Domains[0].Owner.Id;
        }

        /// <summary>
        /// Returns owner address and createdAt time of the registered name
        /// </summary>
        /// <param name="name">registered AZERO.ID name</param>
        /// <returns>Address and CreatedAt time</returns>
        public static async Task<(string, string)?> GetAddressAndRegisteredAtForName(string name)
        {
            GraphQLHttpClient client = new GraphQLHttpClient(
                graphUrl, new NewtonsoftJsonSerializer()
            );

            return await GetAddressAndRegisteredAtForName(client, name);
        }

        /// <summary>
        /// Returns owner address and createdAt time of the registered name
        /// </summary>
        /// <param name="client">GraphQLHttpClient</param>
        /// <param name="name">registered AZERO.ID name</param>
        /// <returns>Address and CreatedAt time</returns>
        public static async Task<(string, string)?> GetAddressAndRegisteredAtForName(GraphQLHttpClient client, string name)
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                    query MyQuery ($where: DomainWhereInput) {
                      domains(limit: 1, where: $where) {
                        registeredAt
                        owner {
                          id
                        }
                      }
                    }",
                OperationName = "MyQuery",
                Variables = new GraphQLVariables
                {
                    where = new { id_eq = Helpers.AddTld(name, tld) },
                },
            };

            var graphQLResponse = await client.SendQueryAsync<DomainResponseType>(request);

            if (graphQLResponse.Errors != null && graphQLResponse.Errors.Length > 0)
            {
                foreach (var error in graphQLResponse.Errors)
                {
                    throw new Exception(error.Message);
                }
            }

            if (graphQLResponse.Data.Domains.Count() == 0)
            {
                return null;
            }

            return (graphQLResponse.Data.Domains[0].Owner.Id, graphQLResponse.Data.Domains[0].RegisteredAt);
        }

        public static async Task<(DateTime, DateTime)?> GetRegistrationPeriodForName(string name)
        {
            SubstrateClient client = await Helpers.GetSubstrateClient(wssUrl);

            return await GetRegistrationPeriodForName(client, name);
        }

        public static async Task<(DateTime, DateTime)?> GetRegistrationPeriodForName(SubstrateClient client, string name)
        {
            string rootKey = "0xca000000";

            /// Actual code logic down here
            List<byte> rootKeyHex = new List<byte>(Utils.HexToByteArray(rootKey));

            byte[] nameHex = new Str(Helpers.RemoveTld(name)).Encode();

            // concat the rootKey and accountId param
            rootKeyHex.AddRange(nameHex);

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, rootKeyHex.ToArray());

            // query the result
            var temp = await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                prefixedStorageKey,
                Utils.Bytes2HexString(finalHash)
            }, CancellationToken.None);
            if (temp == null) return null;

            var byteResult = Utils.HexToByteArray(temp);

            int p = 0;

            U64 createdAt = new U64();
            createdAt.Decode(byteResult, ref p);

            U64 expiresAt = new U64();
            expiresAt.Decode(byteResult, ref p);

            // Unix timestamp is seconds past epoch
            DateTime createdAtDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            createdAtDate = createdAtDate.AddSeconds((int)(createdAt.Value / 1000)).ToLocalTime();

            DateTime expiresAtDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            expiresAtDate = expiresAtDate.AddSeconds((int)(expiresAt.Value / 1000)).ToLocalTime();

            return (createdAtDate, expiresAtDate);
        }
    }
}

