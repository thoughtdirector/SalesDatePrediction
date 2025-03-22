namespace SalesDatePrediction.Core.Application.DTOs
{
    public class CustomerPredictionDto
    {
        public string CustomerName { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public DateTime? NextPredictedOrder { get; set; }
    }
}
