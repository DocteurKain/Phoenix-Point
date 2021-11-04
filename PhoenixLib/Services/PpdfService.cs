namespace PhoenixLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CsvHelper;
    using Models;

    public static class PpdfService
    {
        public static void PpdfToCsvPath(String ppdfPath, String csvPath)
        {
            if (!Directory.Exists(ppdfPath))
                return;

            if (!Directory.Exists(csvPath))
                Directory.CreateDirectory(csvPath);

            var ppdfFiles = Directory.EnumerateFiles(ppdfPath, "*.ppdf", SearchOption.TopDirectoryOnly);

            /*Parallel.ForEach(ppdfFiles, ppdfFile =>
            {
                var ppdfFilename = Path.GetFileNameWithoutExtension(ppdfFile);
                var csvFile = Path.Combine(csvPath, $"{ppdfFilename}.csv");

                PpdfToCsv(ppdfFile, csvFile);
            });*/

            foreach (var ppdfFile in ppdfFiles)
            {
                var ppdfFilename = Path.GetFileNameWithoutExtension(ppdfFile);
                var csvFile = Path.Combine(csvPath, $"{ppdfFilename}.csv");

                PpdfToCsv(ppdfFile, csvFile);
            }
        }

        public static void PpdfToCsv(String ppdfFile, String csvFile)
        {
            if (!File.Exists(ppdfFile))
                return;

            var ppdfList = LoadFrom(ppdfFile);

            if (ppdfList == null)
                return;

            var csvList = ppdfList.Select(a => new CsvLine(a)).ToList();

            using (var writer = new StreamWriter(csvFile))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(csvList);
            }
        }

        public static List<PpdfLine> LoadFrom(String ppdfFile)
        {
            if (!File.Exists(ppdfFile))
                return null;

            var list = new List<PpdfLine>();

            using (var reader = new StreamReader(ppdfFile, Encoding.UTF8))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    var match = Regex.Match(line, @"^# (?<key>.+)$");

                    if (!match.Success) continue;

                    var key = match.Groups["key"].Value;
                    var english = reader.ReadLine();
                    var french = reader.ReadLine();

                    list.Add(new PpdfLine(key, english, french));
                }
            }

            return list;
        }

        public static void SaveTo(List<PpdfLine> list, String ppdfFile)
        {
            if (list == null)
                return;

            var ppdfPath = Path.GetDirectoryName(ppdfFile);

            if (!Directory.Exists(ppdfPath))
                Directory.CreateDirectory(ppdfPath);

            var sb = new StringBuilder();

            foreach (var l in list)
            {
                sb.AppendLine($"# {l.Key}");
                sb.AppendLine(l.English);
                sb.AppendLine(l.French);
            }

            File.WriteAllText(ppdfFile, sb.ToString(), Encoding.UTF8);
        }
    }
}