using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vending.Tests.Unit
{
    [TestClass]
    public class General
    {
        [TestMethod]
        public void AcceptCoins_01_Neg_InvalidMetal_After_Valid_Returns_Metal()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var result = new Result();

            // Act
            machine.Insert(Metal.Quarter, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            machine.Insert(Metal.Nickel, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            machine.Insert(Metal.Dime, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.IsFalse(context.CoinReturn.Items.Any());
        }

        [TestMethod]
        public void AcceptCoins_01_Pos_InvalidMetal_After_Valid_Returns_Metal()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var invalidMetal = new Metal(0, 0, 0);
            var result = new Result();

            // Act
            machine.Insert(Metal.Quarter, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            machine.Insert(invalidMetal, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.IsTrue(context.CoinReturn.Items.Any());
            Assert.AreEqual("$0.25", context.DisplayAmount.Current);
        }

        [TestMethod]
        public void AcceptCoins_02_Neg_InvalidMetal_DoesNot_Update_Display()
        {
            // Arrange
            var result = new Result();
            IContext context = new DefaultContext();
            var machine = new Machine();
            machine.Boot(context, (r) =>
            {
                result.Push(r);
            });
            var invalidMetal = new Metal(0, 0, 0);

            // Act
            machine.Insert(invalidMetal, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success());
            Assert.IsTrue(context.DisplayMessage.MessageStack.Any());
            Assert.AreEqual("INSERT COIN", context.DisplayMessage.Current);
        }

        [TestMethod]
        public void AcceptCoins_02_Neg_ValidMetal_Displays_Total_By_Running_Total()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var invalidMetal = new Metal(0, 0, 0);
            var result = new Result();

            // Act
            machine.Insert(invalidMetal, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.AreEqual(new decimal(0.0), context.TotalDeposit);
            Assert.AreEqual("$0.00", context.DisplayAmount.Current);
        }

        [TestMethod]
        public void AcceptCoins_02_Pos_ValidMetal_Displays_Total_By_Running_Total()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            machine.Insert(metal, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success());
            Assert.AreEqual(new decimal(0.50), context.TotalDeposit);
            Assert.AreEqual("$0.50", context.DisplayAmount.Current);
        }

        [TestMethod]
        public void AcceptCoins_02_Pos_ValidMetal_Displays_Total_By_Value()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success());
            Assert.AreEqual(new decimal(0.25), context.TotalDeposit);
            Assert.AreEqual("$0.25", context.DisplayAmount.Current);
        }

        [TestMethod]
        public void AcceptCoins_02_Pos_ValidMetal_Updates_Display()
        {
            // Arrange
            var result = new Result();
            IContext context = new DefaultContext();
            var machine = new Machine();

            // Act
            machine.Insert(Metal.Quarter, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            machine.Insert(Metal.Dime, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.IsTrue(context.DisplayAmount.MessageStack.Contains("$0.25"));
            Assert.IsTrue(context.DisplayAmount.MessageStack.Contains("$0.35"));
            Assert.AreEqual(Tags.InsertCoin, context.DisplayMessage.Current);
        }

        [TestMethod]
        public void AcceptCoins_04_Neg_InvalidMetal_Returns_Metal()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var result = new Result();

            // Act
            machine.Insert(Metal.Dime, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            machine.Insert(Metal.Dime, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            machine.Insert(Metal.Dime, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.AreEqual(0, context.CoinReturn.Items.Count());
        }

        [TestMethod]
        public void AcceptCoins_04_Pos_InvalidMetal_Returns_Metal()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var invalidMetal = new Metal(0, 0, 0);
            var result = new Result();

            // Act
            machine.Insert(invalidMetal, context, (r) =>
            {
                result.Push(r);
            }).Wait();

            // Assert
            Assert.AreEqual(1, context.CoinReturn.Items.Count());
        }

        [TestMethod]
        public void MakeChange_01_Neg_Returns_Change()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var result = new Result();

            // Act
            machine.Select(context, "B", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.AreEqual(new decimal(0), context.CoinReturn.TotalValue());
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.InsertCoin));
        }

        [TestMethod]
        public void MakeChange_01_Pos_Returns_Change()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var result = new Result();

            // Act
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "B", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.AreEqual(new decimal(1.50), context.CoinReturn.TotalValue());
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.InsertCoin));
        }

        [TestMethod]
        public void ReturnCoins_01_Neg_Returns_Inserted_Coins()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var result = new Result();

            // Act
            machine.Return(context, (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.AreEqual(0, context.CoinReturn.Items.Count());
            Assert.AreEqual(Tags.InsertCoin, context.DisplayMessage.Current);
        }

        [TestMethod]
        public void ReturnCoins_01_Pos_Returns_Inserted_Coins()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Return(context, (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.AreEqual(4, context.CoinReturn.Items.Count());
            Assert.AreEqual(Tags.InsertCoin, context.DisplayMessage.Current);
        }

        [TestMethod]
        public void Select_Product_Amount_Too_Much_Makes_Change()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

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
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(result.Success());
            Assert.IsTrue(context.ProductReturn.Items.Any());
        }

        [TestMethod]
        public void Select_Product_Empty_Inventory_Updates_Display()
        {
            // Arrange
            var context = new NotEnoughProductInventoryContext();
            var result = new Result();
            var machine = new Machine();
            machine.Boot(context, (r) => { result.Push(r); });
            var metal = Metal.Quarter;

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.SoldOut));
            Assert.IsTrue(context.DisplayAmount.MessageStack.Contains("$1.00"));
        }

        [TestMethod]
        public void Select_Product_Empty_Not_Enough_Coin_Updates_Display()
        {
            // Arrange
            IContext context = new NotEnoughProductInventoryContext();
            var result = new Result();
            var machine = new Machine();
            machine.Boot(context, (r) => { result.Push(r); });
            var metal = Metal.Quarter;

            // Act
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.SoldOut));
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.InsertCoin));
        }

        [TestMethod]
        public void Select_Product_Incorrect_Amount_Updates_Display()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.Price));
            Assert.IsTrue(context.DisplayAmount.MessageStack.Contains("$1.00"));
        }

        [TestMethod]
        public void SelectProduct_01_Pos_Valid_Products()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "D", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsFalse(context.ProductReturn.Items.Any());
            Assert.IsFalse(context.DisplayAmount.MessageStack.Contains("$0.00"));
        }

        [TestMethod]
        public void SelectProduct_02_Neg_Dispense_Moves_To_Start_Status()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();
            Thread.Sleep(50);

            // Arrange
            Assert.IsFalse(context.ProductReturn.Items.Any());
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.InsertCoin));
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.Price));
            Assert.IsTrue(context.DisplayAmount.MessageStack.Contains("$0.00"));
        }

        [TestMethod]
        public void SelectProduct_02_Pos_Dispense_Moves_To_Start_Status()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Items.Any());
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.ThankYou));
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.InsertCoin));
        }

        [TestMethod]
        public void SelectProduct_03_Neg_Not_Enough_Money_Deposited()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var result = new Result();

            // Act
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(Metal.Quarter, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Items.Any());
            Assert.IsFalse(context.DisplayMessage.MessageStack.Contains(Tags.Price));
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.InsertCoin));
            Assert.IsTrue(context.DisplayAmount.MessageStack.Contains("$1.00"));
        }

        [TestMethod]
        public void SelectProduct_03_Pos_Not_Enough_Money_Deposited()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var result = new Result();

            // Act

            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsFalse(context.ProductReturn.Items.Any());
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.InsertCoin));
            Assert.IsTrue(context.DisplayAmount.MessageStack.Contains("$1.00"));
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
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(result.Success());
            Assert.AreEqual(1, context.ProductReturn.Items.Count());
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains("THANK YOU"));
            Assert.IsTrue(context.ProductReturn.Items.Any());
        }

        [TestMethod]
        public void SelectProduct_Product_Dispense_Shows_Valid_Message()
        {
            // Arrange
            IContext context = new DefaultContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsTrue(context.ProductReturn.Items.Any());
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains("THANK YOU"));
        }

        [TestMethod]
        public void SoldOut_01_Neg_Inventory_Empty_Returns_Error()
        {
            // Arrange
            IContext context = new NotEnoughProductInventoryContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsFalse(context.AvailableBins.Items.Any(i => i.Id == "A"));
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.SoldOut));
            Assert.IsFalse(context.DisplayMessage.MessageStack.Contains("1.00"));
        }

        [TestMethod]
        public void SoldOut_01_Pos_Inventory_Empty_Returns_Error()
        {
            // Arrange
            IContext context = new NotEnoughProductInventoryContext();
            var machine = new Machine();
            var metal = Metal.Quarter;
            var result = new Result();

            // Act
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Insert(metal, context, (r) => { result.Push(r); }).Wait();
            machine.Select(context, "A", (r) => { result.Push(r); }).Wait();

            // Arrange
            Assert.IsFalse(context.AvailableBins.Items.Any(i => i.Id == "A"));
            Assert.IsTrue(context.DisplayMessage.MessageStack.Contains(Tags.SoldOut));
            Assert.IsTrue(context.DisplayAmount.MessageStack.Contains("$1.00"));
        }
    }
}