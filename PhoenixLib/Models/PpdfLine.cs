namespace PhoenixLib.Models
{
    using System;
    using Tools;

    public class PpdfLine
    {
        public String Key { get; set; }
        public String English { get; set; }
        public String French { get; set; }

        public PpdfLine() { }

        public PpdfLine(String key, String english, String french)
        {
            Key = key;
            English = english.ToOneLine();
            French = french.ToOneLine();
        }
    }
}