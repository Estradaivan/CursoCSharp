using GestionVentas.Models;
using System.Data;
using System.Data.SqlClient;

namespace GestionVentas.Repositories
{
    public class UsuariosRepository
    {
        private SqlConnection? conexion;
        private String cadenaConexion = "Server=207.180.253.56;Database=SistemaGestion;User Id=iotsoporte;Password=6fdf583fe9;";

        public UsuariosRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<Usuario> listarUsuario()
        {
            List<Usuario> lista = new List<Usuario>();
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("select * from Usuario", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Usuario usuario = obtenerUsuarioDesdeReader(reader);
                                lista.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }

        public Usuario? obtenerUsuario(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("select * from Usuario where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Usuario usuario = obtenerUsuarioDesdeReader(reader);
                            return usuario;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public Usuario? crearUsuario(Usuario usuario)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert into Usuario(Nombre, Apellido, NombreUsuario, Contrasenia, Mail) values (@nombre, @apellido, @nombreUsuario, @contrasenia, @mail); select @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuario.nombre });
                    cmd.Parameters.Add(new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuario.apellido });
                    cmd.Parameters.Add(new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = usuario.nombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("contrasenia", SqlDbType.VarChar) { Value = usuario.contrasenia });
                    cmd.Parameters.Add(new SqlParameter("mail", SqlDbType.VarChar) { Value = usuario.mail });
                    usuario.id = long.Parse(cmd.ExecuteScalar().ToString());
                    return usuario;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public Usuario? actualizarUsuario(long id, Usuario usuarioAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                Usuario? usuario = obtenerUsuario(id);
                if (usuario == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (usuario.nombre != usuarioAActualizar.nombre && !string.IsNullOrEmpty(usuarioAActualizar.nombre))
                {
                    camposAActualizar.Add("nombre = @nombre");
                    usuario.nombre = usuarioAActualizar.nombre;
                }
                if (usuario.apellido != usuarioAActualizar.apellido && !string.IsNullOrEmpty(usuarioAActualizar.apellido))
                {
                    camposAActualizar.Add("apellido = @apellido");
                    usuario.apellido = usuarioAActualizar.apellido;
                }
                if (usuario.nombreUsuario != usuarioAActualizar.nombreUsuario && !string.IsNullOrEmpty(usuarioAActualizar.nombreUsuario))
                {
                    camposAActualizar.Add("nombreUsuario = @nombreUsuario");
                    usuario.nombreUsuario = usuarioAActualizar.nombreUsuario;
                }
                if (usuario.contrasenia != usuarioAActualizar.contrasenia && !string.IsNullOrEmpty(usuarioAActualizar.contrasenia))
                {
                    camposAActualizar.Add("contraseña = @contrasenia");
                    usuario.contrasenia = usuarioAActualizar.contrasenia;
                }
                if (usuario.mail != usuarioAActualizar.mail && !string.IsNullOrEmpty(usuarioAActualizar.mail))
                {
                    camposAActualizar.Add("mail = @mail");
                    usuario.mail = usuarioAActualizar.mail;
                }
                if(camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay nuevas filas por actualizar");
                }
                using (SqlCommand cmd = new SqlCommand($"update Usuario set {String.Join((", "), camposAActualizar)} where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuarioAActualizar.nombre });
                    cmd.Parameters.Add(new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuarioAActualizar.apellido });
                    cmd.Parameters.Add(new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = usuarioAActualizar.nombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("contrasenia", SqlDbType.VarChar) { Value = usuarioAActualizar.contrasenia });
                    cmd.Parameters.Add(new SqlParameter("mail", SqlDbType.VarChar) { Value = usuarioAActualizar.mail });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    cmd.ExecuteNonQuery();
                    return usuario;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public bool eliminarUsuario(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("delete from Usuario where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
                conexion.Close();
                return filasAfectadas> 0;
            }
            catch
            {
                throw;
            }
        }

        private Usuario obtenerUsuarioDesdeReader(SqlDataReader reader)
        {
            Usuario usuario = new Usuario();
            usuario.id = long.Parse(reader["Id"].ToString());
            usuario.nombre = reader["Nombre"].ToString();
            usuario.apellido = reader["Apellido"].ToString();
            usuario.nombreUsuario = reader["NombreUsuario"].ToString();
            usuario.contrasenia = reader["Contraseña"].ToString();
            usuario.mail = reader["Mail"].ToString();
            return usuario;
        }
    }
}
