using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using var context = new CarDealerContext();
            ////9#
            //string inputSuppliers = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, inputSuppliers));

            ////10#
            //string inputParts = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, inputParts));

            ////11#
            //string inputCars = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, inputCars));

            //    //12#
            //    string inputCustomers = File.ReadAllText("../../../Datasets/customers.xml");
            //    Console.WriteLine(ImportCustomers(context,inputCustomers));

            ////13#
            //string inputSales = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, inputSales));

            ////14#
            //Console.WriteLine(GetCarsWithDistance(context));

            ////15
            //Console.WriteLine(GetCarsFromMakeBmw(context));

            ////16#
            //Console.WriteLine(GetLocalSuppliers(context));

            ////17#
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            ////18#
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            //19#
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        //9#
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SupplierImportDTO[])
                , new XmlRootAttribute("Suppliers"));

            SupplierImportDTO[]? suppliersDTO;

            using var reder = new StringReader(inputXml);

            suppliersDTO = xmlSerializer.Deserialize(reder) as SupplierImportDTO[];

            var suppliers = suppliersDTO?.Select(s => new Supplier
            {
                Name = s.Name,
                IsImporter = s.IsImporter
            }).ToList();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers?.Count}";
        }

        //10#

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PartImortDTO[]), new XmlRootAttribute("Parts"));

            PartImortDTO[]? partsDTO;

            using var reader = new StringReader(inputXml);

            partsDTO = serializer.Deserialize(reader) as PartImortDTO[];

            var validSupplierId = context.Suppliers.Select(s => s.Id).ToList();

            var parts = partsDTO?.Where(p => validSupplierId.Contains(p.SupplierId))
                .Select(p => new Part
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = p.SupplierId
                })
                .ToList();
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}";
        }

        //11#
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CarImportDTO>), new XmlRootAttribute("Cars"));

            List<CarImportDTO>? carsDTO;

            using StringReader reader = new StringReader(inputXml);
            carsDTO = serializer.Deserialize(reader) as List<CarImportDTO>;

            List<Car> cars = new List<Car>();

            List<PartCar> partsCars = new List<PartCar>();

            List<int> validPartsId = context.Parts.Select(p => p.Id).ToList();

            foreach (var carDTO in carsDTO)
            {
                Car car = new Car()
                {
                    Make = carDTO.Make,
                    Model = carDTO.Model,
                    TraveledDistance = carDTO.TraveledDistance
                };
                cars.Add(car);

                foreach (var part in carDTO.Parts.Select(p => p.Id).Where(id => validPartsId.Contains(id)).Distinct())
                {
                    PartCar partCar = new PartCar()
                    {
                        PartId = part,
                        Car = car
                    };
                    partsCars.Add(partCar);
                }
            }

            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partsCars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //12#
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomerImportDTO>), new XmlRootAttribute("Customers"));
            List<CustomerImportDTO>? customersDTO;
            using StringReader reader = new StringReader(inputXml);
            customersDTO = serializer.Deserialize(reader) as List<CustomerImportDTO>;
            var customers = customersDTO.Select(c => new Customer
            {
                Name = c.Name,
                BirthDate = c.BirthDate,
                IsYoungDriver = c.IsYoungDriver
            }).ToList();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        //13#
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serilaizer = new XmlSerializer(typeof(List<SaleImportDTO>), new XmlRootAttribute("Sales"));

            List<SaleImportDTO>? salesDTO;
            using var reade = new StringReader(inputXml);
            salesDTO = serilaizer.Deserialize(reade) as List<SaleImportDTO>;

            List<int> validCarID = context.Cars.Select(c => c.Id).ToList();
            List<int> validCustomerId = context.Customers.Select(c => c.Id).ToList();

            List<Sale> sales = salesDTO
                .Where(s => validCarID.Contains(s.CarId))
                .Select(s => new Sale
                {
                    CarId = s.CarId,
                    CustomerId = s.CustomerId,
                    Discount = s.Discount
                }).ToList();
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}";
        }

        //14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carsWithDistance = context.Cars.Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make).ThenBy(c => c.Model)
                .Select(c => new CarExportDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                }).Take(10).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CarExportDTO>), new XmlRootAttribute("cars"));

            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add("", "");

            StringBuilder sb = new StringBuilder();

            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, carsWithDistance, xmlns);
            return sb.ToString().TrimEnd();
        }

        //15#
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var listBmv = context.Cars.Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new CarsFromMakeBmwDTO
                {
                    Id = c.Id,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CarsFromMakeBmwDTO>), new XmlRootAttribute("cars"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true, // Премахва декларацията
                Indent = true,             // Форматира XML с отстъп за по-добра четимост (по избор)
            };
            // using StringWriter writer=new StringWriter(sb);
            using XmlWriter writer = XmlWriter.Create(sb, settings);

            serializer.Serialize(writer, listBmv, namespaces);

            return sb.ToString().TrimEnd();
        }

        //16#
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new LocalSuppliersDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<LocalSuppliersDTO>), new XmlRootAttribute("suppliers"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using XmlWriter writer = XmlWriter.Create(sb, settings);

            serializer.Serialize(writer, suppliers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //17#
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Select(c => new CarsWithTheirListOfPartsDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars.OrderByDescending(pc => pc.Part.Price)
                    .Select(pc => new PartDTO
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price,
                    }).ToList()
                })
                .Take(5)
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CarsWithTheirListOfPartsDTO>)
                , new XmlRootAttribute("cars"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = false,
                Indent = true,
            };

            using var writer = XmlWriter.Create(sb, settings);

            serializer.Serialize(writer, cars, namespaces);
            return sb.ToString().TrimEnd();
        }

        //18#
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.IsYoungDriver
                    ? c.Sales.SelectMany(s => s.Car.PartsCars.Select(pc => Math.Round(pc.Part.Price * 0.95m, 2))).Sum()
                    : c.Sales.SelectMany(s => s.Car.PartsCars.Select(pc => Math.Round(pc.Part.Price, 2))).Sum()
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToList();
            List<CustomerDTO> customerDTOs = new List<CustomerDTO>();

            foreach (var c in customers)
            {
                CustomerDTO customerDTO = new CustomerDTO()
                {
                    FullName = c.Name,
                    BoughtCars = c.BoughtCars,
                    SpentMoney = c.SpentMoney
                };
                customerDTOs.Add(customerDTO);

            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomerDTO>), new XmlRootAttribute("customers"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = false,
                Indent = true,
            };
            using XmlWriter writer = XmlWriter.Create(sb, settings);

            serializer.Serialize(writer, customerDTOs, namespaces);

            return sb.ToString().TrimEnd();
        }

        //19#
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SaleExportDTO()
                {
                    Car = new CarDTO()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance,
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = Math.Round((double)(s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - (s.Discount / 100))), 4)
                })
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(SaleExportDTO[]), new XmlRootAttribute("sales"));

            XmlSerializerNamespaces sns = new XmlSerializerNamespaces();

            sns.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, sales, sns);

            return sb.ToString().TrimEnd();
        }
    }
}