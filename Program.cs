using System;
using System.Globalization;

namespace BadCalcVeryBad
{
    /*
     * =======================
     *  COMENTARIOS DE REFACTORIZACIÓN
     * =======================
     *
     * Cambio 1:
     *   Se eliminó el código original que utilizaba clases auxiliares con estado global,
     *   uso de 'goto' y lógica no relacionada con una calculadora (como escritura de archivos
     *   y plantillas de texto). Este diseño dificultaba la legibilidad y el mantenimiento.
     *
     * Cambio 2:
     *   Se creó la clase estática Calculator con métodos puros para cada operación aritmética.
     *   Esto mejora la claridad, facilita las pruebas y reduce el acoplamiento.
     *
     * Cambio 3:
     *   Se añadió validación de entrada mediante double.TryParse para evitar excepciones
     *   al convertir valores numéricos ingresados por el usuario.
     *
     * Cambio 4:
     *   Se reemplazó el uso de 'goto' por un ciclo while controlado por una variable booleana.
     *   Esto mejora el flujo de control y se ajusta a las buenas prácticas de C#.
     *
     * Cambio 5:
     *   Se eliminó la escritura del archivo AUTO_PROMPT.txt y cualquier otra lógica que no
     *   estuviera directamente relacionada con la funcionalidad de calculadora, reduciendo
     *   riesgos de seguridad y “code smells” detectables por herramientas como SonarQube.
     *
     * Cambio 6:
     *   Se añadieron mensajes claros en consola para guiar al usuario y manejo explícito de
     *   errores comunes (por ejemplo, división por cero).
     */

    /// <summary>
    /// Clase estática que encapsula la lógica de la calculadora.
    /// Cambio: antes la lógica estaba repartida en varias clases y métodos poco claros;
    /// ahora se centraliza en métodos simples y bien nombrados.
    /// </summary>
    public static class Calculator
    {
        // Cambio: se definen operaciones básicas como métodos puros, fáciles de probar.

        public static double Add(double a, double b)
        {
            return a + b;
        }

        public static double Subtract(double a, double b)
        {
            return a - b;
        }

        public static double Multiply(double a, double b)
        {
            return a * b;
        }

        public static double Divide(double a, double b)
        {
            // Cambio: se valida explícitamente la división por cero en lugar de permitir
            // que el runtime devuelva Infinity o lance una excepción no controlada.
            if (Math.Abs(b) < double.Epsilon)
            {
                throw new DivideByZeroException("No es posible dividir por cero.");
            }

            return a / b;
        }

        public static double Power(double @base, double exponent)
        {
            // Cambio: se usa Math.Pow de la BCL, evitando implementaciones manuales imprecisas.
            return Math.Pow(@base, exponent);
        }

        public static double Sqrt(double value)
        {
            if (value < 0)
            {
                // Cambio: se define comportamiento explícito para raíces negativas.
                // Se devuelve NaN para indicar que el resultado no es un número real.
                return double.NaN;
            }

            return Math.Sqrt(value);
        }
    }

    internal class Program
    {
        /// <summary>
        /// Punto de entrada de la aplicación.
        /// Cambio: se reemplaza el uso de etiquetas y 'goto' por un ciclo while con una
        /// variable de control, lo que mejora la estructura y comprensión del flujo.
        /// </summary>
        private static void Main(string[] args)
        {
            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();

                Console.Write("Seleccione una opción: ");
                string? opcion = Console.ReadLine();

                if (opcion == "7")
                {
                    // Cambio: se agregó una opción de salida explícita y clara.
                    Console.WriteLine("Saliendo de la calculadora. ¡Hasta luego!");
                    continuar = false;
                    continue;
                }

                try
                {
                    EjecutarOperacion(opcion);
                }
                catch (Exception ex)
                {
                    // Cambio: manejo genérico de excepciones para evitar cierres inesperados
                    // y entregar un mensaje amigable al usuario.
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Muestra el menú principal de la calculadora.
        /// Cambio: se reorganizó la presentación para que sea más clara y fácil de leer.
        /// </summary>
        private static void MostrarMenu()
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("        CALCULADORA MEJORADA         ");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. Suma");
            Console.WriteLine("2. Resta");
            Console.WriteLine("3. Multiplicación");
            Console.WriteLine("4. División");
            Console.WriteLine("5. Potencia");
            Console.WriteLine("6. Raíz cuadrada");
            Console.WriteLine("7. Salir");
            Console.WriteLine("=====================================");
        }

        /// <summary>
        /// Ejecuta la operación seleccionada por el usuario.
        /// Cambio: se centralizó la lógica de lectura de datos y ejecución en un solo método.
        /// </summary>
        private static void EjecutarOperacion(string? opcion)
        {
            switch (opcion)
            {
                case "1":
                    {
                        double a = LeerNumero("Ingrese el primer número: ");
                        double b = LeerNumero("Ingrese el segundo número: ");
                        double resultado = Calculator.Add(a, b);
                        Console.WriteLine($"Resultado (suma): {resultado}");
                        break;
                    }
                case "2":
                    {
                        double a = LeerNumero("Ingrese el minuendo: ");
                        double b = LeerNumero("Ingrese el sustraendo: ");
                        double resultado = Calculator.Subtract(a, b);
                        Console.WriteLine($"Resultado (resta): {resultado}");
                        break;
                    }
                case "3":
                    {
                        double a = LeerNumero("Ingrese el primer número: ");
                        double b = LeerNumero("Ingrese el segundo número: ");
                        double resultado = Calculator.Multiply(a, b);
                        Console.WriteLine($"Resultado (multiplicación): {resultado}");
                        break;
                    }
                case "4":
                    {
                        double a = LeerNumero("Ingrese el dividendo: ");
                        double b = LeerNumero("Ingrese el divisor: ");
                        double resultado = Calculator.Divide(a, b);
                        Console.WriteLine($"Resultado (división): {resultado}");
                        break;
                    }
                case "5":
                    {
                        double a = LeerNumero("Ingrese la base: ");
                        double b = LeerNumero("Ingrese el exponente: ");
                        double resultado = Calculator.Power(a, b);
                        Console.WriteLine($"Resultado (potencia): {resultado}");
                        break;
                    }
                case "6":
                    {
                        double a = LeerNumero("Ingrese el número para calcular la raíz cuadrada: ");
                        double resultado = Calculator.Sqrt(a);
                        Console.WriteLine($"Resultado (raíz cuadrada): {resultado}");
                        break;
                    }
                default:
                    // Cambio: se maneja explícitamente el caso de opción inválida.
                    Console.WriteLine("Opción no válida. Por favor, seleccione una opción del 1 al 7.");
                    break;
            }
        }

        /// <summary>
        /// Lee un número desde la consola con validación robusta.
        /// Cambio: antes se utilizaban conversiones directas que podían lanzar excepciones;
        /// ahora se usa double.TryParse con reintentos para asegurar que el dato sea válido.
        /// </summary>
        private static double LeerNumero(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();

                if (double.TryParse(entrada, NumberStyles.Float, CultureInfo.InvariantCulture, out double valor))
                {
                    return valor;
                }

                Console.WriteLine("Entrada no válida. Intente de nuevo usando formato numérico correcto (por ejemplo: 12.5).");
            }
        }
    }
}
