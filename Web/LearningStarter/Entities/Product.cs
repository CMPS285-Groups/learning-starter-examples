using System.Collections.Generic;

namespace LearningStarter.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }

    public class ProductGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
    }

    public class ProductCreateDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
    }

    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
    }

    public class ProductListingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}
