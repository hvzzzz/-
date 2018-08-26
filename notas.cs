using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
using static System.ConsoleColor;
using static Menu;
enum Menu { DefinirExpresion= 1,IngresarNotas,MostrarPromedio,Salir }
namespace notas
{
	class StringTokenizer
    {
        //----- ATRIBUTOS ----------------------------------------------------------
        private int currentPosition;
        private int newPosition;
        private int maxPosition;
        private string str;
        private string delimiters;
        private bool retDelims;
        private bool delimsChanged;
        private char maxDelimChar;

        //---- METODOS -------------------------------------------------------------

        //---- Constructores -------------------------------------------------------
        public StringTokenizer(string str, string delim, bool returnDelims)
        {
            currentPosition = 0;
            newPosition = -1;
            delimsChanged = false;
            this.str = str;
            maxPosition = str.Length;
            delimiters = delim;
            retDelims = returnDelims;
            setMaxDelimChar();
        }
        //--------------------------------------------------------------------------     
        public StringTokenizer(string str, string delim)
            : this(str, delim, false)
        {
        }
        //--------------------------------------------------------------------------          
        public StringTokenizer(string str)
            : this(str, " \t\n\r\f", false)
        {
        }
        //--------------------------------------------------------------------------

        //---- Otros metodos -------------------------------------------------------                 
        private void setMaxDelimChar()
        {
            if (delimiters == null)
            {
                maxDelimChar = (char)0;
                return;
            }
            char m = (char)0;
            for (int i = 0; i < delimiters.Length; i++)
            {
                char c = delimiters[i];
                if (m < c)
                    m = c;
            }
            maxDelimChar = m;
        }
        //--------------------------------------------------------------------------
        private int skipDelimiters(int startPos)
        {
            if (delimiters == null)
                Console.WriteLine("Null Pointer Exception.");

            int position = startPos;
            while (!retDelims && position < maxPosition)
            {
                char c = str[position];
                if ((c > maxDelimChar) || (delimiters.IndexOf(c) < 0))
                    break;
                position++;
            }
            return position;
        }
        //--------------------------------------------------------------------------     
        private int scanToken(int startPos)
        {
            int position = startPos;
            while (position < maxPosition)
            {
                char c = str[position];
                if ((c <= maxDelimChar) && (delimiters.IndexOf(c) >= 0))
                    break;
                position++;
            }
            if (retDelims && (startPos == position))
            {
                char c = str[position];
                if ((c <= maxDelimChar) && (delimiters.IndexOf(c) >= 0))
                    position++;
            }
            return position;
        }
        //--------------------------------------------------------------------------          
        public bool hasMoreTokens()
        {
            newPosition = skipDelimiters(currentPosition);
            return (newPosition < maxPosition);
        }
        //--------------------------------------------------------------------------               
        public string nextToken()
        {
            currentPosition = (newPosition >= 0 && !delimsChanged) ?
                newPosition : skipDelimiters(currentPosition);

            /* Reset these anyway */
            delimsChanged = false;
            newPosition = -1;

            if (currentPosition >= maxPosition)
                Console.WriteLine("No Such Element Exception");
            int start = currentPosition;
            currentPosition = scanToken(currentPosition);
            return str.Substring(start, currentPosition - start);  //------ Adecuado
        }
        //--------------------------------------------------------------------------               
        public string nextToken(string delim)
        {
            delimiters = delim;

            /* delimiter string specified, so set the appropriate flag. */
            delimsChanged = true;

            setMaxDelimChar();
            return nextToken();
        }
        //--------------------------------------------------------------------------                    
        public bool hasMoreElements()
        {
            return hasMoreTokens();
        }
        //--------------------------------------------------------------------------                         
        public object nextElement()
        {
            return nextToken();
        }
        //--------------------------------------------------------------------------                              
        public int countTokens()
        {
            int count = 0;
            int currpos = currentPosition;
            while (currpos < maxPosition)
            {
                currpos = skipDelimiters(currpos);
                if (currpos >= maxPosition)
                    break;
                currpos = scanToken(currpos);
                count++;
            }
            return count;
        }
        //--------------------------------------------------------------------------                              
    }// End StringTokenizer 
	class CPila
    {
        // Atributos
        public object Elemento { get; set; }
        public CPila SubPila { get; set; }

        // Métodos
        // --- Constructores
        public CPila(object pElemento = null, CPila pSubPila = null)
        {
            Elemento = pElemento;
            SubPila = pSubPila;
        }
        // --- Otros métodos
        public bool EsVacia()
        {
            return (Elemento == null && SubPila == null);
        }

        public void Apilar(object pElemento)
        {
            SubPila = new CPila(Elemento, SubPila);
            Elemento = pElemento;
        }

