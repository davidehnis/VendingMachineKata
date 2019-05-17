using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vending.Tests.Unit
{
    [TestClass]
    public class General
    {
        [TestMethod]
        public void Inserting_InvalidMetal_DoesNot_Update_Display()
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
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success());
            Assert.IsFalse(context.Display.MessageStack.Any());
        }

        [TestMethod]
        public void Inserting_InvalidMetal_Returns_Metal()
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
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success());
            Assert.IsFalse(context.Display.MessageStack.Any());
            Assert.IsTrue(context.CoinReturn.Inventory.Any());
        }

        [TestMethod]
        public void Inserting_ValidMetal_Displays_Total_By_Value()
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
            Assert.AreEqual(0.25, context.TotalDeposit);
            Assert.AreEqual("0.25", context.Display.Current);
        }

        [TestMethod]
        public void Product_Dispense_Moves_To_Start_Status()
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
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Inventory.Any());
            Assert.IsTrue(context.Display.MessageStack.Contains("THANK YOU"));
            Assert.IsTrue(context.Display.MessageStack.Contains("INSERT COIN"));
            Assert.IsTrue(context.Display.MessageStack.Contains("$0.00"));
        }

        [TestMethod]
        public void Product_Dispense_Shows_Valid_Message()
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
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Inventory.Any());
            Assert.IsTrue(context.Display.MessageStack.Contains("THANK YOU"));
        }

        [TestMethod]
        public void Return_Coins_Returns_Inserted_Coins()
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
            machine.Return(context, (r, s) => { }).Wait();

            // Arrange
            Assert.AreEqual(4, context.CoinReturn.Inventory.Count());
        }

        [TestMethod]
        public void Select_Inventory_Empty_Returns_Error()
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
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsFalse(context.ProductInventory.Any());
            Assert.IsTrue(context.Display.MessageStack.Contains("SOLD OUT"));
            Assert.IsTrue(context.Display.MessageStack.Contains("$1.00"));
        }

        [TestMethod]
        public void Select_Inventory_Has_Items_Returns_Success()
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
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Inventory.Any());
            Assert.IsTrue(context.Display.MessageStack.Contains("THANK YOU"));
            Assert.IsTrue(context.ProductReturn.Inventory.Any());
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
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Inventory.Any());
            Assert.IsTrue(context.CoinReturn.Inventory.Any());
        }

        [TestMethod]
        public void Select_Product_Correct_Amount_Inserted_Dispences()
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
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Inventory.Any());
        }

        [TestMethod]
        public void Select_Product_Empty_Inventory_Updates_Display()
        {
            // Arrange
            IContext context = new NotEnoughCoinInventoryContext();
            var machine = new Machine();
            machine.Boot(context, (r, c) => { });
            var metal = Metal.Quarter;
            IResult result = null;

            // Act
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.Display.MessageStack.Contains("SOLD OUT"));
            Assert.IsTrue(context.Display.MessageStack.Contains("$1.00"));
        }

        [TestMethod]
        public void Select_Product_Empty_Not_Enough_Coin_Updates_Display()
        {
            // Arrange
            IContext context = new NotEnoughProductInventoryContext();
            var machine = new Machine();
            machine.Boot(context, (r, c) => { });
            var metal = Metal.Quarter;
            IResult result = null;

            // Act
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.Display.MessageStack.Contains("SOLD OUT"));
            Assert.IsTrue(context.Display.MessageStack.Contains("INSERT COIN"));
        }

        [TestMethod]
        public void Select_Product_Incorrect_Amount_Updates_Display()
        {
            // Arrange
            IContext context = new Context();
            var machine = new Machine();
            var metal = Metal.Quarter;
            IResult result = null;

            // Act
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Insert(metal, context, (r, c) => { }).Wait();
            machine.Select("A", (r, s) => { }).Wait();

            // Arrange
            Assert.IsTrue(context.Display.MessageStack.Contains("PRICE"));
            Assert.IsTrue(context.Display.MessageStack.Contains("$1.00"));
        }
    }
}