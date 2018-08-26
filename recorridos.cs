using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace recorridos
{
    public class CArbolBB
    {
        // Atributos
        public object Raiz { get; set; }
        public CArbolBB SubArbolIzq { get; set; }
        public CArbolBB SubArbolDer { get; set; }

        // Métodos
        // -- Constructores
        public CArbolBB(object pRaiz = null, CArbolBB pSubArbolIzq = null,
                        CArbolBB pSubArbolDer = null)
        {
            Raiz = pRaiz;
            SubArbolIzq = pSubArbolIzq;
            SubArbolDer = pSubArbolDer;
        }
        // -- Otros metodos
        public bool EsVacio()
        {
            return Raiz == null;
        }
        // ----------------------------------------------------------------------------
        public void Agregar(object pElem)
        {
            if (Raiz == null)
                Raiz = pElem;
            else
                if (pElem.ToString().CompareTo(Raiz.ToString()) < 0)
                {
                    if (SubArbolIzq == null)
                        SubArbolIzq = new CArbolBB(pElem);
                    else
                        SubArbolIzq.Agregar(pElem);
                }
                else  // pElem.ToString().CompareTo(arbol.Raiz.ToString()) > 0
                {
                    if (SubArbolDer == null)
                        SubArbolDer = new CArbolBB(pElem);
                    else
                        SubArbolDer.Agregar(pElem);
                }
        }
        // ----------------------------------------------------------------------------
        public virtual void Eliminar(Object pRaiz)
        {
            if (EsVacio())
                Console.WriteLine("ERROR. Elemento no encontrado...");
            else
            {
                // ----- Verificar si la raiz es el elemento que se desea eliminar
                if (pRaiz.Equals(Raiz))
                {
                    // Si no tiene hijos, eliminar la raiz o una hoja
                    if (SubArbolIzq == null && SubArbolDer == null)
                        Raiz = null;
                    else // árbol tiene por lo menos un hijo   
                        if (SubArbolIzq == null) // ----- Solo tiene hijo derecho		
                        {
                            Raiz = SubArbolDer.Raiz;
                            SubArbolIzq = SubArbolDer.SubArbolIzq;
                            SubArbolDer = SubArbolDer.SubArbolDer;
                        }
                        else
                            if (SubArbolDer == null) // ----- Solo tiene hijo izquierdo		
                            {
                                Raiz = SubArbolIzq.Raiz;
                                SubArbolDer = SubArbolIzq.SubArbolDer;
                                SubArbolIzq = SubArbolIzq.SubArbolIzq;
                            }
                            else // Tiene ambos hijos
                            {
                                Raiz = SubArbolDer.Minimo();
                                SubArbolDer.Eliminar(Raiz);
                            }
                }
                else
                    // ----- Verificar si el elemento a eliminar esta en el hijo Izq
                    if (pRaiz.ToString().CompareTo(Raiz.ToString()) < 0)
                    {
                        if (SubArbolIzq != null)
                            SubArbolIzq.Eliminar(pRaiz);
                    }
                    else
                        // ----- Elemento a eliminar esta en el hijo Der
                        if (SubArbolDer != null)
                            SubArbolDer.Eliminar(pRaiz);
                // Verificar si los hijos son hojas vacias 
                if (SubArbolIzq != null && SubArbolIzq.EsVacio())
                    SubArbolIzq = null;
                if (SubArbolDer != null && SubArbolDer.EsVacio())
                    SubArbolDer = null;
            }
        }
        // ----------------------------------------------------------------------------
        public CArbolBB SubArbol(object pRaiz)
        {
            if (EsVacio())
                return null;
            else
                if (Raiz.Equals(pRaiz))
                    return this;
                else
                    if (pRaiz.ToString().CompareTo(Raiz.ToString()) < 0)
                        return SubArbolIzq != null ? SubArbolIzq.SubArbol(pRaiz) : null;
                    else
                        return SubArbolDer != null ? SubArbolDer.SubArbol(pRaiz) : null;
        }
        // ----------------------------------------------------------------------------
        public CArbolBB Padre(object pRaiz)
        {
            if (EsVacio())
                return null;
            else
                if (EsHijo(pRaiz))
                    return this;
                else
                    if (pRaiz.ToString().CompareTo(Raiz.ToString()) < 0)
                        return SubArbolIzq != null ? SubArbolIzq.Padre(pRaiz) : null;
                    else
                        return SubArbolDer != null ? SubArbolDer.Padre(pRaiz) : null;
        }
        // ----------------------------------------------------------------------------
        public bool EsHijo(object pRaiz)
        {
            return ((SubArbolIzq != null && SubArbolIzq.Raiz.Equals(pRaiz)) ||
                    (SubArbolDer != null && SubArbolDer.Raiz.Equals(pRaiz)));
        }
        // ----------------------------------------------------------------------------
        public object Minimo()
        {
            if (EsVacio())
                return null;
            else
                return SubArbolIzq == null ? Raiz : SubArbolIzq.Minimo();
        }
        // ----------------------------------------------------------------------------
        public object Maximo()
        {
            if (EsVacio())
                return null;
            else
                return SubArbolDer == null ? Raiz : SubArbolDer.Maximo();
        }
        // ----------------------------------------------------------------------------
        public int Altura()
        {
            if (EsVacio())
                return 0;
            else
            {
                int AlturaIzq = (SubArbolIzq == null ? 0 : 1 + SubArbolIzq.Altura());
                int AlturaDer = (SubArbolDer == null ? 0 : 1 + SubArbolDer.Altura());
                return (AlturaIzq > AlturaDer ? AlturaIzq : AlturaDer);
            }
        }
        // ----------------------------------------------------------------------------
        public void PreOrden()
        {
            if (Raiz != null)
            {
                // ----- Procesar la raiz
                Console.WriteLine(Raiz.ToString());
                // ----- Procesar hijo Izq 
                if (SubArbolIzq != null)
                    SubArbolIzq.PreOrden();
                // ----- Procesar hijo Der 
                if (SubArbolDer != null)
                    SubArbolDer.PreOrden();
            }
        }
        // ----------------------------------------------------------------------------
        public void InOrden()
        {
            if (Raiz != null)
            {
                // ----- Procesar hijo Izq 
                if (SubArbolIzq != null)
                    SubArbolIzq.InOrden();
                // ----- Procesar la raiz
                Console.WriteLine(Raiz.ToString());
                // ----- Procesar hijo Der 
                if (SubArbolDer != null)
                    SubArbolDer.InOrden();
            }
        }
        // ----------------------------------------------------------------------------
        public void PostOrden()
        {
            if (Raiz != null)
            {
                // ----- Procesar hijo Izq 
                if (SubArbolIzq != null)
                    SubArbolIzq.PostOrden();
                // ----- Procesar hijo Der 
                if (SubArbolDer != null)
                    SubArbolDer.PostOrden();
                // ----- Procesar la raiz
                Console.WriteLine(Raiz.ToString());
            }
        }
    }
	class Program  
    {
		static void Agregar(CArbolBB arbol, object pElem)
        {
            if (arbol.Raiz == null)
                arbol.Raiz = pElem;
            else
                if (pElem.ToString().CompareTo(arbol.Raiz.ToString()) < 0)
                    if (arbol.SubArbolIzq == null)
                        arbol.SubArbolIzq = new CArbolBB(pElem);
                    else
                        Agregar(arbol.SubArbolIzq, pElem);

            if (pElem.ToString().CompareTo(arbol.Raiz.ToString()) > 0)
                if (arbol.SubArbolDer == null)
                    arbol.SubArbolDer = new CArbolBB(pElem);
                else
                    Agregar(arbol.SubArbolDer, pElem);
        }
        static void PreOrden(CArbolBB arbol)
        {
			if (arbol.Raiz != null)
            {
                // -- procesar raiz
                Console.WriteLine(arbol.Raiz);
                // -- procesar hijo izquierdo
                if (arbol.SubArbolIzq != null)
                    PreOrden(arbol.SubArbolIzq);
                // -- procesar hijo derecho
                if (arbol.SubArbolDer != null)
                    PreOrden(arbol.SubArbolDer);
            }
        }
        static void InOrden(CArbolBB arbol)
        {
            if (arbol.Raiz != null)
            {
                // -- procesar hijo izquierdo
                if (arbol.SubArbolIzq != null)
                    InOrden(arbol.SubArbolIzq);
                // -- procesar raiz(hazlo que quieras con la raiz)
                Console.WriteLine(arbol.Raiz);
                // -- procesar hijo derecho
                if (arbol.SubArbolDer != null)
                    InOrden(arbol.SubArbolDer);
            }
        }
        static void PosOrden(CArbolBB arbol)
        {
            if (arbol.Raiz != null)
            {
                // -- procesar hijo izquierdo
                if (arbol.SubArbolIzq != null)
                    PosOrden(arbol.SubArbolIzq);
                // -- procesar hijo derecho
                if (arbol.SubArbolDer != null)
                    PosOrden(arbol.SubArbolDer);
                // -- procesar raiz
                Console.WriteLine(arbol.Raiz);
            }
        }
		 static void RDI(CArbolBB arbol)
        {
            if (arbol.Raiz != null)
            {
				Console.WriteLine(arbol.Raiz);
				if (arbol.SubArbolDer != null)
                    RDI(arbol.SubArbolDer);
                if (arbol.SubArbolIzq != null)
                    RDI(arbol.SubArbolIzq);
            }
        } static void DRI(CArbolBB arbol)
        {
            if (arbol.Raiz != null)
            {
				if(arbol.SubArbolDer != null)
					DRI(arbol.SubArbolDer);
                Console.WriteLine(arbol.Raiz);
                if (arbol.SubArbolIzq != null)
                    DRI(arbol.SubArbolIzq);
            }
        } static void DIR(CArbolBB arbol)
        {
            if (arbol.Raiz != null)
            {
				if (arbol.SubArbolDer != null)
                    DIR(arbol.SubArbolDer);
                if (arbol.SubArbolIzq != null)
                    DIR(arbol.SubArbolIzq);
				Console.WriteLine(arbol.Raiz);
            }
        }
        static void Main(string[] args)  
        {  
			CArbolBB arbol = new CArbolBB();
            Agregar(arbol, "36");
            Agregar(arbol, "81");
            Agregar(arbol, "25");
            Agregar(arbol, "35");
            Agregar(arbol, "33");
            Agregar(arbol, "74");
            Agregar(arbol, "10");
            Agregar(arbol, "90");
            Console.WriteLine("Recorrido en Profundidad");
            Console.WriteLine("========================");
            Console.WriteLine("1. PreOrden");
            Console.WriteLine("-----------");
            PreOrden(arbol);
            Console.WriteLine("2. InOrden");
            Console.WriteLine("-----------");
            InOrden(arbol);
            Console.WriteLine("3. PosOrden");
            Console.WriteLine("-----------");
            PosOrden(arbol);
            Console.WriteLine("4.RDI ");
            Console.WriteLine("-----------");
            RDI(arbol);
            Console.WriteLine("5. DRI");
            Console.WriteLine("-----------");
            DRI(arbol);
            Console.WriteLine("6. DIR");
            Console.WriteLine("-----------");
            DIR(arbol);
        }  
    }   
}
