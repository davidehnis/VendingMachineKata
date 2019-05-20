# Virtual implementation of a vending machine

The vending machine has the following features:

1. Insert Money
2. Select Product
3. Coin Return
4. Make Change

To run build the product, run msbuild.exe on the vending.sln file.
To run all tests, run mstest.exe on the Vending.Tests.Unit dll file.

**Core Concepts**

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
