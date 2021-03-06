using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.ConsoleColor;
using static Categorias;
using static Menu;
enum Menu { Agregar = 1, Atender, Mostrar, Simulacion, Cerrar }
enum Categorias { C, S }

namespace ColadeBanco
{
    class CCola
    {
        // Atributos
        public object Elemento { get; set; }
        public CCola SubCola { get; set; }
        // Métodos
        // ---- Constructores
        public CCola(object pElement = null, CCola pSubCola = null)
        {
            Elemento = pElement;
            SubCola = pSubCola;
        }
        // ---- Otros métodos
        public bool EsVacia()
        {
            return (Elemento == null && SubCola == null);
        }
        // -------------------------------------------------------------
        public void Acolar(object pElemento) // Agregar o AgregarFinal
        {
            if (EsVacia())
            {
                SubCola = new CCola(Elemento, SubCola);
                Elemento = pElemento;
            }
            else
                SubCola.Acolar(pElemento);
        }
        // -------------------------------------------------------------
        public void Desacolar()  // Avanzar o Eliminar
        {
            if (!EsVacia())
            {
                Elemento = SubCola.Elemento;
                SubCola = SubCola.SubCola;
            }
        }
        // -------------------------------------------------------------
        public object Primero()
        {
            return Elemento;
        }
        // -------------------------------------------------------------
        public int Longitud()
        {
            if (EsVacia())
                return 0;
            else
                return 1 + SubCola.Longitud();
        }
        // ----- Si existe devuelve un valor mayor a 0, caso contrario 0
        public int Ubicacion(object pElemento)
        {
            if (EsVacia())
                return 0;  // No existe en la cola
            else
                if (Elemento.Equals(pElemento))
                return 1;
            else
            {
                int k = SubCola.Ubicacion(pElemento);
                return ((k > 0) ? 1 + k : 0);
            }
        }
        public void Listar()
        {
            if (!EsVacia())
            {
                Console.WriteLine(Primero().ToString());
                SubCola.Listar();
            }
        }
        // -------------------------------------------------------------
        // -------------------------------------------------------------
    }
    class Program
    {
        static CCola cola1 = new CCola();
        //static CCola colaprefe= new CCola();
        static int clientes = 0;
        static int MostrarMenu()
        {
            Clear();
            ForegroundColor = White;
            WriteLine("Menu principal\n");
            ResetColor();
            foreach (var item in Enum.GetValues(typeof(Menu)))
                WriteLine($"{(int)item} - {item}");
            var input = ReadLine();
            int opcion;
            if (!int.TryParse(input, out opcion))
                return 0;
            return opcion;
        }

