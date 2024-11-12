namespace KoiPool_Project.Models
{
    public class CartModel
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get { return Quantity * Price; }
        }
        public CartModel()
        {

        }
        public CartModel(ProductModel product)
        {
            ProductID = product.Id;
            ProductName = product.Name;
            Price = product.Price;
            Quantity = 1;
        }
    }
}
