namespace SalesDatePrediction.Core.Domain.Entities
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Qty { get; set; }
        public float Discount { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
