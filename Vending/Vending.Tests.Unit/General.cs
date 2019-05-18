using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vending.Tests.Unit
{
    [TestClass]
    public class General
    {
        [TestMethod]
        public void AcceptCoins_InvalidMetal_After_Valid_Returns_Metal()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var invalidMetal = new Metal(0, 0, 0);
            IResult result = null;

            // Act
            machine.Insert(Metal.Quarter, context, (r, c) =>
            {
                context = c;
                result = r;
            }).Wait();

            machine.Insert(invalidMetal, context, (r, c) =>
            {
                context = c;
                result = r;
            }).Wait();

            // Assert
            Assert.IsTrue(context.CoinReturn.Items.Any());
            Assert.AreEqual("0.25", context.Display.Current);
        }

        [TestMethod]
        public void AcceptCoins_InvalidMetal_DoesNot_Update_Display()
        {
            // Arrange
            var result = new Result();
            IContext context = new DefaultContext();
            var machine = new Machine();
            machine.Boot(context, (r, c) =>
            {
                context = c;
                result.Push(r);
            });
            var invalidMetal = new Metal(0, 0, 0);

            // Act
            machine.Insert(invalidMetal, context, (r, c) =>
            {
                context = c;
                result.Push(r);
            }).Wait();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success());
            Assert.IsTrue(context.Display.MessageStack.Any());
            Assert.AreEqual("INSERT COIN", context.Display.Current);
        }

        [TestMethod]
        public void AcceptCoins_InvalidMetal_Returns_Metal()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var invalidMetal = new Metal(0, 0, 0);
            IResult result = null;

            // Act
            machine.Insert(invalidMetal, context, (r, c) =>
            {
                context = c;
                result = r;
            }).Wait();

            // Assert
            Assert.IsTrue(context.CoinReturn.Items.Any());
        }

        [TestMethod]
        public void AcceptCoins_ValidMetal_Displays_Total_By_Running_Total()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            IResult result = null;

            // Act
            machine.Insert(metal, context, (r, c) =>
            {
                context = c;
                result = r;
            }).Wait();

            machine.Insert(metal, context, (r, c) =>
            {
                context = c;
                result = r;
            }).Wait();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success());
            Assert.AreEqual(new decimal(0.50), context.TotalDeposit);
            Assert.AreEqual("0.50", context.Display.Current);
        }

        [TestMethod]
        public void AcceptCoins_ValidMetal_Displays_Total_By_Value()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            IResult result = null;

            // Act
            machine.Insert(metal, context, (r, c) =>
            {
                context = c;
                result = r;
            }).Wait();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success());
            Assert.AreEqual(new decimal(0.25), context.TotalDeposit);
            Assert.AreEqual("0.25", context.Display.Current);
        }

        [TestMethod]
        public void ReturnCoins_Returns_Inserted_Coins()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Return(context, (r, s) => { result.Push(r); }).Wait();

            // Arrange
            Assert.AreEqual(4, context.CoinReturn.Items.Count());
        }

        [TestMethod]
        public void Select_Product_Amount_Too_Much_Makes_Change()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            IResult result = null;

            // Act
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Select(context, "A", (r, c) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Items.Any());
            Assert.IsTrue(context.CoinReturn.Items.Any());
        }

        [TestMethod]
        public void Select_Product_Correct_Amount_Inserted_Dispences()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r, s) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(result.Success());
            Assert.IsTrue(context.ProductReturn.Items.Any());
        }

        [TestMethod]
        public void Select_Product_Empty_Inventory_Updates_Display()
        {
            // Arrange
            IContext context = new NotEnoughProductInventoryContext();
            var machine = new Machine();
            machine.Boot(context, (r, c) => { });
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r, s) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.Display.MessageStack.Contains(Tags.Sold_Out));
            Assert.IsTrue(context.Display.MessageStack.Contains("1.00"));
        }

        [TestMethod]
        public void Select_Product_Empty_Not_Enough_Coin_Updates_Display()
        {
            // Arrange
            IContext context = new NotEnoughProductInventoryContext();
            var machine = new Machine();
            machine.Boot(context, (r, c) => { });
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Select(context, "A", (r, s) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.Display.MessageStack.Contains(Tags.Sold_Out));
            Assert.IsTrue(context.Display.MessageStack.Contains(Tags.Insert_Coin));
        }

        [TestMethod]
        public void Select_Product_Incorrect_Amount_Updates_Display()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            IResult result = null;

            // Act
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Select(context, "A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.Display.MessageStack.Contains(Tags.Price));
            Assert.IsTrue(context.Display.MessageStack.Contains("$1.00"));
        }

        [TestMethod]
        public void SelectProduct_Dispense_Moves_To_Start_Status()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r, s) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Items.Any());
            Assert.IsTrue(context.Display.MessageStack.Contains("THANK YOU"));
            Assert.IsTrue(context.Display.MessageStack.Contains("INSERT COIN"));
            Assert.IsTrue(context.Display.MessageStack.Contains("$0.00"));
        }

        [TestMethod]
        public void SelectProduct_Inventory_Empty_Returns_Error()
        {
            // Arrange
            IContext context = new NotEnoughProductInventoryContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r, s) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsFalse(context.AvailableBins.Items.Any(i => i.Id == "A"));
            Assert.IsTrue(context.Display.MessageStack.Contains(Tags.Sold_Out));
            Assert.IsTrue(context.Display.MessageStack.Contains("1.00"));
        }

        [TestMethod]
        public void SelectProduct_Inventory_Has_Items_Returns_Success()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r, c) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r, s) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(result.Success());
            Assert.AreEqual(1, context.ProductReturn.Items.Count());
            Assert.IsTrue(context.Display.MessageStack.Contains("THANK YOU"));
            Assert.IsTrue(context.ProductReturn.Items.Any());
        }

        [TestMethod]
        public void SelectProduct_Product_Dispense_Shows_Valid_Message()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            IResult result = null;

            // Act
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Select(context, "A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Items.Any());
            Assert.IsTrue(context.Display.MessageStack.Contains("THANK YOU"));
        }
    }
}