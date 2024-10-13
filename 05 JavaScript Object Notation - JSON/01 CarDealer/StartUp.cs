using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();


            //9#
            //string inputSuppliers = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, inputSuppliers));

            //10#
            //string inputParts = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, inputParts));

            //11#
            //string inputCarsJson = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(context, inputCarsJson));

            //12# 
            //string inputCustomers = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, inputCustomers));

            //13#
            //string inputSales = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(context,inputSales));

            //14#
            //Console.WriteLine(GetOrderedCustomers(context));

            //15#
            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //16#
            //Console.WriteLine(GetLocalSuppliers(context));

            //17#
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //18#
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            //19#
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }
        //9#
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        //10#
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);
            var existingSupplierIds = context.Suppliers.Select(s => s.Id).ToList();
            var partsSupplierIdNottNull = parts
                .Where(p => p.SupplierId != null && existingSupplierIds
                .Contains(p.SupplierId)).ToList();

            context.Parts.AddRange(partsSupplierIdNottNull);
            context.SaveChanges();

            return $"Successfully imported {partsSupplierIdNottNull.Count}.";
        }
        //11#
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDTO = JsonConvert.DeserializeObject<List<CarDTO>>(inputJson);
            var cars = new HashSet<Car>();
            var partCars = new HashSet<PartCar>();

            foreach (var carDto in carsDTO)
            {
                var car = new Car();

                car.Make = carDto.Make;
                car.Model = carDto.Model;
                car.TraveledDistance = carDto.TraveledDistance;
                cars.Add(car);

                foreach (var partId in carDto.PartsId)
                {
                    var partCar = new PartCar();
                    partCar.Car = car;
                    partCar.PartId = partId;
                    partCars.Add(partCar);
                }
            }

            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partCars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}.";
        }
        //12#
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<HashSet<Customer>>(inputJson);
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}.";
        }
        //13#
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<HashSet<Sale>>(inputJson);
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}.";
        }
        //14#
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customaers = context.Customers

                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    c.IsYoungDriver
                }).ToList();
            var setings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            var result = JsonConvert.SerializeObject(customaers, setings);
            return result;
        }
        //15#
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var carsToyotd = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TraveledDistance
                }
                ).ToList();
            var setung = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };

            var resultCarsToyota = JsonConvert.SerializeObject(carsToyotd, setung);
            return resultCarsToyota;
        }
        //16#
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                }).ToArray();


            var seting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            };

            var result = JsonConvert.SerializeObject(suppliers, seting);
            return result;
        }
        //17#
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance

                    },
                    parts = c.PartsCars.Select(p => new
                    {
                        p.Part.Name,
                        Price = $"{p.Part.Price:f2}"
                    })
                }).ToArray();


            var seting = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };

            var result = JsonConvert.SerializeObject(cars, seting);
            return result;

        }
        //18#
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.SelectMany(s => s.Car.PartsCars.Select(pc => pc.Part.Price)).Sum()
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToArray();

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var result = JsonConvert.SerializeObject(customers, settings);

            return result;
        }
        //19#
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = $"{s.Discount:f2}",
                    price = $"{s.Car.PartsCars.Select(pc => pc.Part.Price).Sum():f2}",
                    priceWithDiscount = $"{s.Car.PartsCars.Select(pc => pc.Part.Price).Sum()* (1 - s.Discount / 100):f2}"
                })
                .ToArray();

            var setting = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };

            var result = JsonConvert.SerializeObject(sales, setting);

            return result;

        }
    }
}