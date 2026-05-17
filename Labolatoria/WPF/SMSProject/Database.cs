using System;
using Microsoft.Data.SqlClient;

public static class Database
{
    private static readonly string ConnectionString = "Server=localhost;Database=SMSDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}