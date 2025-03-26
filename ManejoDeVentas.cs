using System;
using System.Linq;

namespace TiendaOnline
{
    public class ManejoDeVentas
    {
        public void Compra(Usuario cliente)
        {
            bool continuarComprando = true;
            while (continuarComprando)
            {
                Console.Clear();
                Console.WriteLine("=== Carrito de Compras ===\n");
                MostrarCarrito(cliente.CarritoActual);

                Console.WriteLine("\n1. Agregar producto al carrito");
                Console.WriteLine("2. Remover producto del carrito");
                Console.WriteLine("3. Vaciar carrito");
                Console.WriteLine("4. Finalizar compra");
                Console.WriteLine("5. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AgregarAlCarrito(cliente);
                        break;
                    case "2":
                        RemoverDelCarrito(cliente);
                        break;
                    case "3":
                        cliente.CarritoActual.VaciarCarrito();
                        Console.WriteLine("Carrito vaciado exitosamente.");
                        Console.ReadKey();
                        break;
                    case "4":
                        if (FinalizarCompra(cliente))
                            continuarComprando = false;
                        break;
                    case "5":
                        continuarComprando = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void MostrarCarrito(Carrito carrito)
        {
            if (!carrito.Items.Any())
            {
                Console.WriteLine("El carrito está vacío.");
                return;
            }

            Console.WriteLine("Productos en el carrito:");
            Console.WriteLine("Producto".PadRight(20) + "Cantidad");
            Console.WriteLine(new string('-', 35));

            foreach (var item in carrito.Items)
            {
                Console.WriteLine($"{item.Key.PadRight(20)}{item.Value}");
            }

            Console.WriteLine(new string('-', 35));
            Console.WriteLine($"Total: ${carrito.Total:F2}");
        }

        private void AgregarAlCarrito(Usuario cliente)
        {
            Console.Clear();
            Console.Write("Ingrese la categoría del producto que desea comprar: ");
            string categoria = Console.ReadLine();

            var productosEnCategoria = Program.inventario.GetProductsByCategory(categoria).ToList();

            if (!productosEnCategoria.Any())
            {
                Console.WriteLine("No hay productos disponibles en esa categoría.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nProductos disponibles en la categoría seleccionada:\n");
            Console.WriteLine("Producto".PadRight(20) + "Stock".PadRight(10) + "Precio");
            Console.WriteLine(new string('-', 40));

            foreach (var productos in productosEnCategoria)
            {
                Console.WriteLine($"{productos.Nombre.PadRight(20)}{productos.Stock.ToString().PadRight(10)}${productos.Precio:F2}");
            }

            Console.Write("\nIngrese el nombre del producto a comprar: ");
            string nombreProducto = Console.ReadLine();

            if (!Program.inventario.TryGetProduct(nombreProducto, out Productos producto))
            {
                Console.WriteLine("Producto no encontrado.");
                Console.ReadKey();
                return;
            }

            Console.Write("Ingrese la cantidad: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad) || cantidad <= 0)
            {
                Console.WriteLine("Cantidad no válida.");
                Console.ReadKey();
                return;
            }

            if (producto.Stock < cantidad)
            {
                Console.WriteLine("Stock insuficiente.");
                Console.ReadKey();
                return;
            }

            cliente.CarritoActual.AgregarItem(nombreProducto, cantidad, producto.Precio);
            Console.WriteLine("Producto agregado al carrito.");
            Console.ReadKey();
        }

        private void RemoverDelCarrito(Usuario cliente)
        {
            if (!cliente.CarritoActual.Items.Any())
            {
                Console.WriteLine("El carrito está vacío.");
                Console.ReadKey();
                return;
            }

            Console.Write("\nIngrese el nombre del producto a remover: ");
            string nombreProducto = Console.ReadLine();

            if (!cliente.CarritoActual.Items.ContainsKey(nombreProducto))
            {
                Console.WriteLine("Producto no encontrado en el carrito.");
                Console.ReadKey();
                return;
            }

            if (Program.inventario.TryGetProduct(nombreProducto, out Productos producto))
            {
                cliente.CarritoActual.RemoverItem(nombreProducto, producto.Precio);
                Console.WriteLine("Producto removido del carrito.");
                Console.ReadKey();
            }
        }

        private bool FinalizarCompra(Usuario cliente)
        {
            if (!cliente.CarritoActual.Items.Any())
            {
                Console.WriteLine("El carrito está vacío.");
                Console.ReadKey();
                return false;
            }

            var resultadoPago = Program.manejoDePagos.ProcesarPago(cliente.CarritoActual.Total);
            if (!resultadoPago.exito)
                return false;

            foreach (var item in cliente.CarritoActual.Items)
            {
                if (Program.inventario.TryGetProduct(item.Key, out Productos producto))
                {
                    Program.inventario.UpdateStock(item.Key, item.Value);
                    cliente.PurchaseHistory.Add((
                        item.Key,
                        item.Value,
                        item.Value * producto.Precio,
                        DateTime.Now,
                        resultadoPago.metodoPago,
                        resultadoPago.numeroFactura
                    ));
                }
            }

            cliente.CarritoActual.VaciarCarrito();
            Console.WriteLine("¡Compra finalizada exitosamente!");
            Console.ReadKey();
            return true;
        }

        public void MostrarHistorial(Usuario cliente)
        {
            Console.Clear();
            Console.WriteLine("=== Historial de Compras ===\n");

            if (!cliente.PurchaseHistory.Any())
            {
                Console.WriteLine("No hay compras registradas.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Fecha".PadRight(12) + "Producto".PadRight(20) + "Cantidad".PadRight(10) +
                            "Total".PadRight(15) + "Método de Pago".PadRight(20) + "No. Factura");
            Console.WriteLine(new string('-', 85));

            foreach (var compra in cliente.PurchaseHistory)
            {
                Console.WriteLine($"{compra.date:dd/MM/yyyy}".PadRight(12) +
                                $"{compra.product}".PadRight(20) +
                                $"{compra.quantity}".PadRight(10) +
                                $"${compra.totalPrice:F2}".PadRight(15) +
                                $"{compra.paymentMethod}".PadRight(20) +
                                $"{compra.invoiceNumber}");
            }

            var totalGastado = cliente.PurchaseHistory.Sum(c => c.totalPrice);
            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"Total gastado: ${totalGastado:F2}");

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
