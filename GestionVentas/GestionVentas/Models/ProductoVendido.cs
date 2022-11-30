namespace GestionVentas.Models
{
    public class ProductoVendido
    {
        public long id { get; set; }
        public long idProducto { get; set; }
        public int stock { get; set; }
        public long idVenta { get; set; }

        public ProductoVendido(int id, int idProducto, int stock, int idVenta)
        {
            this.id = id;
            this.idProducto = idProducto;
            this.stock = stock;
            this.idVenta = idVenta;
        }
        public ProductoVendido()
        {
            id = 0;
            idProducto = 0;
            stock = 0;
            idVenta = 0;
        }
    }
}
