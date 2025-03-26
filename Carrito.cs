using System.Collections.Generic;

namespace TiendaOnline
{
    public class Carrito
    {
        public Dictionary<string, int> Items { get; private set; }
        public double Total { get; private set; }

        public Carrito()
        {
            Items = new Dictionary<string, int>();
            Total = 0;
        }

        public void AgregarItem(string producto, int cantidad, double precio)
        {
            if (Items.ContainsKey(producto))
            {
                Items[producto] += cantidad;
            }
            else
            {
                Items.Add(producto, cantidad);
            }
            Total += cantidad * precio;
        }

        public void RemoverItem(string producto, double precio)
        {
            if (Items.ContainsKey(producto))
            {
                Total -= Items[producto] * precio;
                Items.Remove(producto);
            }
        }

        public void VaciarCarrito()
        {
            Items.Clear();
            Total = 0;
        }
    }
}