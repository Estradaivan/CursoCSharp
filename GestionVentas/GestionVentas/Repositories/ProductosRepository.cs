using GestionVentas.Models;
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

        public List<Producto> listarProducto()
        {
            List<Producto> lista = new List<Producto>();
            if (conexion == null)
            {
                throw new Exception("Conexion no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("select * from Producto", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Producto producto = new Producto();
                                producto.id = long.Parse(reader["Id"].ToString());
                                producto.idUsuario = long.Parse(reader["IdUsuario"].ToString());
                                producto.stock = int.Parse(reader["stock"].ToString());
                                producto.precioVenta = double.Parse(reader["PrecioVenta"].ToString());
                                producto.costo = double.Parse(reader["Costo"].ToString());
                                producto.descripcion = reader["Descripciones"].ToString();

                                lista.Add(producto);
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