using System;
using System.Collections.Generic;
using System.Linq;

namespace TiendaOnline
{
    public class Inventario
    {
        private readonly Dictionary<string, Productos> inventory;

        public Inventario()
        {
            inventory = new Dictionary<string, Productos>
            {
                { "Laptop", new Productos("Laptop", "Electronica", 10, 1200.50) },
                { "Telefono", new Productos("Telefono", "Electronica", 20, 800.75) },
                { "Tablet", new Productos("Tablet", "Electronica", 15, 500.00) },
                { "Mouse", new Productos("Mouse", "Accesorios", 12, 12.50) },
                { "Teclado", new Productos("Teclado", "Accesorios", 15, 25.00) },
                { "Monitor", new Productos("Monitor", "Electronica", 5, 300.00) },
                { "Audifonos", new Productos("Audifonos", "Accesorios", 20, 50.00) },
                { "Impresora", new Productos("Impresora", "Electronica", 8, 150.00) }
            };
        }

        public void ManejoInventario()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=== Gestión de Inventario ===");
                Console.WriteLine("1. Ver Inventario");
                Console.WriteLine("2. Agregar Producto");
                Console.WriteLine("3. Modificar Stock");
                Console.WriteLine("4. Volver al Menú Principal");
                Console.Write("\nSeleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        MostrarInventario();
                        break;
                    case "2":
                        AddProduct();
                        break;
                    case "3":
                        ModifyStock();
                        break;
                    case "4":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void MostrarInventario()
        {
            Console.Clear();
            Console.WriteLine("=== Inventario Actual ===\n");
            Console.WriteLine("Producto".PadRight(20) + "Categoría".PadRight(15) + "Stock".PadRight(10) + "Precio");
            Console.WriteLine(new string('-', 55));

            foreach (var item in inventory.Values)
            {
                Console.WriteLine($"{item.Nombre.PadRight(20)}{item.Categoria.PadRight(15)}{item.Stock.ToString().PadRight(10)}${item.Precio:F2}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("=== Agregar Nuevo Producto ===\n");

            Console.Write("Nombre del producto: ");
            string name = Console.ReadLine();

            if (inventory.ContainsKey(name))
            {
                Console.WriteLine("Este producto ya existe en el inventario.");
                Console.ReadKey();
                return;
            }

            Console.Write("Categoría: ");
            string category = Console.ReadLine();

            Console.Write("Stock inicial: ");
            if (!int.TryParse(Console.ReadLine(), out int stock) || stock < 0)
            {
                Console.WriteLine("Stock no válido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Precio: $");
            if (!double.TryParse(Console.ReadLine(), out double price) || price < 0)
            {
                Console.WriteLine("Precio no válido.");
                Console.ReadKey();
                return;
            }

            inventory.Add(name, new Productos(name, category, stock, price));
            Console.WriteLine("\nProducto agregado exitosamente.");
            Console.ReadKey();
        }

        private void ModifyStock()
        {
            Console.Clear();
            Console.WriteLine("=== Modificar Stock ===\n");

            Console.Write("Nombre del producto: ");
            string name = Console.ReadLine();

            if (!inventory.TryGetValue(name, out Productos product))
            {
                Console.WriteLine("Producto no encontrado.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Stock actual: {product.Stock}");
            Console.Write("Nuevo stock: ");

            if (!int.TryParse(Console.ReadLine(), out int newStock) || newStock < 0)
            {
                Console.WriteLine("Stock no válido.");
                Console.ReadKey();
                return;
            }

            product.Stock = newStock;
            Console.WriteLine("Stock actualizado exitosamente.");
            Console.ReadKey();
        }

        public bool TryGetProduct(string name, out Productos product)
        {
            return inventory.TryGetValue(name, out product);
        }

        public void UpdateStock(string productName, int quantity)
        {
            if (inventory.TryGetValue(productName, out Productos product))
            {
                product.Stock -= quantity;
            }
        }

        public IEnumerable<Productos> GetAllProducts()
        {
            return inventory.Values;
        }

        public IEnumerable<Productos> GetProductsByCategory(string category)
        {
            return inventory.Values.Where(p => p.Categoria.Equals(category, StringComparison.OrdinalIgnoreCase));
        }
    }
}