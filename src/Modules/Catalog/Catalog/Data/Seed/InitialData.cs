namespace Catalog.Data.Seed
{
    public static class InitialData
    {
        public static IEnumerable<Product> Products()
        {
            return
            [
                Product.Create("Test data 1", ["Phone", "Laptop"], "Test description 1", null, 3600000000),
                Product.Create("Test data 2", ["headphone", "headset"], "Test description 1", null, 160000000)
            ];
        }
    }
}
