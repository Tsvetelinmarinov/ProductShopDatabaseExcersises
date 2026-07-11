using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            string json = @"C:\Users\Totenkopf\Downloads\07.JSON-Processing-Exercises-ProductShop-6.0\ProductShop\Datasets\categories.json";

            Console.WriteLine(
                 value: ImportCategories(
                          context: new ProductShopContext(),
                          inputJson: File.ReadAllText(path: json)
                 )
            );
    
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

        /**
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

        /**
         * NOTE: You will need method public static string ImportCategoryProducts(ProductShopContext context, string inputJson) and public StartUp class.
         * Import the users from the provided file "categories-products.json".
         * Your method should return a string with the message: $"Successfully imported {categoryProducts.Count}"
         */

    }
}