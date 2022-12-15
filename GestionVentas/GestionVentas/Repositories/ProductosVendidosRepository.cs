using GestionVentas.Models;
using System.Data;
using System.Data.SqlClient;

namespace GestionVentas.Repositories
{
    public class ProductosVendidosRepository
    {
        private SqlConnection? conexion;
        private String cadenaConexion = "Server=207.180.253.56;Database=SistemaGestion;User Id=iotsoporte;Password=6fdf583fe9;";

        public ProductosVendidosRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<ProductoVendido> listarProductoVendido()
        {
            List<ProductoVendido> lista = new List<ProductoVendido>();
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("select * from ProductoVendido", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductoVendido productoVendido = obtenerProductoVendidoDesdeReader(reader);
                                lista.Add(productoVendido);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            { 
                conexion.Close(); 
            }
            return lista;
        }

        public ProductoVendido? obtenerProductoVendido(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("select * from ProductoVendido where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id});
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();  
                            ProductoVendido productoVendido = obtenerProductoVendidoDesdeReader(reader);
                            return productoVendido;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public ProductoVendido? crearProductoVendido(ProductoVendido productoVendido)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("insert into ProductoVendido (Stock, IdProducto, IdVenta) values (@stock, @idProducto, @idVenta); select @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoVendido.stock });
                    cmd.Parameters.Add(new SqlParameter("idProducto", SqlDbType.BigInt) { Value = productoVendido.idProducto });
                    cmd.Parameters.Add(new SqlParameter("idVenta", SqlDbType.BigInt) { Value = productoVendido.idVenta });
                    productoVendido.id = long.Parse(cmd.ExecuteScalar().ToString());
                    return productoVendido;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public ProductoVendido? actualizarProductoVendido(long id, ProductoVendido productoVendidoAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                ProductoVendido? productoVendido = obtenerProductoVendido(id);
                if (productoVendido == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (productoVendido.stock != productoVendidoAActualizar.stock && productoVendidoAActualizar.stock > 0)
                {
                    camposAActualizar.Add("stock = @stock");
                    productoVendido.stock = productoVendidoAActualizar.stock;
                }
                if (productoVendido.idProducto != productoVendidoAActualizar.idProducto && productoVendidoAActualizar.idProducto > 0)
                {
                    camposAActualizar.Add("idProducto = @idProducto");
                    productoVendido.idProducto = productoVendidoAActualizar.idProducto;
                }
                if (productoVendido.idVenta != productoVendidoAActualizar.idVenta && productoVendidoAActualizar.idVenta > 0)
                {
                    camposAActualizar.Add("idVenta = @idVenta");
                    productoVendido.idVenta = productoVendidoAActualizar.idVenta;
                }
                if(camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay nuevas filas por actualizar");
                }
                using (SqlCommand cmd = new SqlCommand($"update ProductoVendido set {String.Join(",", camposAActualizar)} where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoVendidoAActualizar.stock });
                    cmd.Parameters.Add(new SqlParameter("idProducto", SqlDbType.BigInt) { Value = productoVendidoAActualizar.idProducto });
                    cmd.Parameters.Add(new SqlParameter("idVenta", SqlDbType.BigInt) { Value = productoVendidoAActualizar.idVenta });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    cmd.ExecuteNonQuery();
                    return productoVendido;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public bool eliminarProductoVendido(long id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("delete from ProductoVendido where id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
                conexion.Close();
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ProductoVendido obtenerProductoVendidoDesdeReader(SqlDataReader reader)
        {
            ProductoVendido productovendido = new ProductoVendido();
            productovendido.id = long.Parse(reader["Id"].ToString());
            productovendido.idProducto = long.Parse(reader["IdProducto"].ToString());
            productovendido.idVenta = long.Parse(reader["IdVenta"].ToString());
            productovendido.stock = int.Parse(reader["Stock"].ToString());
            return productovendido;
        }
    }
}
