namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;


    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context.Producers.Find(producerId).Albums
                 .Select(a => new
                 {
                     AlbumName = a.Name,

                     ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),

                     ProducerName = a.Producer.Name,

                     a.Price,

                     Songs = a.Songs.Select(s => new
                     {
                         SongName = s.Name,

                         s.Price,

                         WriterName = s.Writer.Name
                     }).OrderByDescending(s => s.SongName).ThenBy(s => s.WriterName).ToList()



                 }).OrderByDescending(a => a.Price).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var album in albumsInfo)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");
                int song = 1;
                foreach (var s in album.Songs)
                {
                    sb.AppendLine($"---#{song++}");
                    sb.AppendLine($"---SongName: {s.SongName}");
                    sb.AppendLine($"---Price: {s.Price:f2}");
                    sb.AppendLine($"---Writer: {s.WriterName}");
                }
                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }
            return sb.ToString().TrimEnd();

        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var sonds = context.Songs.ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    Writer = s.Writer.Name,
                    Performers = s.SongPerformers.Select(sp => new
                    {
                        FulName = $"{sp.Performer.FirstName} {sp.Performer.LastName}",
                    }).OrderBy(sp => sp.FulName).ToList(),
                    Producer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                }).OrderBy(s => s.Name).ThenBy(s => s.Writer).ToList();

            StringBuilder sb= new StringBuilder();
            int song = 1;
            foreach (var s in sonds)
            {
                sb.AppendLine($"-Song #{song++}");
                sb.AppendLine($"---SongName: {s.Name}");
                sb.AppendLine($"---Writer: {s.Writer}");
                if (s.Performers.Any())
                {
                    foreach (var p in s.Performers)
                    {
                        sb.AppendLine($"---Performer: {p.FulName}");

                    }
                }
                sb.AppendLine($"---AlbumProducer: {s.Producer}");
                sb.AppendLine($"---Duration: {s.Duration}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
