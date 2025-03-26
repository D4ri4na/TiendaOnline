using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace TiendaOnline
{
    class Program
    {
        public static readonly ManejoDeUsuarios manejoDeUsuarios = new ManejoDeUsuarios();
        public static readonly Inventario inventario = new Inventario();
        public static readonly ManejoDeVentas manejoDeVentas = new ManejoDeVentas();
        public static readonly ManejoDePagos manejoDePagos = new ManejoDePagos();
        public static readonly ManejoDeReportes manejoDeReportes = new ManejoDeReportes();

        static void Main()
        {
            bool continueRunning = true;
            while (continueRunning)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Sistema de Gestión de Tienda ===");
                    Console.WriteLine("1. Registrarse");
                    Console.WriteLine("2. Iniciar Sesión");
                    Console.WriteLine("3. Salir");
                    Console.Write("\nSeleccione una opción: ");

                    string option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            manejoDeUsuarios.Register();
                            break;
                        case "2":
                            manejoDeUsuarios.Login();
                            break;
                        case "3":
                            continueRunning = false;
                            Console.WriteLine("¡Gracias por usar el sistema!");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }
}