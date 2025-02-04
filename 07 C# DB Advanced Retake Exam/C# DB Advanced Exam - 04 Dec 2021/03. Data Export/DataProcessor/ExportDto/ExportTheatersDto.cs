namespace Theatre.DataProcessor.ExportDto
{
    public class ExportTheatersDto
    {

        //    "Name": "Capitol Theatre Building",
        public string Name { get; set; } = null!;

        //"Halls": 10,
        public sbyte Halls { get; set; }

        //"TotalIncome": 860.02,
        public decimal TotalIncome { get; set; } 

        //"Tickets": [
        public ExportTicketsDto[] Tickets { get; set; } = new ExportTicketsDto[] { };

    }
}
