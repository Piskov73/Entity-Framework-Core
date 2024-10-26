using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text;
using System.Xml.Serialization;


using System.Xml;


using System.Xml.Linq;


namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            //1#
            //string inputuser = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, inputuser));

            //2#
            //string inputProducts = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, inputProducts));

            //3#
            //string inpueCategories = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, inpueCategories));

            //4#
            //string categoryProductInput = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductInput));

            //5#
            // Console.WriteLine(GetProductsInRange(context));

            //6#
            //Console.WriteLine(GetSoldProducts(context));

            //7#
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //8#
            Console.WriteLine(GetUsersWithProducts(context));

        }
        //1#
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserImportDTO[]), new XmlRootAttribute("Users"));
            UserImportDTO[] userImports;
            using (var reader = new StringReader(inputXml))
            {
                userImports = (UserImportDTO[])xmlSerializer.Deserialize(reader);
            };

            var users = userImports.Select(u => new User
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age,
            }).ToArray();

            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}";
        }
        //2#
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProductImportDTO[]), new XmlRootAttribute("Products"));
            ProductImportDTO[] productImports;
            using (var reader = new StringReader(inputXml))
            {
                productImports = (ProductImportDTO[])xmlSerializer.Deserialize(reader);
            }

            var products = productImports
                .Select(p => new Product
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerId = p.SellerId,
                    BuyerId = p.BuyerId == 0 ? (int?)null : p.BuyerId
                }).ToArray();
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
        //3#
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CategoryImportDTO>)
                , new XmlRootAttribute("Categories"));
            List<CategoryImportDTO> categoriesImport;
            using (var reader = new StringReader(inputXml))
            {
                categoriesImport = (List<CategoryImportDTO>)xmlSerializer.Deserialize(reader);
            }

            var categories = categoriesImport.Where(c => c.Name != null)
                .Select(c => new Category
                {
                    Name = c.Name
                }).ToList();
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count}";
        }
        //4#
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CategoryProductImportDTO>)
                , new XmlRootAttribute("CategoryProducts"));

            List<CategoryProductImportDTO> categoryProductsImport;
            using (var reader = new StringReader(inputXml))
            {
                categoryProductsImport = xmlSerializer.Deserialize(reader) as List<CategoryProductImportDTO>;
            }
            var validCategoriId = context.Categories.Select(c => c.Id).ToList();
            var validProductId = context.Products.Select(p => p.Id).ToList();

            var categoryProducts = categoryProductsImport
                .Where(cp => validCategoriId.Contains(cp.CategoryId)
                 && validProductId.Contains(cp.ProductId))
                .Select(cp => new CategoryProduct
                {
                    CategoryId = cp.CategoryId,
                    ProductId = cp.ProductId,
                })
                .ToList();

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //5#
        public static string GetProductsInRange(ProductShopContext context)
        {

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ProductsInRangeDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .ToArray();


            XmlRootAttribute xmlAttributes = new XmlRootAttribute("Products");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProductsInRangeDTO[]), xmlAttributes);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, products, namespaces);


            return sb.ToString().TrimEnd();
        }

        //6#
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new UserSoldProductDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new ProductsInRangeDTO
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToList()
                })
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Users");

            XmlSerializer serializer = new XmlSerializer(typeof(UserSoldProductDTO[]), xmlRoot);

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            StringBuilder sb = new StringBuilder();

            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, users, ns);

            return sb.ToString().TrimEnd();
        }

        //7#
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var category = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Select(cp => cp.Product.Price).Average(),
                    TotalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).Sum()
                }).ToList().OrderByDescending(c => c.Count).ThenBy(c => c.TotalRevenue);

            var categoriesByProduct = category.Select(c => new CategoriesByProductsDTO
            {
                Name = c.Name,
                Count = c.Count,
                AveragePrice = c.AveragePrice,
                TotalRevenue = c.TotalRevenue
            }).ToList();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Categories");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CategoriesByProductsDTO>), xmlRoot);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, categoriesByProduct, namespaces);

            return sb.ToString().TrimEnd();
        }

        //8#
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any() )
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new UserDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProduct = new SoldProductDTO()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.Where(ps => ps.BuyerId != null)
                        .Select(ps => new ProductDto()
                        {
                            Name = ps.Name,
                            Price = ps.Price

                        }).OrderByDescending(ps => ps.Price).ToList(),

                    }

                }).Take(10).ToList();

            UsersWithProductsDTO userDTO = new UsersWithProductsDTO()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any() && u.ProductsSold.Any(ps => ps.BuyerId != null)),
                Users = users
            };

            XmlRootAttribute rootAttribute = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(UsersWithProductsDTO), rootAttribute);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, userDTO, namespaces);
            return sb.ToString().TrimEnd();

        }
    }
}
