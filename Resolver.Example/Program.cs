using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using AzeroIdResolver;

// Connect to the client
// - This is optional, but recommended for better performance

SubstrateClient client = new SubstrateClient(new Uri("wss://ws.test.azero.dev"), ChargeAssetTxPayment.Default());
await client.ConnectAsync();

Console.WriteLine(await AzeroId.GetPrimaryNameForAddress(client, "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));
// returns: "rosta"

Console.WriteLine(await AzeroId.GetTld(client));
// returns: "tzero"

Console.WriteLine(await AzeroId.GetNamesForAddress(client, "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));
// returns: ["rosta", "bitcoin", "bitcoinmaxik"]

Console.WriteLine(await AzeroId.GetAddressForName("rosta.tzero"));
// returns: "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"

Console.WriteLine(await AzeroId.GetAddressAndRegisteredAtForName("bitcoin.tzero"));
// returns: ("5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y", "2023-07-27T18:03:41.000000Z")

Console.WriteLine(await AzeroId.GetAddressForName("bullshithaha.tzero"));
// returns: null

Console.WriteLine(await AzeroId.GetRegistrationPeriodForName("rosta"));
// returns: (9/26/2023 11:31:47 AM, 9/25/2026 11:31:47 AM)

Console.WriteLine(await AzeroId.GetRegistrationPeriodForName("bitcoin.tzero"));
// returns: (7/27/2023 8:03:41 PM, 7/26/2025 8:03:41 PM)

Console.ReadKey(); // This is so that the console stays open.
