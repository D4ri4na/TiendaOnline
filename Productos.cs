namespace TiendaOnline
{
    public class Productos
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
        public double Precio { get; set; }

        public Productos(string nombre, string categoria, int stock, double precio)
        {
            Nombre = nombre;
            Categoria = categoria;
            Stock = stock;
            Precio = precio;
        }
    }
}