namespace PhoenixLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CsvHelper;
    using Models;

    public static class CsvService
    {
        public static void CsvToPpdfPath(String csvPath, String ppdfPath)
        {
            if (!Directory.Exists(csvPath))
                return;

            if (!Directory.Exists(ppdfPath))
                Directory.CreateDirectory(ppdfPath);

            var csvFiles = Directory.EnumerateFiles(csvPath, "*.csv", SearchOption.TopDirectoryOnly);

            /*Parallel.ForEach(csvFiles, csvFile =>
            {
                var csvFilename = Path.GetFileNameWithoutExtension(csvFile);
                var ppdfFile = Path.Combine(ppdfPath, $"{csvFilename}.ppdf");

                CsvToPpdf(csvFile, ppdfFile);
            });*/

            foreach (var csvFile in csvFiles)
            {
                var csvFilename = Path.GetFileNameWithoutExtension(csvFile);
                var ppdfFile = Path.Combine(ppdfPath, $"{csvFilename}.ppdf");

                if (csvFilename == "Tutorial")
                    continue;

                CsvToPpdf(csvFile, ppdfFile);
            }
        }

        public static void CsvToPpdf(String csvFile, String ppdfFile)
        {
            if (!File.Exists(csvFile))
                return;

            using (var reader = new StreamReader(csvFile))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CsvLine>();

                var list = new List<PpdfLine>();

                foreach (var record in records)
                    list.Add(new PpdfLine(record.Key, record.English, record.French));

                PpdfService.SaveTo(list, ppdfFile);
            }
        }
    }
}