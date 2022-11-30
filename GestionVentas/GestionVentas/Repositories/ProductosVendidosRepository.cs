using GestionVentas.Models;
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
                                ProductoVendido productovendido = new ProductoVendido();
                                productovendido.id = long.Parse(reader["Id"].ToString());
                                productovendido.idProducto = long.Parse(reader["IdProducto"].ToString());
                                productovendido.idVenta = long.Parse(reader["IdVenta"].ToString());
                                productovendido.stock = int.Parse(reader["Stock"].ToString());

                                lista.Add(productovendido);
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
