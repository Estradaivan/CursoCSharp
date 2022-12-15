using GestionVentas.Models;
using System.Data;
using System.Data.SqlClient;

namespace GestionVentas.Repositories
{
    public class ProductosRepository
    {
        private SqlConnection? conexion;
        private String cadenaConexion = "Server=207.180.253.56;Database=SistemaGestion;User Id=iotsoporte;Password=6fdf583fe9;";

        public ProductosRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<Producto> listarProductos()
        {
            List<Producto> lista = new List<Producto>();
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM producto", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Producto producto = obtenerProductoDesdeReader(reader);
                                lista.Add(producto);
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

        public Producto? obtenerProducto(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM producto WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Producto producto = obtenerProductoDesdeReader(reader);
                            return producto;
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

        public Producto crearProducto(Producto producto)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Producto(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) VALUES(@descripcion, @costo, @precioVenta, @stock, @idUsuario); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("descripcion", SqlDbType.VarChar) { Value = producto.descripcion });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = producto.costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = producto.precioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = producto.stock });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = producto.idUsuario });
                    producto.id = long.Parse(cmd.ExecuteScalar().ToString());
                    return producto;
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

        public Producto? actualizarProducto(long id, Producto productoAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                Producto? producto = obtenerProducto(id);
                if (producto == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (producto.descripcion != productoAActualizar.descripcion && !string.IsNullOrEmpty(productoAActualizar.descripcion))
                {
                    camposAActualizar.Add("descripciones = @descripcion");
                    producto.descripcion = productoAActualizar.descripcion;
                }
                if (producto.costo != productoAActualizar.costo && productoAActualizar.costo > 0)
                {
                    camposAActualizar.Add("costo = @costo");
                    producto.costo = productoAActualizar.costo;
                }
                if (producto.precioVenta != productoAActualizar.precioVenta && productoAActualizar.precioVenta > 0)
                {
                    camposAActualizar.Add("precioVenta = @precioVenta");
                    producto.precioVenta = productoAActualizar.precioVenta;
                }
                if (producto.stock != productoAActualizar.stock && productoAActualizar.stock > 0)
                {
                    camposAActualizar.Add("stock = @stock");
                    producto.stock = productoAActualizar.stock;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay nuevas filas por actualizar");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {String.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("descripcion", SqlDbType.VarChar) { Value = productoAActualizar.descripcion });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = productoAActualizar.costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = productoAActualizar.precioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoAActualizar.stock });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return producto;
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

        public bool eliminarProducto(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM producto WHERE id = @id", conexion))
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

        private Producto obtenerProductoDesdeReader(SqlDataReader reader)
        {
            Producto producto = new Producto();
            producto.id = long.Parse(reader["Id"].ToString());
            producto.descripcion = reader["Descripciones"].ToString();
            producto.costo = double.Parse(reader["Costo"].ToString());
            producto.precioVenta = double.Parse(reader["PrecioVenta"].ToString());
            producto.stock = int.Parse(reader["Stock"].ToString());
            return producto;
        }


    }
}