

using Moq;

namespace MockDB.Tests
{
    public class DBUnitTest
    {
        /// <summary>
        /// Save order.
        /// Read the order.
        /// Modify the order.
        /// Verify the proper GST was added to the Amount.
        /// </summary>
        [Fact]
        public void TestOrderProcessing()
        {

            // **** dummy order Id ****
            var orderId = 1234;

            // **** we will be potentially mocking a few methods or properties of the interface or System Under Test (SUT) ****
            Mock<IDBContext> mockDBContext = new Mock<IDBContext>();

            // **** “SaveOrder” has been mocked for “any” order passed ****
            mockDBContext.Setup(t => t.GetNextOrderDetailFromDB(It.IsAny<int>())).Returns(new Order() { OrderId = orderId, Amount = 1000 });
            mockDBContext.Setup(t => t.SaveOrder(It.IsAny<Order>()));

            // **** “orderProcessing” object will have the mocking IDBContext object injected ****
            OrderProcessing orderProcessing = new OrderProcessing();

            // **** update the order (add the GST which is 10% of the Amount) ****
            var modifiedOrder = orderProcessing.ProcessGSTForNextOrder(mockDBContext.Object, orderId);

            // **** verify the updated order amount (10% of initial Amount) ****
            Assert.Equal(1100, modifiedOrder.Amount);
        }
    }
}