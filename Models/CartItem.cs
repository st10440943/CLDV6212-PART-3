namespace ABC_Retail.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; } = 1;

        public double Total => Price * Quantity;
    }
}
