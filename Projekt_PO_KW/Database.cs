using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_PO_KW
{
    public static class Database
    {
        private static readonly string ConnectionString = "Server=localhost;Database=klinika_weterynaryjna;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
