using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Xml.Serialization;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var guides = context.Guides.Where(g => g.Language == Language.Spanish)

                .Select(g => new
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides

                    .Select(tp => new
                    {
                        Name = tp.TourPackage.PackageName,
                        Description = tp.TourPackage.Description,
                        Price = tp.TourPackage.Price

                    })
                    .OrderByDescending(tp => tp.Price)
                    .ThenBy(tp => tp.Name)
                    .ToList()

                })
                .OrderByDescending(g => g.TourPackages.Count)
                .ThenBy(g => g.FullName)
                .ToList();
            List<GuideExportDTO> guideExports = new List<GuideExportDTO>();

            foreach (var g in guides)
            {
                var guide = new GuideExportDTO()
                {
                    FullName = g.FullName,

                };
                foreach (var tp in g.TourPackages)
                {
                    var tour = new TourPackageExportDTO()
                    {
                        Name = tp.Name,
                        Description = tp.Description,
                        Price = tp.Price
                    };
                    guide.TourPackages.Add(tour);
                }
                guideExports.Add(guide);

            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<GuideExportDTO>), new XmlRootAttribute("Guides"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using StringWriter sw = new StringWriter(sb);

            serializer.Serialize(sw, guideExports, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customers = context.Customers
                .Where(c => c.Bookings
                .Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .Select(c=> new
                {
                    c.FullName,
                    c.PhoneNumber,
                    Bookings=c.Bookings
                    .Where(b=>b.TourPackage.PackageName== "Horse Riding Tour")
                    .Select(b=> new
                    {
                        TourPackageName=b.TourPackage.PackageName,
                        Date=b.BookingDate.ToString("yyyy-MM-dd")
                    })
                    .ToList()
                })
                .OrderByDescending(c=>c.Bookings.Count)
                .ThenBy(c=>c.FullName)
                .ToList();

            return JsonConvert.SerializeObject(customers,Formatting.Indented);
        }
    }
}
