using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;

namespace Herramientas
{
    public class Log
        {


        /// <summary>
        ///  Método creado para dejar log en un archivo txt
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void EscribirLog(string log, string rutaLog)
        {
            try
            {
                Thread.Sleep(1000);
                byte[] newLine = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine);

                byte[] LogByte = Encoding.ASCII.GetBytes(log);
                using (FileStream str = new FileStream(rutaLog, FileMode.Append))
                {
                    str.Write(LogByte, 0, LogByte.Length);
                    str.Write(newLine, 0, newLine.Length);
                    str.Close();
                }
            }
            catch(Exception ex)
            {
                string edd = ex.Message;
            }

        }

        /// <summary>
        ///  Método creado para capturar el popup de error cuando hay campos con errores
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void EscribirLogArray(string path, Dictionary<int, Dictionary<string, string>> DatosBd, Dictionary<int, Dictionary<string, string>> DatosA,string condiciones )
        {
            string Final = "Para las condiciones {0} el resultado fue:";
            string resultado = "";
            for(int i =0;i <= DatosBd.Count - 1; i++)
                {
                    string[] Keys = DatosBd[i].Keys.ToArray();
                    string[] Values = DatosBd[i].Values.ToArray();
                    for(int j=0;j<= Keys.Count() - 1; j++)
                    {
                    resultado =resultado+Keys[j] + ":" + DatosA[i][Keys[j]] +"|";
                   }
            }
            EscribirLog(string.Format(Final,condiciones)+resultado, path);

        }
    }
}
