using System;
using System.Collections.Generic;
using System.Linq;
using DataIntegrationChallenge.API.Data;

namespace DataIntegrationChallenge.API.Helpers {
    public static class CsvReader {
        private const char LineEnding = '\n';
        private const char Separator = ';';
        private const string Name = "name";
        private const string Zip = "addresszip";
        private const string Website = "website";

        public static IEnumerable<CompanyDto> Read(string content) {
            var lines = content.Split(new[] {LineEnding}, StringSplitOptions.RemoveEmptyEntries);
            var headerLine = lines[0].ToLower().Split(new[] {Separator}, StringSplitOptions.RemoveEmptyEntries);
            var nameCol = FindColumnIndex(headerLine, Name);
            var zipCol = FindColumnIndex(headerLine, Zip);
            var websiteCol = FindColumnIndex(headerLine, Website);

            for (var i = 1; i < lines.Length; i++) {
                var line = lines[i].Split(Separator);
                yield return new CompanyDto {
                    Name = line[nameCol],
                    AddressZip = line[zipCol],
                    Website = line[websiteCol]
                };
            }
        }

        internal static int FindColumnIndex(string[] headercolumns, string columnName) {
            var index =  Array.IndexOf(headercolumns, columnName);
            if (index == -1) {
                throw new CsvException($"Missing column '{columnName}'");
            }

            return index;
        }
    }
}