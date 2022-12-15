using GestionVentas.Models;
using System.Data;
using System.Data.SqlClient;

namespace GestionVentas.Repositories
{
    public class VentasRepository
    {
        private SqlConnection? conexion;
        private String cadenaConexion = "Server=207.180.253.56;Database=SistemaGestion;User Id=iotsoporte;Password=6fdf583fe9;";

        public VentasRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<Venta> listarVenta()
        {
            List<Venta> lista = new List<Venta>();
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("select * from Venta", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Venta venta = obtenerVentaDesdeReader(reader);
                                lista.Add(venta);
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

        public Venta? obtenerVenta(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("select * from Venta where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Venta venta = obtenerVentaDesdeReader(reader);
                            return venta;
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

        public Venta crearVenta(Venta venta)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert into Venta (Comentarios, IdUsuario) values (@comentarios, @idUsuario); select @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("comentarios", SqlDbType.VarChar) { Value = venta.comentarios });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = venta.idUsuario });
                    venta.id = long.Parse(cmd.ExecuteScalar().ToString());
                    return venta;
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

        public Venta? actualizarVenta(long id, Venta ventaAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                Venta? venta = obtenerVenta(id);
                if(venta == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (venta.comentarios != ventaAActualizar.comentarios && !string.IsNullOrEmpty(ventaAActualizar.comentarios))
                {
                    camposAActualizar.Add("comentarios = @comentarios");
                    venta.comentarios = ventaAActualizar.comentarios;
                }
                if (venta.idUsuario != ventaAActualizar.idUsuario && ventaAActualizar.idUsuario > 0)
                {
                    camposAActualizar.Add("idUsuario = @idUsuario");
                    venta.idUsuario = ventaAActualizar.idUsuario;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay nuevas filas por actualizar");
                }
                using (SqlCommand cmd = new SqlCommand($"update Venta set {String.Join(", ", camposAActualizar)} where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("comentarios", SqlDbType.VarChar) { Value = ventaAActualizar.comentarios });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = ventaAActualizar.idUsuario });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    cmd.ExecuteNonQuery();
                    return venta;
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

        public bool eliminarVenta(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("delete from Venta where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
                conexion.Close();
                return filasAfectadas > 0;
            }
            catch
            {
                throw;
            }
        }

        private Venta obtenerVentaDesdeReader(SqlDataReader reader)
        {
            Venta venta = new Venta();
            venta.id = long.Parse(reader["Id"].ToString());
            venta.comentarios = reader["Comentarios"].ToString();
            venta.idUsuario = long.Parse(reader["IdUsuario"].ToString());
            return venta;
        }
    }
}
