﻿
namespace Theatre
{
    using System;
    using System.IO;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new TheatreContext();

            ResetDatabase(context, shouldDropDatabase: true);

            var projectDir = GetProjectDirectory();
            Console.WriteLine(projectDir);

            //ImportEntities(context, projectDir + @"Datasets/", projectDir + @"ImportResults/");

            //ExportEntities(context, projectDir + @"ExportResults/");

            using (var transaction = context.Database.BeginTransaction())
            {
                transaction.Rollback();
            }

        }

        private static void ImportEntities(TheatreContext context, string baseDir, string exportDir)
        {
            var theatersAndTickets =
              DataProcessor.Deserializer.ImportPlays(context,
                  File.ReadAllText(baseDir + "plays.xml"));
            PrintAndExportEntityToFile(theatersAndTickets, exportDir + "Actual Result - ImportPlays.txt");

            var casts = DataProcessor.Deserializer.ImportCasts(context,
               File.ReadAllText(baseDir + "casts.xml"));
            PrintAndExportEntityToFile(casts, exportDir + "Actual Result - ImportCasts.txt");

            var plays =
                DataProcessor.Deserializer.ImportTtheatersTickets(context,
                    File.ReadAllText(baseDir + "theatres-and-tickets.json"));
            PrintAndExportEntityToFile(plays, exportDir + "Actual Result - ImportTheatresTickets.txt");

        }

        private static void ExportEntities(TheatreContext context, string exportDir)
        {
            var exportTheaters = DataProcessor.Serializer.ExportTheatres(context, 6);
            Console.WriteLine(exportTheaters);
            File.WriteAllText(exportDir + "Actual Result - ExportTheatres.json", exportTheaters);

            var exportActors = DataProcessor.Serializer.ExportPlays(context, 7.5);
            Console.WriteLine(exportActors);
            File.WriteAllText(exportDir + "Actual Result - ExportActors.xml", exportActors);
        }

        private static void ResetDatabase(TheatreContext context, bool shouldDropDatabase = false)
        {
            if (shouldDropDatabase)
            {
                context.Database.EnsureDeleted();
            }

            if (context.Database.EnsureCreated())
            {
                return;
            }

            var disableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlRaw(disableIntegrityChecksQuery);

            var deleteRowsQuery = "EXEC sp_MSforeachtable @command1='SET QUOTED_IDENTIFIER ON;DELETE FROM ?'";
            context.Database.ExecuteSqlRaw(deleteRowsQuery);

            var enableIntegrityChecksQuery =
                "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlRaw(enableIntegrityChecksQuery);

            var reseedQuery =
                "EXEC sp_MSforeachtable @command1='IF OBJECT_ID(''?'') IN (SELECT OBJECT_ID FROM SYS.IDENTITY_COLUMNS) DBCC CHECKIDENT(''?'', RESEED, 0)'";
            context.Database.ExecuteSqlRaw(reseedQuery);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static string GetProjectDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryName = Path.GetFileName(currentDirectory);
            var relativePath = directoryName.StartsWith("net6.0") ? @"../../../" : string.Empty;

            return relativePath;
        }
    }
}
