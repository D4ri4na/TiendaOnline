using System;
using System.Linq;
using System.Collections.Generic;

namespace TiendaOnline
{
    public class ManejoDeReportes
    {
        public void GenerarReporteInventario()
        {
            Console.Clear();
            Console.WriteLine("=== Reporte de Inventario ===\n");

            var productos = Program.inventario.GetAllProducts();
            var productosBajoStock = productos.Where(p => p.Stock < 5).ToList();
            var productosSinStock = productos.Where(p => p.Stock == 0).ToList();

            Console.WriteLine("Productos con bajo stock (menos de 5 unidades):");
            Console.WriteLine("Producto".PadRight(20) + "Stock".PadRight(10) + "Categoría");
            Console.WriteLine(new string('-', 45));

            foreach (var producto in productosBajoStock)
            {
                Console.WriteLine($"{producto.Nombre.PadRight(20)}{producto.Stock.ToString().PadRight(10)}{producto.Categoria}");
            }

            Console.WriteLine("\nProductos sin stock:");
            Console.WriteLine("Producto".PadRight(20) + "Categoría");
            Console.WriteLine(new string('-', 35));

            foreach (var producto in productosSinStock)
            {
                Console.WriteLine($"{producto.Nombre.PadRight(20)}{producto.Categoria}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public void GenerarReporteVentas()
        {
            Console.Clear();
            Console.WriteLine("=== Reporte de Ventas por Fecha ===\n");

            Console.Write("Ingrese la fecha inicial (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaInicial))
            {
                Console.WriteLine("Fecha no válida.");
                Console.ReadKey();
                return;
            }

            Console.Write("Ingrese la fecha final (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaFinal))
            {
                Console.WriteLine("Fecha no válida.");
                Console.ReadKey();
                return;
            }

            var ventasEnRango = new Dictionary<string, (int cantidad, double total)>();
            double totalVentas = 0;

            foreach (var usuario in Program.manejoDeUsuarios.GetUsers().Where(u => u.Role == "cliente"))
            {
                var ventas = usuario.PurchaseHistory
                    .Where(p => p.date.Date >= fechaInicial.Date && p.date.Date <= fechaFinal.Date);

                foreach (var venta in ventas)
                {
                    if (!ventasEnRango.ContainsKey(venta.product))
                    {
                        ventasEnRango[venta.product] = (venta.quantity, venta.totalPrice);
                    }
                    else
                    {
                        var actual = ventasEnRango[venta.product];
                        ventasEnRango[venta.product] = (actual.cantidad + venta.quantity, actual.total + venta.totalPrice);
                    }
                    totalVentas += venta.totalPrice;
                }
            }

            Console.WriteLine($"\nVentas del {fechaInicial:dd/MM/yyyy} al {fechaFinal:dd/MM/yyyy}:");
            Console.WriteLine("Producto".PadRight(20) + "Cantidad".PadRight(15) + "Total");
            Console.WriteLine(new string('-', 45));

            foreach (var venta in ventasEnRango)
            {
                Console.WriteLine($"{venta.Key.PadRight(20)}{venta.Value.cantidad.ToString().PadRight(15)}${venta.Value.total:F2}");
            }

            Console.WriteLine(new string('-', 45));
            Console.WriteLine($"Total de Ventas: ${totalVentas:F2}");

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public void GenerarReporteVentasPorCategoria()
        {
            Console.Clear();
            Console.WriteLine("=== Reporte de Ventas por Categoría ===\n");

            var ventasPorCategoria = new Dictionary<string, (int cantidad, double total)>();

            foreach (var usuario in Program.manejoDeUsuarios.GetUsers().Where(u => u.Role == "cliente"))
            {
                foreach (var venta in usuario.PurchaseHistory)
                {
                    if (Program.inventario.TryGetProduct(venta.product, out Productos producto))
                    {
                        string categoria = producto.Categoria;
                        if (!ventasPorCategoria.ContainsKey(categoria))
                        {
                            ventasPorCategoria[categoria] = (venta.quantity, venta.totalPrice);
                        }
                        else
                        {
                            var actual = ventasPorCategoria[categoria];
                            ventasPorCategoria[categoria] = (actual.cantidad + venta.quantity, actual.total + venta.totalPrice);
                        }
                    }
                }
            }

            Console.WriteLine("Categoría".PadRight(20) + "Cantidad".PadRight(15) + "Total");
            Console.WriteLine(new string('-', 45));

            foreach (var venta in ventasPorCategoria)
            {
                Console.WriteLine($"{venta.Key.PadRight(20)}{venta.Value.cantidad.ToString().PadRight(15)}${venta.Value.total:F2}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}