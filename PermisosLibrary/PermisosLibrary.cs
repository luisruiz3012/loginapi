using Microsoft.Data.SqlClient;
using PermisosLibrary.Database;
using PermisosLibrary.Models;
using System.Collections;
using System.Collections.Generic;

namespace PermisosLibrary
{
    public class Methods
    {
        private readonly DB _db;

        public Methods()
        {
            _db = new DB();
        }

        public dynamic Get()
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre, estado_id, roles_id FROM permisos AS p WITH(NOLOCK) ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Permiso> permisos = new List<Permiso>();

                while (reader.Read())
                {
                    Permiso permiso = new Permiso
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Estado_id = (int)reader["Estado_id"],
                        Roles_id = (int)reader["Roles_id"]
                    };

                    permisos.Add(permiso);
                }

                if (permisos.Count > 0)
                {
                    conn.Close();
                    return permisos;
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

                string query = "SELECT id, nombre, estado_id, roles_id FROM permisos AS p WITH(NOLOCK) WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Permiso permiso = new Permiso
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Estado_id = (int)reader["Estado_id"],
                        Roles_id = (int)reader["Roles_id"]
                    };

                    conn.Close();
                    return permiso;
                }

                conn.Close();
                return null;
            }
        }

        public dynamic Create(Permiso permiso)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO permisos (nombre, estado_id, roles_id) VALUES (@Nombre, @Estado_id, @Roles_id)".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", permiso.Nombre);
                cmd.Parameters.AddWithValue("@Estado_id", permiso.Estado_id);
                cmd.Parameters.AddWithValue("@Roles_id", permiso.Roles_id);
                cmd.ExecuteNonQuery();

                conn.Close();
                conn.Open();

                query = "SELECT nombre, estado_id, roles_id FROM permisos AS p WITH(NOLOCK) WHERE nombre = @Nombre AND estado_id = @Estado_id AND roles_id = @Roles_id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", permiso.Nombre);
                cmd.Parameters.AddWithValue("@Estado_id", permiso.Estado_id);
                cmd.Parameters.AddWithValue("@Roles_id", permiso.Roles_id);
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

        public dynamic Update(int id, Permiso permiso)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT nombre, estado_id, roles_id FROM permisos AS p WITH(NOLOCK) WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
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

                query = "UPDATE permisos SET nombre = @Nombre, estado_id = @Estado_id, roles_id = @Roles_id WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", permiso.Nombre);
                cmd.Parameters.AddWithValue("@Estado_id", permiso.Estado_id);
                cmd.Parameters.AddWithValue("@Roles_id", permiso.Roles_id);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();
                return new { message = "Updated successfully!" };
            }
        }

        public dynamic Delete(int id)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre FROM permisos AS p WITH(NOLOCK) WHERE id = @Id".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if(!reader.Read())
                {
                    conn.Close();
                    return null;
                }

                conn.Close();
                conn.Open();

                query = "DELETE FROM permisos WHERE id = @Id".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();
                return new { message = "Deleted successfully!" };
            }
        }
    }
}
