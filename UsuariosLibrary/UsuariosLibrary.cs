using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using UsuariosLibrary.Database;
using UsuariosLibrary.Models;

namespace UsuariosLibrary
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
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre, correo, contrasena FROM usuarios AS u WITH(NOLOCK) ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Usuario> usuarios = new List<Usuario>();

                while (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Contrasena = reader["Contrasena"].ToString(),
                    };

                    usuarios.Add(usuario);
                }

                if (usuarios.Count > 0)
                {
                    conn.Close();
                    return usuarios;
                }

                conn.Close();
                return null;
            }
        }

        public dynamic Login(Usuario usuario)
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre, correo, contrasena FROM usuarios AS u WITH(NOLOCK) WHERE correo = @Correo ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    conn.Close();
                    return null;
                }

                Usuario usuarioEncontrado = new Usuario
                {
                    Id = (int)reader["Id"],
                    Nombre = reader["Nombre"].ToString(),
                    Correo = reader["Correo"].ToString(),
                    Contrasena = reader["Contrasena"].ToString()
                };

                if (usuario.Contrasena != usuarioEncontrado.Contrasena)
                {
                    conn.Close();
                    conn.Open();

                    string confirmationQuery = "INSERT INTO bitacora (usuario_id, login_status) VALUES (@Usuario_id, @Status)".Replace("'", "").Replace("%", "").Replace(" --", "");
                    cmd = new SqlCommand(confirmationQuery, conn);
                    cmd.Parameters.AddWithValue("@Usuario_id", usuarioEncontrado.Id);
                    cmd.Parameters.AddWithValue("@Status", "failed");
                    cmd.ExecuteNonQuery();

                    return null;
                }

                conn.Close();
                conn.Open();

                query = "INSERT INTO bitacora (usuario_id, login_status) VALUES (@Usuario_id, @Status)".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario_id", usuarioEncontrado.Id);
                cmd.Parameters.AddWithValue("@Status", "success");
                cmd.ExecuteNonQuery();

                return usuarioEncontrado;
            }
        }

        public dynamic GetById(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre, correo, contrasena FROM usuarios AS u WITH(NOLOCK) WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Contrasena = reader["Contrasena"].ToString(),
                    };

                    conn.Close();
                    return usuario;
                }

                conn.Close();
                return null;
            }
        }

        public dynamic GetPermisos()
        {
            using(var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "EXEC get_permisos_usuarios".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<dynamic> permisos = new List<dynamic>();

                while (reader.Read())
                {
                     var permiso = new
                    {
                        Usuario_Id = (int)reader["Usuario_Id"],
                        Usuario = reader["Usuario"].ToString(),
                        Pol = reader["rol"].ToString(),
                        Permiso = reader["permiso"].ToString()
                    };

                    permisos.Add(permiso);
                }

                if (permisos.Count > 0)
                {
                    return permisos;
                }

                return null;
            }
        }

        public dynamic Create(Usuario usuario)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "INSERT INTO usuarios (nombre, correo, contrasena) VALUES (@Nombre, @Correo, @Contrasena) ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                cmd.ExecuteNonQuery();

                conn.Close();
                conn.Open();

                query = "SELECT id, nombre, correo, contrasena FROM usuarios AS u WITH(NOLOCK) WHERE nombre = @Nombre AND correo = @Correo AND contrasena = @Contrasena ".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
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

        public dynamic Update(int id, Usuario usuario)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre, correo, contrasena FROM usuarios AS u WITH(NOLOCK) WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                conn.Close();
                conn.Open();

                query = "UPDATE usuarios SET nombre = @Nombre, correo = @Correo, contrasena = @Contrasena WHERE id = @Id".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();
                return new { message = "Updated successfully" };
            }
        }

        public dynamic Delete(int id)
        {
            using (var conn = _db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nombre, correo, contrasena FROM usuarios AS u WITH(NOLOCK) WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
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

                query = "DELETE FROM usuarios WHERE id = @Id ".Replace("'", "").Replace("%", "").Replace(" --", "");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                conn.Close();
                return new { message = "Deleted successfully!" };
            }
        }
    }
}
