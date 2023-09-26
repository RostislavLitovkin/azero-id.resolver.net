# azero-id.resolver.net

A resolver for [AZERO.ID](https://azero.id/) for C# language.

This is a programming tool meant for developers. It significantly simplifies the integration of AZERO.ID into a c# based projects.

# Installation

Nuget package: https://www.nuget.org/packages/AzeroIdResolver/
```
dotnet add package AzeroIdResolver
```

# Supported Networks

- [x] Aleph Zero Testnet
- [ ] Aleph Zero Mainnet

# Methods

- [x] GetPrimaryNameForAddress
- [x] GetNamesForAddress
- [x] GetTld
- [ ] GetAddressForPrimaryName
- [ ] GetReservedPeriodForPrimaryName
- [ ] ReserveName
- [ ] ReserveNameAndSetAsPrimary
- [ ] TransferName
- [ ] GetControlledNamesForAddress
- [ ] GetControllerAddressForName
- [ ] GetRecordForName
- [ ] SetRecordForName

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

Polkadot C# community is growing thanks to Ajuna Network and their [Substrate.NetApi](https://github.com/SubstrateGaming/Substrate.NET.API). C# is mostly popular for it's games, thanks to the popular game engines like Unity, Godot and CryEngine.

# Team

#### [Rostislav Litovkin](http://rostislavlitovkin.pythonanywhere.com/aboutme)
- Alumnus at Polkadot Blockchain Academy 2023 in Berkeley
- Experienced .net MAUI developer, e.g.:
   - [Galaxy Logic Game](https://github.com/RostislavLitovkin/galaxylogicgamemaui) (successful game for watches and mobiles, 50k+ downloads)
- Frontend developer at [Calamar explorer](https://calamar.app/)
- Successful student at Polkadot DevCamp #2
- Successful student at [Solana Summer School](https://ackeeblockchain.com/school-of-solana)
- Polkadot Global Series 2023 (Europe) - second place with https://github.com/RostislavLitovkin/PlutoWallet
- Polkadot Global Series 2023 (APEC) - second place with https://github.com/RostislavLitovkin/Uniquery.Net
- Audience choice prize at EthPrague 2023 with https://github.com/RostislavLitovkin/HackerPackerDao
