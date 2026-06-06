    public static class Database
    {
        private static readonly string ConnectionString = 
        "Server=localhost;Database=klinika_weterynaryjna;" +
        "Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }

