using Herramientas;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace PruebaVinculacion
{
    public class Program
    {
        #region Variables
        private static Funcionalidad metodo = new Funcionalidad();
        public static CargarArchivo Cargue = new CargarArchivo();
        public static Dictionary<int, Dictionary<string, string>> DatosConfig = null;

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Ingresa el nombre del archivo de configuración");
            string NombreArchivo = Console.ReadLine();
            string archivoConfig = ConfigurationManager.AppSettings["archivoConfig"].ToString() + NombreArchivo + ".csv";
            DatosConfig = Cargue.CargaDeArchivo(archivoConfig, "Archivo de configuración");
            FormulariosVinculacion miPrueba = new FormulariosVinculacion();

            for (int i = 0; i <= DatosConfig.Count - 1; i++)
            {
                metodo.IniciarPrueba(miPrueba, DatosConfig[i]);
            }
        }

     }
}
