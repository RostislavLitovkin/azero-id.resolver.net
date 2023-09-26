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

# Motivation

This tool simplifies the use of AZERO.ID in c# based applications. Without this tool, developers would have to learn how to ink! smart contracts work, how to query the state within the contract described [here](https://use.ink/datastructures/storage-in-metadata#accessing-storage-items-with-the-childstate-rpc-call-api). This is very time consuming and it takes douzens of hours to fully implement.

Thanks to this Nuget package, it can be implemented and used in literally minutes.

To get the AZERO.ID Primary name, all you need to do is this:

```C#
using AzeroIdResolver;

Console.WriteLine(await AzeroId.GetPrimaryNameForAddress("5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));
// returns: "rosta"
```


