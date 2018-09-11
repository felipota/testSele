using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herramientas;
using AutomatizarApp;
using System.Configuration;

namespace AutomatizarApp
{
    class Program
    {
        public static CargarArchivo Cargue = new CargarArchivo();
        public static Dictionary<int, Dictionary<string, string>> DatosConfig = null;
        static void Main(string[] args)
        {
            ProcesarArchivo Procesar = new ProcesarArchivo();
            Console.WriteLine("Ingresa el nombre del archivo de configuración");
            string NombreArchivo = Console.ReadLine();
            string archivoConfig = ConfigurationManager.AppSettings["archivoConfig"].ToString()+NombreArchivo+".csv";
            DatosConfig = Cargue.CargaDeArchivo(archivoConfig, "Archivo de configuración");
            for (int i = 0; i <= DatosConfig.Count - 1; i++)
            {
                string texto = Procesar.leerTexto(DatosConfig[i]["RutaProyecto"]);
                Procesar.CrearInstancia(texto,DatosConfig[i]);
            }


        }
    }
}
