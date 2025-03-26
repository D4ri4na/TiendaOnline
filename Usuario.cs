using System;
using System.Collections.Generic;

namespace TiendaOnline
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; }
        public List<(string product, int quantity, double totalPrice, DateTime date, string paymentMethod, string invoiceNumber)> PurchaseHistory { get; set; }
        public Carrito CarritoActual { get; set; }

        public Usuario(string nombre, string contraseña, string rol)
        {
            Nombre = nombre;
            Contraseña = contraseña;
            Rol = rol;
            PurchaseHistory = new List<(string, int, double, DateTime, string, string)>();
            CarritoActual = new Carrito();
        }
    }
}