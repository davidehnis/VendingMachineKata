# Virtual implementation of a vending machine

The vending machine has the following features:

1. Insert Money
2. Select Product
3. Coin Return
4. Make Change

## Building and Testing
To build and test from the command line, these instructions assume that the user has Visual Studio 2017 installed on the build and test machine(s).

To **build** the solution, run msbuild.exe on the vending.sln file.

For Example

> "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe" [repos location]\VendingMachineKata\Vending\Vending.sln

To run **unit tests**, run vstest.console.exe on the Vending.Tests.Unit dll file.

For Example:

> "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" [repos directory]\VendingMachineKata\Vending\Vending.Tests.Unit\bin\Debug\Vending.Tests.Unit.dll

## Core Concepts

Context:  Holds the current state of Machine
Machine:  Mutates context according to the rules set forth in Vending.dll
Result: A composite End state of a method on Machine
Inventory:  A collection of assets to be tracked in the Context

In Vending.Tests.Unit you will find multiple implementations of IContext to show how to setup the machine.

**Example code**
```
IContext context = new DefaultContext();
var machine = new Machine();
var metal = metal.Quarter;
var result = new Result();

machine.Insert(invalidMetal, context, (r) =>
{
    result.Push(r);
}).Wait();
```
