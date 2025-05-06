
namespace Core.Utilities
{
    public class EnvVariableService
    {
        public static string GetJwtSecretKey()
        {
            string? JwtSecretKey = GetVariable("JWT_SECRET_KEY");

            if (string.IsNullOrEmpty(JwtSecretKey))
            {
                throw new ArgumentNullException(nameof(JwtSecretKey), "JWT secret key is not set.");
            }

            return JwtSecretKey;
        }
        public static string GetJwtAudience()
        {
            string? uiDomainUrl = GetVariable("DOMAIN_URL");
            string? uiPort = GetVariable("REACT_APP_PORT");

            if (uiDomainUrl == null)
                throw new ArgumentNullException(nameof(uiDomainUrl), "Front end domain url is not set.");

            if (uiPort == null)
                throw new ArgumentNullException(nameof(uiPort), "Front end port is not set.");

            return $"http://{uiDomainUrl}:{uiPort}";
        }

        public static string GetJwtIssuer()
        {
            string? serverPort = GetVariable("HTTP_SERVER_PORT");

            return $"http://localhost:{serverPort}";
        }

        public static string GetConnectionString()
        {
            string? dbConnString = GetVariable("ConnectionString");

            if (dbConnString == null)
            {
                throw new ArgumentNullException("No connection string to the DB.");
            }

            return dbConnString;
        }

        public static void SetConnectionString(string? dbConnStringTemplate)
        {
            string dbConnString = string.Empty;

            string? server = GetVariable("SQL_SERVER_HOST");
            string? database = GetVariable("SQL_DATABASE");
            string? user = GetVariable("SQL_USER");
            string? password = GetVariable("SA_PASSWORD");

            dbConnString = string.Format(dbConnStringTemplate, server, database, user, password);

            Environment.SetEnvironmentVariable("ConnectionString", dbConnString);
        }

        public static string GetCorsOriginsUrl()
        {
            string uiDomainUrl = GetVariable("DOMAIN_URL")!;
            string uiPort = GetVariable("REACT_APP_PORT")!;

            return $"http://{uiDomainUrl}:{uiPort}";
        }

        private static string? GetVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }

        public static void LoadEnvironmentVariables(Dictionary<string, string> envVars)
        {
            foreach (var keyValPair in envVars)
            {
                Environment.SetEnvironmentVariable(keyValPair.Key, keyValPair.Value);
            }
        }
    }
}
