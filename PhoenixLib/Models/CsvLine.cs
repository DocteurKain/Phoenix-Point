namespace PhoenixLib.Models
{
    using System;
    using CsvHelper.Configuration.Attributes;
    using Tools;

    public class CsvLine
    {
        [Name("Key")]
        public String Key { get; set; }
        [Name("English")]
        public String English { get; set; }
        [Name("French")]
        public String French { get; set; }

        public CsvLine() { }

        public CsvLine(PpdfLine ppdfLine)
        {
            this.Key = ppdfLine.Key;
            this.English = ppdfLine.English.ToMultiLine();
            this.French = ppdfLine.French.ToMultiLine();
        }

        public override String ToString()
            => English.Length > 20 ? $"[{Key}] {English.Substring(0, 20)} ..." : $"[{Key}] {English}";
    }
}