        static void Opcion(int opt)
        {
            switch ((Menu)opt)
            {
                case Agregar:
                    IncluirClienteFila();
                    break;
                case Atender:
                    RetirarClienteFila();
                    break;
                case Mostrar:
                    ExibirFila();
                    break;
                case Simulacion:
                    break;
                case Cerrar:
                    break;
                default:
                    ForegroundColor = Red;
                    WriteLine("Opcion inválida!");
                    ResetColor();
                    Write("\nPresione ");
                    ForegroundColor = White;
                    Write("ENTER");
                    ResetColor();
                    Write(" para intentar de nuevo...");
                    ReadLine();
                    break;
            }
        }
        static void IncluirClienteFila()
        {
            Clear();
            WriteLine("C - Con Tarjeta (Clientes)");
            WriteLine("S - Sin tarjeta (Visitantes)");
            Write("Digite la categoria: ");
            var categoria = default(Categorias);
            try
            {
                categoria = (Categorias)Enum.Parse(typeof(Categorias), ReadLine(), true);
            }
            catch (Exception)
            {
                ForegroundColor = Red;
                WriteLine("\nCategoria inválida! Operación cancelada.");
                ResetColor();
                Write("\nPressione ");
                ForegroundColor = White;
                Write("ENTER");
                ResetColor();
                Write(" para volver...");
                ReadLine();
                return;
            }
            //x.number = ++clientes;
            cola1.Acolar(categoria);
            clientes++;
            ForegroundColor = Yellow;
            WriteLine("\nAdicionado!");
            ResetColor();
            Write("\nPresione ");
            ForegroundColor = White;
            Write("ENTER");
            ResetColor();
            Write(" para continuar...");
            ReadLine();
        }
        static void RetirarClienteFila()
        {
            Clear();
            if (clientes == 0)
            {
                ForegroundColor = Red;
                WriteLine("No Quedan Clientes por Atender");
                ResetColor();
                Write("\nPresione ");
                ForegroundColor = White;
                Write("ENTER");
                ResetColor();
                Write(" para regresar al menu anterior...");
                ReadLine();
                return;
            }
            var numerodecaja = new Random();    
            var atenciones = clientes;//guardo el valor de clientes en atenciones para no perderlo
                                      //WriteLine(rnd);
            for (var i = 0; i < atenciones; i++)
            {
                var caja = numerodecaja.Next(1, 6);
                switch (caja)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        cola1.Desacolar();
                        clientes--;
                        WriteLine("\n");
                        break;
                }
            }
            WriteLine($"Total de {atenciones} clientes atendidos");
            Write("\nPresione ");
            ForegroundColor = Green;
            Write("ENTER");
            ResetColor();
            Write(" para continuar...");
            ReadLine();
        }
        static void ExibirFila()
        {
            Clear();
            WriteLine($"Clientes en Espera");
            ForegroundColor = White;
            for (var i = 0; i < clientes; i++)
            {
                WriteLine(cola1.Primero().ToString());
                cola1.Desacolar();
            }
            ResetColor();
            Write("\nPresione ");
            ForegroundColor = White;
            Write("ENTER");
            ResetColor();
            Write(" para continuar...");
            ReadLine();
        }
        static void Lineas(int ancho, int alto)
        {
            int cont = 0;
            while (cont <= ancho)
            {
                Console.SetCursorPosition(cont, 0);
                Console.Write(" ");
                Console.SetCursorPosition(cont, alto);
                Console.Write(" ");
                cont++;
            }
            cont = 2;
            while (cont < alto)
            {
                Console.SetCursorPosition(0, cont + 2);
                Console.Write("|");
                Console.SetCursorPosition(ancho, cont);
                Console.Write("|");
                cont++;
            }
        }
        static void Cuadro(int ancho, int alto)
        {
            int cont = 0;
            while (cont <= ancho)
            {
                Console.SetCursorPosition(cont, 0);
                Console.Write("-");
                Console.SetCursorPosition(cont, alto);
                Console.Write("-");
                cont++;
            }
            cont = 0;
            while (cont <= alto)
            {
                Console.SetCursorPosition(0, cont);
                Console.Write("|");
                Console.SetCursorPosition(ancho, cont);
                Console.Write("|");
                cont++;
            }
        }
        static void Punto(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("☺");
        }
        static void Caja(int a, int b)
        {
            Console.SetCursorPosition(a, b);
            Console.Write("■");
        }
        static void Main()
        {
            var option = 0;
            do
            {
                option = MostrarMenu();
                Opcion(option);
                if (option == 4)
                {
                    Clear();
                    Random rnd = new Random();
                    for (int i = 1; i < 5 + 2; i++)
                    {
                        Lineas(i * 6, 9);
                    }
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.WriteLine("      |");
                    Console.Write("      |");
                    for (int i = 0; i < 40; i++)
                    {
                        Console.Write("_");
                    }
                    for (int i = 1; i < 5 + 1; i++)
                    {
                        Caja((i * 6) + 3, 2);
                    }
                    Cuadro(60, 15);
                    for (int k=0; k < clientes; k++)
                    {
                        int j = rnd.Next(0, clientes);
                        for (int i = 50; i > (j + 1) * 6 + 3; i--)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Punto(i, 10);
                            Thread.Sleep(50);
                            Console.ForegroundColor = Console.BackgroundColor;
                            Punto(i, 10);
                        }
                        for (int i = 10; i > 3; i--)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Punto((j + 1) * 6 + 3, i);
                            Thread.Sleep(50);
                            Console.ForegroundColor = Console.BackgroundColor;
                            Punto((j + 1) * 6 + 3, i);
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Punto((j + 1) * 6 + 3, 3);                        
                    }
                    Clear();
                    Console.SetCursorPosition(35, 10);
                    Console.Write("Simulacion Terminada");
                    Console.ReadKey();
                    Clear();
                }
            }
            while (option != (int)Cerrar);
        }
    }
}
