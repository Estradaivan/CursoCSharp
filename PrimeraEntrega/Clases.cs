using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeraEntrega
{
    public class Usuario
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nombreUsuario { get; set; }
        public string contrasenia { get; set; }
        public string mail { get; set; }
        public Usuario(int id, string nombre, string apellido, string nombreUsuario, string contrasenia, string mail)
        {
            this.id = id;
            this.nombre = nombre;
            this.apellido = apellido;
            this.nombreUsuario = nombreUsuario;
            this.contrasenia = contrasenia;
            this.mail = mail;
        }
        public Usuario()
        {
            id = 0;
            nombre = "";
            apellido = "";
            nombreUsuario = "";
            contrasenia = "";
            mail = "";
        }
    }

    public class Producto
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public double costo { get; set; }
        public double precioVenta { get; set; }
        public int stock { get; set; }
        public int idUsuario { get; set; }

        public Producto(int id, string descripcion, double costo, double precioVenta, int stock, int idUsuario)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.costo = costo;
            this.precioVenta = precioVenta;
            this.stock = stock;
            this.idUsuario = idUsuario;
        }

        public Producto()
        {
            id = 0;
            descripcion = "";
            costo = 0;
            precioVenta = 0;
            stock = 0;
            idUsuario = 0;
        }

    }

    public class ProductoVendido
    {
        public int id { get; set; }
        public int idProducto { get; set; }
        public int stock { get; set; }
        public int idVenta { get; set; }

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

    public class Venta
    {
        public int id { get; set; }
        public string comentarios { get; set; }

        public Venta(int id, string comentarios)
        {
            this.id = id;
            this.comentarios = comentarios;
        }
        public Venta()
        {
            id = 0;
            comentarios = "";
        }
    }
}
