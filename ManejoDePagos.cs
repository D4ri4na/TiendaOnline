using System;
using System.Collections.Generic;

namespace TiendaOnline
{
    public class ManejoDePagos
    {
        private readonly List<string> metodoPagos = new List<string> { "Efectivo", "Tarjeta de Crédito", "Tarjeta de Débito", "Transferencia" };
        private int ultimoNumeroFactura = 1000;

        public (bool exito, string metodoPago, string numeroFactura) ProcesarPago(double monto)
        {
            Console.Clear();
            Console.WriteLine("=== Proceso de Pago ===\n");
            Console.WriteLine($"Total a pagar: ${monto:F2}\n");

            Console.WriteLine("Métodos de pago disponibles:");
            for (int i = 0; i < metodoPagos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {metodoPagos[i]}");
            }

            Console.Write("\nSeleccione el método de pago: ");
            if (!int.TryParse(Console.ReadLine(), out int opcionPago) || opcionPago < 1 || opcionPago > metodoPagos.Count)
            {
                Console.WriteLine("Método de pago no válido.");
                Console.ReadKey();
                return (false, null, null);
            }

            string metodoPagoSeleccionado = metodoPagos[opcionPago - 1];
            string numeroFactura = GenerarNumeroFactura();

            Console.WriteLine($"\nProcesando pago con {metodoPagoSeleccionado}...");
            Console.WriteLine($"Número de factura: {numeroFactura}");
            Console.WriteLine("Pago procesado exitosamente.");
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();

            return (true, metodoPagoSeleccionado, numeroFactura);
        }

        private string GenerarNumeroFactura()
        {
            ultimoNumeroFactura++;
            return $"FAC-{DateTime.Now:yyyyMMdd}-{ultimoNumeroFactura}";
        }
    }
}