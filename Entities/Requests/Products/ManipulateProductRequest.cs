namespace Entities.Requests.Products
{
    public abstract class ManipulateProductRequest
    {
        public string Name { get; set; }
        public int DefaultQuantity { get; set; }
    }
}
