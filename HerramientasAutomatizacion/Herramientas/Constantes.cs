using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herramientas
{
    public class Constantes
    {
        #region Clase
        public const string EstructuraClase = @"using System.Collections.Generic;
                                                using OpenQA.Selenium;
                                                using Herramientas;
                                                using System.Configuration;
                                                using System;

                                                namespace PruebaAutomatizacion
                                                    {
                                                        public class prueba : UriBuilder
                                                        {
	                                                    private static Funcionalidad metodo = new Funcionalidad();
	                                                    
                                                        public void metodoPrueba(Dictionary<string, string> DatosConfig){
                                                          
	                                                                ClaseDePrueba miPrueba = new ClaseDePrueba(); //Acá debe ir la clase que se creó
                                                                    metodo.IniciarPrueba(miPrueba,DatosConfig);
	                                                    }
                                                        }
   
                                                          public class ClaseDePrueba:Inicializar
                                                        {
       
                                                            private IWebDriver newDriver = null;
                                                            private static Funcionalidad Metodo;
                                                            private  Dictionary<int,Dictionary<string, string>> Diccionario = null;
                                                            private  Dictionary<string, string> Caso = null;
                                                            public Dictionary<string, string> Datos { get { return Caso; } set { value = Caso; } }
                                                            private string RutaLog = null;
                                                            private string WaitTime=ConfigurationManager.AppSettings[" + "\"TiempoDriver\"" + @"];
                                                            public int TiempoEspera { get { return Convert.ToInt32(WaitTime); } set { } }
                                                            private static Log EscribirLog = new Log();
                                                            public string Parametros = null;

                                                            public ClaseDePrueba() { }

                                                            public void setData(IWebDriver driver,Dictionary<int,Dictionary<string, string>> CasosPrueba,string Ruta,Dictionary<string,string> caso,string parametros)
                                                            {
                                                                 this.newDriver = driver;
                                                                 this.Diccionario = CasosPrueba;
                                                                 this.RutaLog = Ruta;
                                                                 this.Caso = caso;
                                                                 this.Parametros = parametros;
                                                            }
                                                             public void IniciarPagina(string Url)
                                                            {
                                                                try{
                                                                Metodo = new Funcionalidad(newDriver);
                                                                Metodo.AbrirVentana(Url);
                                                                List<string> objeto = DatoElemento();
                                                                if (Parametros==" + "\"true\"" + @") { 
                                                                //poner metodo que encuentra todo
                                                                Caso = ObtenerCaso(Diccionario,objeto);

                                                                    if (Caso != null && objeto!=null)
                                                                    {   //Acá se coloca el método que implementa la captura de los elementos de la página de prueba
                                                                        MetodoGeneral();
                                                                        EscribirLog.EscribirLog(" + "\"Caso de prueba \"+" + "Datos[" + "\"NoCaso\"" + @"]+" + "\" exitoso\"" + @", RutaLog);
                                                                    }
                                                                    else
                                                                    {
                                                                        EscribirLog.EscribirLog("+"\"Caso de prueba \"+"+ "Datos["+"\"NoCaso\""+"]+"+ "\" No Exitoso - No se encontraron datos de entrada\""+ @", RutaLog);
                                                                    }
                                                                }else
                                                                {
                                                                    MetodoGeneral();
                                                                    EscribirLog.EscribirLog(" + "\"Caso de prueba \"+" + "Datos[" + "\"NoCaso\"" + "]+" + "\" Exitoso\"" + @", RutaLog);
                                                                }
                                                                }
                                                                catch(Exception ex)
                                                                {
                                                                    string resultado = (Datos["+"\"ResEsperado\""+"] == "+"\"E\""+") ? "+"\"Caso de prueba \"+"+" Datos["+"\"NoCaso\""+"]+"+"\" Exitoso\""+":"+ "\"Caso de prueba \"+"+ "Datos["+"\"NoCaso\""+"]+"+ "\" Fallido:\"" + @"+ ex.Message;
                                                                    EscribirLog.EscribirLog(resultado, RutaLog);
                                                                 }
                                                            }
                                                            public Dictionary<string,string> ObtenerCaso(Dictionary<int,Dictionary<string,string>> Datos,List<string> objeto )
                                                            {
                                                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                                                for(int i=0;i<= Datos.Count; i++)
                                                                {
                                                                    if(objeto != null)
                                                                    {
                                                                        if (Datos[i][objeto[0]] == objeto[1])
                                                                           {
                                                                             return Datos[i];
                                                                           }
                                                                     }
                                                                 }
                                                                        return null;
                                                              }
                                                              {0}
                                                               }";
                                                            
   
        #endregion

        #region Generales
        #endregion
    }
}
