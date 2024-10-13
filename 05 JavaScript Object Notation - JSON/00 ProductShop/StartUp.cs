using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            //1#
            //string inputUsers = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, inputUsers));

            //2#
            //string inputProducts = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, inputProducts));

            //3#
            //string inputCategories = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, inputCategories));

            //4#
            //string inputCategoryProducts = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, inputCategoryProducts));

            //5#
            //Console.WriteLine(GetProductsInRange(context));

            //6#
            //Console.WriteLine(GetSoldProducts(context));

            //7#
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //8#
            //Console.WriteLine(GetUsersWithProducts(context));
        }
        //1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }

        //2#
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count}";
        }

        //3#
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);
            var categoriesNameNotNull = categories.Where(c => c.Name != null).ToList();
            context.Categories.AddRange(categoriesNameNotNull);
            context.SaveChanges();
            return $"Successfully imported {categoriesNameNotNull.Count}";
        }

        //4#
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);
            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count}";
        }

        //5#
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ExportProductsDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .ToList();

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var setting = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };


            string result = JsonConvert.SerializeObject(products, setting);

            return result;
        }

        //6#
        public static string GetSoldProducts(ProductShopContext context)
        {

            ////Solution without using DTO

            //var soldProducts = context.Users.Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
            //    .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            //    .Select(u => new
            //    {
            //        u.FirstName,
            //        u.LastName,
            //        SoldProducts = u.ProductsSold
            //        .Select(s => new
            //        {
            //            s.Name,
            //            s.Price,
            //            BuyerFirstName=  s.Buyer.FirstName,
            //            BuyerLastName=  s.Buyer.LastName

            //        }).ToList()
            //    })
            //    .ToList();

            //Solution using DTO

            var soldProducts = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .Select(u => new ExportSoldProductsDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new SoldProductDTO
                    {
                        Name = p.Name,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName,
                        Price = p.Price
                    })
                }).ToList();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };



            var result = JsonConvert.SerializeObject(soldProducts, settings);
            return result;
        }

        //7#
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories.OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = $"{c.CategoriesProducts.Average(cp => cp.Product.Price):f2}",
                    TotalRevenue = $"{c.CategoriesProducts.Sum(cp => cp.Product.Price):f2}"
                }).ToList();

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            string result = JsonConvert.SerializeObject(categories, settings);
            return result;
        }

        //8#
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = u.ProductsSold
                    .Where(p => p.BuyerId != null)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price
                    }).ToList()
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var ourput = new
            {
                UsersCount= users.Count,
                Users= users.Select(u=> new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts= new
                    {
                        u.SoldProducts.Count,
                        products=u.SoldProducts
                    }
                } )
            };
            var settingsSerialize = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver=new CamelCasePropertyNamesContractResolver(),
                Formatting=Formatting.Indented
            };

            var result = JsonConvert.SerializeObject(ourput,settingsSerialize);

            return result;
        }
    }
}
