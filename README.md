# ink! hackathon by Encode

Everything in this repo has been done until the 29.10.2023 23:59 (deadline) for the ink! hackathon.

A few parts of the project have been done before the start of the hackathon.

- 3 min YouTube demo: https://www.youtube.com/watch?v=YOWEs3ZIZgw

# azero-id.resolver.net

A resolver for [AZERO.ID](https://azero.id/) for C# language.

This is a programming tool meant for developers. It significantly simplifies the integration of AZERO.ID into a c# based projects.

# Installation

Nuget package: https://www.nuget.org/packages/AzeroIdResolver/
```
dotnet add package AzeroIdResolver
```

# Supported Networks

- Aleph Zero Testnet
- Aleph Zero Mainnet

# Methods

- GetPrimaryNameForAddress
- GetNamesForAddress
- GetTld
- GetAddressForName
- GetRegistrationPeriodForName
- ReserveName
- ReserveNameAndSetAsPrimary
- TransferName
- GetControlledNamesForAddress
- GetControllerAddressForName
- GetRecordForName
- SetRecordForName

All of these methods are also well in-line documented:
<img width="974" alt="Screenshot 2023-09-26 at 16 44 47" src="https://github.com/RostislavLitovkin/azero-id.resolver.net/assets/77352013/83189d9e-989d-4b78-bf38-5ad0bfa30c66">

# Motivation

This tool simplifies the use of AZERO.ID in c# based applications. Without this tool, developers would have to learn how ink! smart contracts work and how to query the state within the contract, which is described [here](https://use.ink/datastructures/storage-in-metadata#accessing-storage-items-with-the-childstate-rpc-call-api). It is very time consuming and it takes douzens of hours to fully implement.

Thanks to this Nuget package, it can be implemented and used in literally minutes.

To get the AZERO.ID Primary name, all you need to do is this:

```C#
using AzeroIdResolver;

Console.WriteLine(await AzeroId.GetPrimaryNameForAddress("5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));
// returns: "rosta"
```

Other examples:

```C#
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using AzeroIdResolver;

// Connect to the client
// - This is optional, but recommended for better performance
SubstrateClient client = new SubstrateClient(new Uri("wss://ws.test.azero.dev"), ChargeAssetTxPayment.Default());
await client.ConnectAsync();

Console.WriteLine(await TzeroId.GetPrimaryNameForAddress(client, "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));
// returns: "rosta"

Console.WriteLine(await TzeroId.GetTld(client));
// returns: "tzero"

Console.WriteLine(await TzeroId.GetNamesForAddress(client, "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));
// returns: ["rosta", "bitcoin", "bitcoinmaxik"]

Console.WriteLine(await TzeroId.GetAddressForName("rosta.tzero"));
// returns: "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"

Console.WriteLine(await TzeroId.GetAddressAndRegisteredAtForName("bitcoin.tzero"));
// returns: ("5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y", "2023-07-27T18:03:41.000000Z")

Console.WriteLine(await TzeroId.GetAddressForName("bullshithaha.tzero"));
// returns: null

Console.WriteLine(await TzeroId.GetRegistrationPeriodForName(client, "rosta"));
// returns: (9/26/2023 11:31:47 AM, 9/25/2026 11:31:47 AM)

Console.WriteLine(await TzeroId.GetRegistrationPeriodForName(client, "bitcoin.tzero"));
// returns: (7/27/2023 8:03:41 PM, 7/26/2025 8:03:41 PM)

Console.WriteLine(await AzeroId.GetAddressForName("bitcoin.azero"));
// returns: "5G6ieicxdNkEMq62NV5hnGRMvcWa5hnVotEahyq2ujAeJDZ5"

Console.ReadKey(); // This is so that the console stays open.
```

Polkadot C# community is growing thanks to Ajuna Network and their [Substrate.NetApi](https://github.com/SubstrateGaming/Substrate.NET.API). C# is mostly popular for it's games, thanks to the popular game engines like Unity, Godot and CryEngine.

# Example usages

<img width="401" alt="Screenshot 2023-10-29 at 23 57 15" src="https://github.com/RostislavLitovkin/azero-id.resolver.net/assets/77352013/375048ea-16b8-4b99-a859-36d046ca4a44">

<img width="401" alt="Screenshot 2023-10-29 at 23 57 35" src="https://github.com/RostislavLitovkin/azero-id.resolver.net/assets/77352013/932b4d73-ea70-4db4-a7bf-ec998333b2b5">

![image](https://github.com/RostislavLitovkin/azero-id.resolver.net/assets/77352013/53a38247-1577-4761-8981-68ffe8ef5b6d)

# Team

#### [Rostislav Litovkin](http://rostislavlitovkin.pythonanywhere.com/aboutme)
- Alumnus at Polkadot Blockchain Academy 2023 in Berkeley
- Experienced .net MAUI developer, e.g.:
   - [Galaxy Logic Game](https://github.com/RostislavLitovkin/galaxylogicgamemaui) (successful game for watches and mobiles, 50k+ downloads)
- Frontend developer at [Calamar explorer](https://calamar.app/)
- Successful student at Polkadot DevCamp #2
- Successful student at [Solana Summer School](https://ackeeblockchain.com/school-of-solana)
- Polkadot Global Series 2023 (Europe) - second place with [PlutoWallet](https://github.com/RostislavLitovkin/PlutoWallet)
- Polkadot Global Series 2023 (APEC) - second place with [Uniquery.Net](https://github.com/RostislavLitovkin/Uniquery.Net)
- Audience choice prize at EthPrague 2023 with [HackerPackerDao](https://github.com/RostislavLitovkin/HackerPackerDao)

Thanks to my experience with other packages, like Uniquery.Net, I think I will be perfect fit for this project.

# Projects utilising azero-id.resolver.net

- https://github.com/RostislavLitovkin/PlutoWallet/tree/AlephZeroId

# Techstack used

- C# programming language
- [Substrate.NetApi](https://github.com/SubstrateGaming/Substrate.NET.API)
