using GestionVentas.Models;
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
                                Usuario usuario = new Usuario();
                                usuario.id = long.Parse(reader["Id"].ToString());
                                usuario.nombre = reader["Nombre"].ToString();
                                usuario.apellido = reader["Apellido"].ToString();
                                usuario.nombreUsuario = reader["NombreUsuario"].ToString();
                                usuario.contrasenia = reader["Contraseña"].ToString();
                                usuario.mail = reader["Mail"].ToString();

                                lista.Add(usuario);
                            }
                        }
                    }
                }
                conexion.Close();
            }
            catch (Exception ex)
            {

            }
            return lista;
        }
    }
}
