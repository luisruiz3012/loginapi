using RolesLibrary.Database;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using RolesLibrary.Models;

namespace RolesLibrary
{
    public class Methods
    {
        // creating internal database reference
        private readonly DB _db;

        public Methods()
        {
            // Initialization database
            _db = new DB();
        }

        // Get method
        public dynamic Get()
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre FROM roles AS r WITH(NOLOCK) ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Rol> roles = new List<Rol>();

                while (reader.Read())
                {
                    Rol rol = new Rol { 
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString()
                    };

                    roles.Add(rol);
                }

                if (roles.Count > 0)
                {
                    conn.Close();
                    return roles;
                }

                conn.Close();
                return null;
            }
        }

        public dynamic GetById(int id)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre FROM roles AS r WITH(NOLOCK) WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Rol rol = new Rol
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                    };

                    conn.Close();
                    return rol;
                }

                conn.Close();
                return null;
            }
        }

        public dynamic Create(Rol rol)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO roles (nombre) VALUES (@Nombre) ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", rol.Nombre);
                cmd.ExecuteNonQuery();

                conn.Close();
                conn.Open();

                query = "SELECT id, nombre FROM roles r WHERE nombre = @Nombre ".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", rol.Nombre);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    conn.Close();
                    return null;
                }

                conn.Close();
                return new { message = "Created successfully" };
            }
        }

        public dynamic Update(int id, Rol rol)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre FROM roles AS r WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                conn.Close();
                conn.Open();

                query = "UPDATE roles SET nombre = @Nombre ".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", rol.Nombre);
                cmd.ExecuteNonQuery();

                conn.Close();
                return new { message = "Updated successfully" };
            }
        }

        public dynamic Delete(int id)
        {
            using (var conn = _db.GetConnection() ) {
                conn.Open();

                string query = "SELECT id, nombre FROM roles AS r WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    conn.Close();
                    return null;
                }

                conn.Close();
                conn.Open();

                query = "DELETE FROM roles WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();
                return new { message = "Deleted successfully!" };
            }
        }
    }
}
