using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;

namespace Herramientas
{
    public class CargarArchivo
    {
        #region Variables
        //string RutaArchivo = ConfigurationManager.AppSettings["RutaOrigen"];
        private static Log EscribirLog = new Log();
        #endregion

        #region Métodos
        public Dictionary<int, Dictionary<string, string>> CargaDeArchivo(string RutaArchivo, string nombreArchivo)
        {
            string nombreLog = ConfigurationManager.AppSettings["RutaOrigen"]+"Error-Archivo configuracion"+DateTime.Now.ToString("hhmmssFFF") + ".txt";
            try {
                Dictionary<int, Dictionary<string, string>> DatosArchivo = new Dictionary<int, Dictionary<string, string>>();
                Dictionary<string, string> CasoPrueba = null;
                if (File.Exists(RutaArchivo))
                {
                    using (StreamReader str = new StreamReader(RutaArchivo,Encoding.Default))
                    {
                        string Linea;
                        int key = 0;
                        string[] Campos = str.ReadLine().Split(','); 
                        while ((Linea = str.ReadLine()) != null)
                        {
                            CasoPrueba = new Dictionary<string, string>();
                            string[] datos = Linea.Split(',');
                            for (int i =0;i<=Campos.Length-1;i++) {
                                CasoPrueba.Add(Campos[i],datos[i]);
                            }
                            DatosArchivo.Add(key, CasoPrueba);
                            key++;
                        }
                    }
                }
                return DatosArchivo;
            }
            catch (Exception ex)
            {
                string error = "Se generó error cargando el archivo: " + nombreArchivo + ex.Message;
                EscribirLog.EscribirLog(error, nombreLog);
                throw ex;
            }
        }

        
        #endregion


    }
}
