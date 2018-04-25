using System.Text;

namespace DataIntegrationChallenge.Api.Tests {
    public static class TestCsv {

        public static string Content => CreateCsv();

        public static string[] DifferentColumnOrderHeader => new[] {"website", "name", "addressZip"};

        public static string MergeCsv => CreateMergeCsv();
        
        public static string[] MissingNameColumnHeader => new[] {"website", "addressZip"};
        
        public static string[] MissingWebsiteColumnHeader => new[] {"name", "addressZip"};

        private static string CreateCsv() {
            var sb = new StringBuilder();
            sb.AppendLine("name;addressZip;website")
                .AppendLine("company;1234;url");
            return sb.ToString();
        }

        private static string CreateMergeCsv() {
            var newContent = Content.Replace("url", "new url");
            var sb = new StringBuilder(newContent);
            sb.AppendLine("new company;9876;new company url");
            return sb.ToString();
        }




        
    }
}