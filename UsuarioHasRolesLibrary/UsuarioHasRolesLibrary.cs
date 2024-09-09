using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using UsuarioHasRolesLibrary.Database;
using UsuarioHasRolesLibrary.Models;

namespace UsuarioHasRolesLibrary
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

                string query = "SELECT id, usuario_id, roles_id FROM usuario_has_roles AS uhr WITH(NOLOCK) ".Replace("%", "").Replace("'", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<UsuarioHasRole> uhr = new List<UsuarioHasRole>();

                while (reader.Read())
                {
                    UsuarioHasRole user_has_roles = new UsuarioHasRole
                    {
                        Id = (int)reader["Id"],
                        Usuario_Id = (int)reader["Usuario_Id"],
                        Roles_Id = (int)reader["Roles_Id"],
                    };

                    uhr.Add(user_has_roles);
                }

                if (uhr.Count > 0)
                {
                    conn.Close();
                    return uhr;
                }

                conn.Close();
                return null;
            }
        }

        public dynamic GetById(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, usuario_id, roles_id FROM usuario_has_roles AS uhr WITH(NOLOCK) WHERE id = @Id ".Replace("%", "").Replace("'", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.Read())
                {
                    UsuarioHasRole user_has_roles = new UsuarioHasRole
                    {
                        Id = (int)reader["Id"],
                        Usuario_Id = (int)reader["Usuario_Id"],
                        Roles_Id = (int)reader["Roles_Id"],
                    };

                    conn.Close();
                    return user_has_roles;
                }

                conn.Close();
                return null;
            }
        }

        public dynamic Create(UsuarioHasRole uhr)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO usuario_has_roles (usuario_id, roles_id) VALUES (@Usuario_id, @Roles_Id) ".Replace("%", "").Replace("'", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(@query, conn);
                cmd.Parameters.AddWithValue("@Usuario_Id", uhr.Usuario_Id);
                cmd.Parameters.AddWithValue("@Roles_Id", uhr.Roles_Id);
                cmd.ExecuteNonQuery();

                conn.Close();
                conn.Open();

                query = "SELECT id, usuario_id, roles_id FROM usuario_has_roles as uhr WITH(NOLOCK) WHERE usuario_id = @Usuario_id AND roles_id = @Roles_id".Replace("%", "").Replace("'", "").Replace(" --", "");
                cmd = new SqlCommand(@query, conn);
                cmd.Parameters.AddWithValue("@Usuario_Id", uhr.Usuario_Id);
                cmd.Parameters.AddWithValue("@Roles_Id", uhr.Roles_Id);
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

        public dynamic Update(int id,  UsuarioHasRole uhr)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, usuario_id, roles_id FROM usuario_has_roles WHERE id = @Id".Replace("%", "").Replace("'", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(@query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    conn.Close();
                    return null;
                }

                conn.Close();
                conn.Open();

                query = "UPDATE usuario_has_roles SET usuario_id = @Usuario_id, roles_id = @Roles_id WHERE id = @Id".Replace("%", "").Replace("'", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario_id", uhr.Usuario_Id);
                cmd.Parameters.AddWithValue("@Roles_id", uhr.Roles_Id);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();
                return new { message = "Updated successfully" };
            }
        }

        public dynamic Delete(int id)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, usuario_id, roles_id FROM usuario_has_roles WHERE id = @Id".Replace("%", "").Replace("'", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(@query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    conn.Close();
                    return null;
                }

                conn.Close();
                conn.Open();

                query = "DELETE FROM usuario_has_roles WHERE id = @Id".Replace("%", "").Replace("'", "").Replace(" --", "");
                cmd = new SqlCommand(@query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();
                return new { message = "Deleted successfully" };
            }
        }
    }
}
