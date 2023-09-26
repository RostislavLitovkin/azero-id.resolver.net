using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Resolver;

// Connect to the client
// - This is optional, but recommended for better performance
SubstrateClient client = new SubstrateClient(new Uri("wss://ws.test.azero.dev"), ChargeAssetTxPayment.Default());
await client.ConnectAsync();

Console.WriteLine(await AzeroId.GetPrimaryNameForAddress(client, "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));
// returns: "rosta"

Console.WriteLine(await AzeroId.GetTld(client));
// returns: tzero

Console.WriteLine(await AzeroId.GetNamesForAddress(client, "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));
// returns: ["rosta", "bitcoin", "bitcoinmaxik"]

Console.ReadKey(); // This is so that the console stays open.
