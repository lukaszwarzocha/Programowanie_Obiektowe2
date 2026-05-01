using Labolatorium5.Models;
using Labolatorium5.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Labolatorium5.Data
{
    internal class ContactRepository
    {
        private readonly string _connectionString;
        public ContactRepository(string connectionString) { 
            _connectionString = connectionString; 
        }

        // CREATE
        public int Add(Contact c) { 
            const string sql = @" INSERT INTO dbo.Contacts (FirstName, LastName, Phone, Email) VALUES (@fn, @ln, @ph, @em); SELECT CAST(SCOPE_IDENTITY() AS int);";

         try { 
                using SqlConnection conn = new SqlConnection(_connectionString); 
                conn.Open();
                using SqlCommand cmd = new SqlCommand(sql, conn); 
                cmd.Parameters.AddWithValue("@fn", c.FirstName); 
                cmd.Parameters.AddWithValue("@ln", c.LastName); 
                cmd.Parameters.AddWithValue("@ph", (object?) c.Phone ?? DBNull.Value); 
                cmd.Parameters.AddWithValue("@em", (object?) c.Email ?? DBNull.Value);
                int newId = (int)cmd.ExecuteScalar(); 
                Logger.Info($"Add Contact OK (Id={newId}, {c.FirstName} {c.LastName})"); 
                return newId; 
            } catch (Exception ex) { 
                Logger.Error($"Add Contact FAILED: {ex.Message}"); 
                throw; 
            }
        }

        //READ: all
        
        public List<Contact> GetAll()
        {
            const string sql = @" SELECT Id, FirstName, LastName, Phone, Email FROM dbo.Contacts ORDER BY Id;";
            var result = new List<Contact>();
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString); conn.Open();
                using SqlCommand cmd = new SqlCommand(sql, conn);
                using SqlDataReader r = cmd.ExecuteReader();

                while (r.Read()) { result.Add(new Contact { Id = r.GetInt32(0), FirstName = r.GetString(1), LastName = r.GetString(2), Phone = r.IsDBNull(3) ? null : r.GetString(3), Email = r.IsDBNull(4) ? null : r.GetString(4) }); }
                Logger.Info($"GetAll OK (count={result.Count})"); 
                return result;
            }
            catch (Exception ex) { 
                Logger.Error($"GetAll FAILED: {ex.Message}"); 
                throw; 
            }
        }

        //SEARCH: by last name (LIKE)
        
        public List<Contact> SearchByLastName(string lastNamePart)
        {
            const string sql = @" SELECT Id, FirstName, LastName, Phone, Email FROM dbo.Contacts WHERE LastName LIKE @ln ORDER BY LastName, FirstName;";
            var result = new List<Contact>();
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString); 
                conn.Open();
                using SqlCommand cmd = new SqlCommand(sql, conn); cmd.Parameters.AddWithValue("@ln", "%" + lastNamePart + "%");
                using SqlDataReader r = cmd.ExecuteReader(); while (r.Read()) { result.Add(new Contact { Id = r.GetInt32(0), FirstName = r.GetString(1), LastName = r.GetString(2), Phone = r.IsDBNull(3) ? null : r.GetString(3), Email = r.IsDBNull(4) ? null : r.GetString(4) }); }
                Logger.Info($"SearchByLastName OK (q='{lastNamePart}', count={result.Count})"); 
                return result;
            }
            catch (Exception ex) { 
                Logger.Error($"SearchByLastName FAILED: {ex.Message}"); 
                throw; 
            }
        }

        // UPDATE
        public bool Update(Contact c) { 
            const string sql = @" UPDATE dbo.Contacts SET FirstName=@fn, LastName=@ln, Phone=@ph, Email=@em WHERE Id=@id;";
         try 
            { 
                using SqlConnection conn = new SqlConnection(_connectionString); 
                conn.Open();
         using SqlCommand cmd = new SqlCommand(sql, conn); 
                cmd.Parameters.AddWithValue("@fn", c.FirstName); 
                cmd.Parameters.AddWithValue("@ln", c.LastName);
                cmd.Parameters.AddWithValue("@ph", (object?) c.Phone ?? DBNull.Value); 
                cmd.Parameters.AddWithValue("@em", (object?) c.Email ?? DBNull.Value); 
                cmd.Parameters.AddWithValue("@id", c.Id);
         int rows = cmd.ExecuteNonQuery(); 
                Logger.Info($"Update Contact {(rows > 0 ? "OK" : "NOT FOUND")} (Id={c.Id})"); return rows > 0; } 
            catch (Exception ex) { 
                Logger.Error($"Update Contact FAILED: {ex.Message}"); 
                throw; 
            }
        }

        // DELETE
        public bool Delete(int id) { 
            const string sql = "DELETE FROM dbo.Contacts WHERE Id=@id;";
         try 
            { 
                using SqlConnection conn = new SqlConnection(_connectionString); 
                conn.Open();
         using SqlCommand cmd = new SqlCommand(sql, conn); 
                cmd.Parameters.AddWithValue("@id", id);
         int rows = cmd.ExecuteNonQuery(); 
                Logger.Info($"Delete Contact {
                    (rows > 0 ? "OK" : "NOT FOUND")
                    } (Id={id})"); 
                return rows > 0; } 
            catch (Exception ex) { 
                Logger.Error($"Delete Contact FAILED: {ex.Message}"); 
                throw; 
            } 
        }

         // TRANSACTION: bulk insert (masowe wstawianie)
         public int BulkInsert(List<Contact> contacts) { 
            const string sql = @" INSERT INTO dbo.Contacts (FirstName, LastName, Phone, Email) VALUES (@fn, @ln, @ph, @em);";
         if (contacts == null || contacts.Count == 0) return 0;
            using SqlConnection conn = new SqlConnection(_connectionString); conn.Open();
            using SqlTransaction tx = conn.BeginTransaction();
            try
            {
                int inserted = 0;
                foreach (var c in contacts)
                {
                    using SqlCommand cmd = new SqlCommand(sql, conn, tx); cmd.Parameters.AddWithValue("@fn", c.FirstName); cmd.Parameters.AddWithValue("@ln", c.LastName);
                    cmd.Parameters.AddWithValue("@ph", (object?)c.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@em", (object?)c.Email ?? DBNull.Value);
                    inserted += cmd.ExecuteNonQuery();
                }
                tx.Commit(); Logger.Info($"BulkInsert OK (count={inserted})"); return inserted;
            }
            catch (Exception ex)
            {
                tx.Rollback(); Logger.Error($"BulkInsert FAILED -> ROLLBACK: {ex.Message}");
                throw;
            }
        }
    }
}
