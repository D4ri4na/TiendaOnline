using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace TiendaOnline
{
    public class ManejoDeUsuarios
    {
        private readonly Dictionary<string, Usuario> users;

        public ManejoDeUsuarios()
        {
            users = new Dictionary<string, Usuario>
            {
                { "admin", new Usuario("admin", "admin123", "admin") },
                { "cliente", new Usuario("cliente", "cliente123", "cliente") }
            };
        }

        public void Register()
        {
            Console.Clear();
            Console.WriteLine("=== Registro de Usuario ===\n");

            string username;
            do
            {
                Console.Write("Ingrese un nombre de usuario (mínimo 3 caracteres): ");
                username = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(username) || username.Length < 3)
                {
                    Console.WriteLine("El nombre de usuario debe tener al menos 3 caracteres.");
                    continue;
                }

                if (users.ContainsKey(username))
                {
                    Console.WriteLine("El usuario ya existe. Por favor, elija otro nombre.");
                    username = null;
                }
            } while (string.IsNullOrEmpty(username));

            string password;
            do
            {
                Console.Write("Ingrese una contraseña (mínimo 7 caracteres y al menos un número): ");
                password = Console.ReadLine();

                if (!IsValidPassword(password))
                {
                    Console.WriteLine("La contraseña no cumple con los requisitos. Intente de nuevo.");
                }
            } while (!IsValidPassword(password));

            users.Add(username, new Usuario(username, password, "cliente"));
            Console.WriteLine("\nRegistro exitoso. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public void Login()
        {
            Console.Clear();
            Console.WriteLine("=== Inicio de Sesión ===\n");

            Console.Write("Ingrese su nombre de usuario: ");
            string username = Console.ReadLine();

            Console.Write("Ingrese su contraseña: ");
            string password = Console.ReadLine();

            if (users.TryGetValue(username, out Usuario user) && user.Contraseña == password)
            {
                Console.WriteLine($"\n¡Bienvenido, {username}!");
                if (user.Role == "admin")
                    AdminMenu(user);
                else
                    ClientMenu(user);
            }
            else
            {
                Console.WriteLine("\nUsuario o contraseña incorrectos. Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            return password.Length >= 7 && Regex.IsMatch(password, @"\d");
        }

        private void AdminMenu(Usuario admin)
        {
            bool continueRunning = true;
            while (continueRunning)
            {
                Console.Clear();
                Console.WriteLine("=== Menú de Administrador ===");
                Console.WriteLine("1. Gestionar Inventario");
                Console.WriteLine("2. Ver Reportes de Ventas");
                Console.WriteLine("3. Cerrar Sesión");
                Console.Write("\nSeleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Program.inventario.ManejoInventario();
                        break;
                    case "2":
                        Program.manejoDeReportes.GenerarReporteVentas();
                        break;
                    case "3":
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ClientMenu(Usuario client)
        {
            bool continueRunning = true;
            while (continueRunning)
            {
                Console.Clear();
                Console.WriteLine("=== Menú de Cliente ===");
                Console.WriteLine("1. Ver Productos Disponibles");
                Console.WriteLine("2. Realizar Compra");
                Console.WriteLine("3. Ver Historial de Compras");
                Console.WriteLine("4. Cerrar Sesión");
                Console.Write("\nSeleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Program.inventario.MostrarInventario();
                        break;
                    case "2":
                        Program.manejoDeVentas.Compra(client);
                        break;
                    case "3":
                        Program.manejoDeVentas.MostrarHistorial(client);
                        break;
                    case "4":
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public IEnumerable<Usuario> GetUsers()
        {
            return users.Values;
        }
    }
}