        public void Desapilar()
        {
            if (!EsVacia())
            {
                Elemento = SubPila.Elemento;
                SubPila = SubPila.SubPila;
            }
        }

        public object Cima()
        {
            if (EsVacia())
                return null;
            else
                return Elemento;
        }
    }
	class CCalculadora
    {
        // Atributos
        public string Expresion { get; set; }
        // Métodos
        // ----- Constructores
        public CCalculadora(string pExpresion = "")
        {
            Expresion = pExpresion;
        }
        // ----- Otras Operaciones
        // ----------------------------------------------------------------------------------
        public float Evaluar()
        {
            string expresionPosFijo;
            // ----- Convertir expresión InFija a PosFija
            CConvertidorAPosFijo convertidorAPosFijo = new CConvertidorAPosFijo(Expresion);
            expresionPosFijo = convertidorAPosFijo.Convertir(Expresion);//lleva el string a postfijo
            // ----- Evaluar expresión PosFija
            CEvaluadorPosFijo evaluadorPosFijo = new CEvaluadorPosFijo();
            return evaluadorPosFijo.Evaluar(expresionPosFijo);//le pasa el string en postfijo al evaluador 
        }
        // ----------------------------------------------------------------------------------
        public float Evaluar(string expresion)
        {
            Expresion = expresion;
            return Evaluar();
        }
    }
	class CConvertidorAPosFijo
    {
        // Atributos
        public string Expresion { get; set; }

        // Métodos
        // ----- Constructores
        public CConvertidorAPosFijo(string pExpresion = "")
        {
            Expresion = pExpresion;
        }
        // ----- Otras Operaciones
        // --------------------------------------------------------------
        static bool OkProcedencia(string token1, string token2)
        {
            if (token1.Equals("+") || token1.Equals("-"))
                return (!token2.Equals("("));
            else
                if ((token1.Equals("*") || token1.Equals("/")) &&
                    (token2.Equals("*") || token2.Equals("/") ||
                     token2.Equals("^")))
                    return true;
                else
                    return false;
        }
        // --------------------------------------------------------------
        static string ProcesarToken(string token, CPila pila, string expresionPosFijo)
        {
            // ----- Si token = ")" desapilar todos los operadores hasta encontrar "("
            if (token.Equals(")"))
            {
                while ((!pila.EsVacia()) && (!((string)pila.Cima()).Equals("(")))
                {
                    expresionPosFijo = expresionPosFijo + (string)pila.Cima();
                    // ---- Desapilar operador
                    pila.Desapilar();
                }
                // ---- Quitar de la pila el ")"
                if (!pila.EsVacia())
                    pila.Desapilar();
            }
            else
                if (token.Equals("+") || token.Equals("-") ||
                    token.Equals("*") || token.Equals("/") ||
                    token.Equals("^"))
                {
                    // ---- Desapilar operadores, si existen de acuerdo a us precedencia
                    while ((!pila.EsVacia()) && OkProcedencia(token, (string)pila.Cima()))
                    {
                        // ---- Agregar operador a la expresion PosFijo
                        expresionPosFijo = expresionPosFijo + (string)pila.Cima();
                        // ---- Desapilar operador
                        pila.Desapilar();
                    }
                    // ---- Apilar nuevo operador
                    pila.Apilar(token);
                }
                else
                    // ---- Si es "(" apilar
                    if (token.Equals("("))
                        pila.Apilar(token);
                    else
                        // ---- Si no es " " agregar token a expreseionPosFijo, caso 
                        //      contrario ignorar token.
                        if (!token.Equals(" "))
                            expresionPosFijo = expresionPosFijo + " " + token;
            // ---- Retornar valor de expresión posFijo
            return expresionPosFijo;
        }
        // --------------------------------------------------------------
        public string Convertir()
        {
            // ---- Declarar objetos
            CPila pila = new CPila();
            StringTokenizer st = new StringTokenizer(Expresion, "+-*/^() ", true);
            string token;
            string expresionPosFijo = "";

            // ---- Convertir expresión, descomponiendo en tokens
            if (st.countTokens() > 0)
                do
                {
                    token = st.nextToken();
                    expresionPosFijo = ProcesarToken(token, pila, expresionPosFijo);
                } while (st.hasMoreTokens());
            // ----- Desapilar todos los operadores que quedan en la pila
            while (!pila.EsVacia())
            { 
                // ----- Agregar operador a la expresión PosFijo
                expresionPosFijo = expresionPosFijo + (string)pila.Cima();
                // ----- Desapilar operador
                pila.Desapilar();
            }
            return expresionPosFijo;
        }
        // --------------------------------------------------------------
        public string Convertir(string expresion)
        {
            Expresion = expresion;
            return Convertir();
        }
    }
	class CEvaluadorPosFijo
    {
        // Atributos
        public string Expresion { get; set; }

