using System;

namespace DataIntegrationChallenge.API.Helpers {
    public class CsvException : Exception {
        public CsvException() { }
        public CsvException(string message) : base(message) { }
    }
}