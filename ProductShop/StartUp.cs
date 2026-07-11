using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine();
        }

        /*
         * NOTE: You will need method public static string ImportUsers(ProductShopContext context, string inputJson)
         * and public StartUp class.
         * Import the users from the provided file "users.json".
         * Your method should return a string with the following message: $"Successfully imported {users.Count}"
         */
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(value: inputJson)
                    ?? new List<User>();

            context = new();
            context.Users.AddRange(entities: users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        /*
         * NOTE: You will need method public static string ImportProducts(ProductShopContext context, string inputJson) and public StartUp class.
         * Import the users from the provided file "products.json".
         * Your method should return a string with the following message: $"Successfully imported {products.Count}";
         */
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(value: inputJson)
                    ?? new List<Product>();

            context = new();
            context.Products.AddRange(entities: products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        /*
         * NOTE: You will need method public static string ImportCategories(ProductShopContext context, string inputJson) and public StartUp class.
         * Import the users from the provided file "categories.json". Some of the names will be null, so you don't have to add them to the database.
         * Just skip the record and continue.
         * Your method should return a string with the following message: $"Successfully imported {categories.Count}";
        */
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categ = JsonConvert.DeserializeObject<List<Category>>(value: inputJson)
                    ?? new List<Category>();

            context = new();
            context.Categories.AddRange(entities: categ.Where(predicate: (categ) => categ.Name != null));
            context.SaveChanges();

            return $"Successfully imported {categ.Count}";
        }

        /*
         * NOTE: You will need method public static string ImportCategoryProducts(ProductShopContext context, string inputJson) and public StartUp class.
         * Import the users from the provided file "categories-products.json".
         * Your method should return a string with the message: $"Successfully imported {categoryProducts.Count}"
         */
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            context.CategoriesProducts
                .Include(navigationPropertyPath: (catProd) => catProd.Product)
                .Include(navigationPropertyPath: (catProd) => catProd.Category);

            List<CategoryProduct> categoryProducts
                = JsonConvert.DeserializeObject<List<CategoryProduct>>(value: inputJson)
                    ?? new List<CategoryProduct>();

            context = new();
            context.CategoriesProducts.AddRange(entities: categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        /*
         * NOTE: You will need method public static string GetProductsInRange(ProductShopContext context) and public StartUp class.
         * Get all products in a specified price range: 500 to 1000 (inclusive). 
         * Order them by price (from lowest to highest). Select only the product name, price and the full name of the seller. 
         * Export the result to JSON(products-in-range.json)
         */
        public static string GetProductsInRange(ProductShopContext context)
        {
            context = new();

            List<Product> products = context
                .Products
                .Include(navigationPropertyPath: (prod) => prod.Seller)
                .Where(predicate: (prod) => prod.Price >= 500 && prod.Price <= 1000)
                .OrderBy(keySelector: (prod) => prod.Price)
                .ToList();

            var selectedProds = products.Select(
                selector: (prod) => new 
                { 
                    prod.Name, 
                    prod.Price, 
                    SellerFullName = string.Concat(prod.Seller.FirstName, " ", prod.Seller.LastName)
                }
            );

            return
                JsonConvert.SerializeObject(
                    value: selectedProds,
                    settings: new JsonSerializerSettings()
                    {
                        ContractResolver = new DefaultContractResolver()
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                        Formatting = Formatting.Indented
                    }
                );
        }
    }
}