        // Métodos
        // ---- Constructores
        public CEvaluadorPosFijo(string pExpresion = "")
        {
            Expresion = pExpresion;
        }
        // ----- Otras Operaciones
        //-----------------------------------------------------------------
        static float Potencia(float bas, float exponente)
        {
            return (float)Math.Exp(exponente * Math.Log(bas));
        }
        //-----------------------------------------------------------------
		public float Evaluar(string expresion)
        {
            // ----- Declarar objetos
            CPila pila;
            StringTokenizer st;
            string token;

            // ----- Crear un objeto pila
            pila = new CPila();
            // ----- Crear un objeto StringTokenizer
            st = new StringTokenizer(expresion, "+-*/^ ", true);

            // ----- Evaluar expresión, descomponiendo en tokens
            if (st.countTokens() > 0)
                do
                {
                    token = st.nextToken();
                    ProcesarToken(token, pila);
                } while (st.hasMoreTokens());
            // ----- Obtener el resultado de la pila
            return ((float)pila.Cima());
        }
        //-----------------------------------------------------------------
        static void ProcesarToken(string token, CPila pila)
        {
            // ----- Este metodo procesa un token, considerando tres casos:
            //       a) Si token es un operador (+,-,*,/,^) desapila dos
            //          operandos de la pila, efectua la operación y apila el resultado
            //       b) Si token es un operando, simplemente apila en la pila
            //       c) Si token es un blanco, simplemente se ignora
			
            if (token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/") ||token.Equals("^"))
            {
                // ----- Recuperar Operandos. Notar que el metodo Cima() de la
                //       pila devuelve un object, por tanto es necesario
                //       convertirlo en un float mediante un casting

                // obtiene operando de la pila
                float operandoDer = float.Parse(pila.Cima().ToString());

                // elimina operando de la pila
                pila.Desapilar();

                // obtiene siguiente operando de la pila
                float operandoIzq = float.Parse(pila.Cima().ToString());

                // elimina operando restante de la pila
                pila.Desapilar();

                // ---- Efectuar operacion y apilar el resultado
                //      Notar que el resultado de cada operación
                //      se debe apilar como un objeto de tipo Float
                if (token.Equals("+"))
                    pila.Apilar((float)(operandoIzq + operandoDer));
                else if (token.Equals("-"))
                    pila.Apilar((float)(operandoIzq - operandoDer));
                else if (token.Equals("*"))
                    pila.Apilar((float)(operandoIzq * operandoDer));
                else if (token.Equals("/"))
                    pila.Apilar((float)(operandoIzq / operandoDer));
                else if (token.Equals("^"))
                    pila.Apilar((float)(Potencia(operandoIzq, operandoDer)));
            }
            else
                if (!token.Equals(" "))  // ----- token es un operando
                    pila.Apilar((token));
        }
        //-----------------------------------------------------------------
    }
	class Program
    {
		static int cont1=0;
		static int cont2=0;
		static string numero="";
		static string aux="";
		static double[] estado=new double[]{0,0,0,0,0,0,0,0}; 
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
                case DefinirExpresion:
					leerinput();
                    break;
                case IngresarNotas:
					showinput();
                    break;
                case MostrarPromedio:
					remaker();
                    break;
                case Salir:
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
		static void Compute()
		{
			Clear();
			CCalculadora calculadora = new CCalculadora();
			Array.Clear(estado, 0, estado.Length);
			WriteLine("El Promedio de Notas es:");
			Console.WriteLine("" + calculadora.Evaluar(aux));
			aux="";
			numero="";
			ReadLine();
		}
		static bool parentesis(string numero)//parentesis equilibrados
		{
			StringTokenizer a1= new StringTokenizer(numero,"+-*/^()",true);
			string token;
			if (a1.countTokens() > 0)
			{	
                do
                {
                    token = a1.nextToken();
					if(token=="(")
						cont1++;
					if(token==")")
						cont2++;
					//WriteLine(token);
                } while (a1.hasMoreTokens());
			}
			if(cont1==cont2)
			{
				cont1=0;
				cont2=0;
				return true;
			}	
			else
			{	
				cont1=0;
				cont2=0;
				return false;
			}	
		}
		static void remaker()
		{
			string token="";
			StringTokenizer a1= new StringTokenizer(numero,"+-*/^()",true);
			if (a1.countTokens() > 0)
			{	
                do
                {
                    token = a1.nextToken();
					if(token=="EP")
					{
						aux=aux+estado[0].ToString();
						token="";
					}
					if(token=="AC")
					{
						aux=aux+estado[1].ToString();
						token="";
					}
					if(token=="EO")
					{
						aux=aux+estado[2].ToString();
						token="";
					}
					if(token=="PC")
					{
						aux=aux+estado[3].ToString();
						token="";
					}
					if(token=="PD")
					{
						aux=aux+estado[4].ToString();
						token="";
					}
					if(token=="PL")
					{
						aux=aux+estado[5].ToString();
						token="";
					}
					if(token=="SE")
					{
						aux=aux+estado[6].ToString();
						token="";
					}
					if(token=="TR")
					{
						aux=aux+estado[7].ToString();
						token="";
					}
					aux=aux+token;
                } 
				while (a1.hasMoreTokens());
				Compute();
			}
		}
		static void leerinput()
		{
			Clear();
            WriteLine("AC - Actividades");
            WriteLine("EP - Examen Parcial");
			WriteLine("EO - Examen Oral");
            WriteLine("PC - Promedio de Practicas Calificas");
			WriteLine("PD - Promedio de Practicas Dirigidas");
            WriteLine("PL - Promedio de Laboratorios");
			WriteLine("SE - Seminarios");
            WriteLine("TR - Trabajos o Asignaciones");
            Write("Digite la Expresion: ");
			numero=ReadLine();
			StringTokenizer a1= new StringTokenizer(numero,"+-*/^()",true);
			string token;
			if (a1.countTokens() > 0)// detectamos las variables de entrada
			{	
                do
                {
                    token = a1.nextToken();
				
					if(token=="EP")
					{
						estado[0]=1;
					}
					if(token=="AC")
					{
						estado[1]=1;
					}
					if(token=="EO")
					{
						estado[2]=1;
					}
					if(token=="PC")
					{
						estado[3]=1;
					}
					if(token=="PD")
					{
						estado[4]=1;
					}
					if(token=="PL")
					{
						estado[5]=1;
					}
					if(token=="SE")
					{
						estado[6]=1;
					}
					if(token=="TR")
					{
						estado[7]=1;
					}
				 	if(token!="EP"&&token!="AC"&&token!="EO"&&token!="PC"&&token!="PD"&&token!="PL"&&token!="SE"&&token!="TR"&&token!="^"&&token!="+"&&token!="-"&&token!="*"&&token!=")"&&token!="("&&token!="0"&&token!="/"&&token!="0.1"&&token!="1"&&token!="0.2"&&token!="2"&&token!="0.3"&&token!="3"&&token!="0.4"&&token!="4"&&token!="0.5"&&token!="5"&&token!="0.6"&&token!="6"&&token!="0.7"&&token!="7"&&token!="0.8"&&token!="8"&&token!="0.9"&&token!="9")
					{
						Array.Clear(estado, 0, estado.Length);
						ForegroundColor = Red;
						WriteLine("Expresion Invalida");
						ResetColor();
						Write("\nPresione ");
						ForegroundColor = White;
						Write("ENTER");
						ResetColor();
						Write(" para intentar de nuevo...");
						ReadLine();	
						break;
					}					
                } while (a1.hasMoreTokens());
			}
			if(parentesis(numero)==false)
			{
				Array.Clear(estado, 0, estado.Length);
				ForegroundColor = Red;
                WriteLine("Los Parentesis no estan Equilibrados");
                ResetColor();
                Write("\nPresione ");
                ForegroundColor = White;
                Write("ENTER");
                ResetColor();
                Write(" para intentar de nuevo...");
                ReadLine();	
			}//llegados a este punto ya sabemos que variables se ingresaron
		}
		static void showinput()
		{
			for (int i=0;i<8;i++)
			{
				if(estado[i]==1)
				{
					if(i==0)
					{
						Write("EP= ");
						estado[0]=int.Parse(ReadLine());
					}
					if(i==1)
					{
						Write("AC= ");
						estado[1]=int.Parse(ReadLine());
					}
					if(i==2)
					{
						Write("EO= ");
						estado[2]=int.Parse(ReadLine());
					}
					if(i==3)
					{
						Write("PC= ");
						estado[3]=int.Parse(ReadLine());
					}
					if(i==4)
					{
						Write("PD= ");
						estado[4]=int.Parse(ReadLine());
					}
					if(i==5)
					{
						Write("PL= ");
						estado[5]=int.Parse(ReadLine());
					}
					if(i==6)
					{
						Write("SE= ");
						estado[6]=int.Parse(ReadLine());
					}
					if(i==7)
					{
						Write("TR= ");
						estado[7]=int.Parse(ReadLine());
					}
				}	
			}
		}
		static void Main()
        {
			var option = 0;
            do
            {
                option = MostrarMenu();
                Opcion(option);
			} 
			while (option != (int)Salir);
		}
	}
}
