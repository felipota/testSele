using System;
using System.Linq;
using OpenQA.Selenium;
using System.Configuration;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.IO;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
namespace Herramientas
{
    public class Funcionalidad
    {

        #region Variables
        public static CargarArchivo Cargue = new CargarArchivo();
        private  ObtenerDatosBd ob = new ObtenerDatosBd();
        private static Log EscribirLog = new Log();
        public static Dictionary<int, Dictionary<string, string>> Datos = null;
        public static Dictionary<int, Dictionary<string, string>> DatosConfig = null;
        private string archivoConfig = ConfigurationManager.AppSettings["archivoConfig"].ToString();
        private string rutaDriver = ConfigurationManager.AppSettings["RutaDriver"];
        private string timeDriver = ConfigurationManager.AppSettings["TiempoDriver"];
        public string Nombre { get { return variable; } set { variable = value; } }
        public string NombrePagina { get { return variable; } set { variable = value; } }
        public string rutaLog { get { return nombreRutaLog; } set { nombreRutaLog = value; } }
        public int TiempoEspera { get { return Convert.ToInt32(timeDriver); } set { } }
        private IWebDriver newDriver;
        private static string variable;
        private static string nombreRutaLog;

        #endregion

        #region Métodos
        /// <summary>
        ///  Constructor vacío
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public Funcionalidad()
        {

        }
        /// <summary>
        ///  Constructor que recibe el driver de la sesión de Chrome
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public Funcionalidad(IWebDriver driver)
        {
            this.newDriver = driver;
        }
        /// <summary>
        ///  Método creado para cargar archivo de configuración
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void IniciarPrueba(Inicializar objeto, Dictionary<string, string> DatosConfig)
        {
           //DatosConfig = Cargue.CargaDeArchivo(archivoConfig,"Archivo de configuración");
           // for (int i = 0; i <= DatosConfig.Count - 1; i++)
            //{
                nombreRutaLog = ConfigurationManager.AppSettings["RutaLog"].ToString()+ DatosConfig["App"] + @"\" + DatosConfig["App"]+"-" + DateTime.Now.ToString("hhmmssFFF") + ".txt";
                variable = DatosConfig["App"];
                EjecutarCasos(objeto, DatosConfig);
            //}

        }
        /// <summary>
        ///  Método creado para generalizar inicializar la prueba sobre la aplicación
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void EjecutarCasos(Inicializar objeto, Dictionary<string, string>DatosConfig)
        {
            string folderApp =ConfigurationManager.AppSettings["RutaLog"] +variable;
            Directory.CreateDirectory(folderApp);

            if (!string.IsNullOrEmpty(DatosConfig["Query"])){
                Datos = OrganizarDatos(DatosConfig,folderApp);
            }else
            {
                Datos = Cargue.CargaDeArchivo(DatosConfig["RutaArchivo"],"Archivo origen de datos");
            }

            for (int i = 0; i <= Datos.Count - 1; i++)
            {
                //string folderName = folderApp+@"\CasoPrueba" + "-" + i+1;

                try
                {
                    //Se obtiene driver
                    newDriver = ObtenerDriver(DatosConfig);
                    //Se agregan datos 
                    objeto.setData(newDriver, Datos, rutaLog,Datos[i],DatosConfig["DatoEntrada"]);

                    objeto.IniciarPagina(DatosConfig["Url"]);

                    newDriver.Close(); newDriver = null;
                }
                catch (Exception ex)
                {
                    if (newDriver != null)
                        newDriver.Close(); newDriver = null;
                  
                }
            }
            if (newDriver != null)
                newDriver.Close();
        }
        /// <summary>
        ///  Método creado para seleccionar el driver a partir del archivo de configuración
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public IWebDriver ObtenerDriver(Dictionary<string, string> DatosConfig)
        {
            if (newDriver == null)
            {
                if (DatosConfig["Navegador"] == "C")
                {
                    ChromeOptions option = new ChromeOptions();
                    newDriver = new ChromeDriver(rutaDriver);/*, option, TimeSpan.FromMinutes(Convert.ToInt32(timeDriver)));*/
                }
                else if (DatosConfig["Navegador"] == "I")
                {
                    InternetExplorerOptions options = new InternetExplorerOptions();
                    options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    newDriver = new InternetExplorerDriver(rutaDriver, options, TimeSpan.FromMinutes(Convert.ToInt32(timeDriver)));
                }
                else { newDriver = new FirefoxDriver(rutaDriver); }
            }

            return newDriver;
        }

        public Dictionary<int, Dictionary<string, string>> OrganizarDatos(Dictionary<string, string> DatosConfig,string ruta)
        {
            Datos = Cargue.CargaDeArchivo(DatosConfig["RutaArchivo"], "Archivo de datos");
            Dictionary<int, Dictionary<string, string>> DatosBd = new Dictionary<int, Dictionary<string, string>>();
            string[] Campos = DatosConfig["Parametros"].Split('-');
            string[] Condicion = DatosConfig["Condiciones"].Split('-');
            string rutaLogBd = ruta + @"\ConsultaBasededatos-" + DateTime.Now.ToString("hhmmssFFF") + ".txt";
            string nuevoQuery = null;
            for (int i = 0; i <= Datos.Count - 1; i++)
            {
                string condiciones = "";

                for (int j=0; j<=Condicion.Length - 1; j++)
                {
                    condiciones = Condicion[j] + ":" + Datos[j][Condicion[j]] + "|"+condiciones;
                    nuevoQuery = string.Format(DatosConfig["Query"], Datos[j][Condicion[j]]);
                }
                DatosBd = ob.ConectarBd(nuevoQuery, DatosConfig["Parametros"]);
                if(DatosBd.Count != 0) { 
                        for(int k = 0; k <= DatosBd[0].Count - 1; k++)
                        {
                            Datos[i][Campos[k]] = DatosBd[0][Campos[k]];
                        }
                        EscribirLog.EscribirLogArray(rutaLogBd, DatosBd,Datos,condiciones);
                }
                else
                {
                    string noData = "No se encontraron datos para la(s) siguiente(s) condiciones-" + condiciones;
                    EscribirLog.EscribirLog(noData, rutaLogBd);
                }
            }
            return Datos;
        }
        /// <summary>
        ///  Método creado para generalizar las acciones en la página
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void Accion(string Id, string nombreId, string action, string posicion)
        {
            IWebElement Enter = EncontrarElemento(Id, nombreId, posicion);
            IJavaScriptExecutor js = (IJavaScriptExecutor)newDriver;
            switch (action.ToUpper())
            {
                case "TAB":
                    Thread.Sleep(2000);
                    Enter.SendKeys(Keys.Tab);
                    Thread.Sleep(2000);
                    break;
                case "CLICK":
                    Enter.Click();
                    break;
                case "SCROLL-FIN":
                    Thread.Sleep(1000);
                    js.ExecuteScript("arguments[0].scrollIntoView(false);", Enter);
                    break;
                case "SCROLL-ELEMENT":
                    js.ExecuteScript("document.getElementById('" + nombreId + "').scrollIntoView(true);");
                    break;
                case "SCROLL":
                    js.ExecuteScript("window.scrollTo(0,"+ posicion + ");");
                    break;


            }

        }
        /// <summary>
        ///  Método creado para generalizar la busqueda de los elementos del HTML
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public IWebElement EncontrarElemento(string by, string id, string value)
        {
            if (string.IsNullOrEmpty(by)) { by = "null"; };
            IWebElement elementNull = null;
            WebDriverWait WaitDriver = new WebDriverWait(newDriver, TimeSpan.FromSeconds(20));
            switch (by.ToUpper())
            {
                case "NAME":

                    IWebElement val = WaitDriver.Until<IWebElement>(d => d.FindElement(By.Name(id)));
                    if (!string.IsNullOrEmpty(value)) { val.SendKeys(value); }
                    return val;
                case "ID":

                    IWebElement val2 = WaitDriver.Until<IWebElement>(d => d.FindElement(By.Id(id)));
                    if (!string.IsNullOrEmpty(value)) { val2.SendKeys(value); }
                    return val2;
                case "XPATH":

                    IWebElement val3 = WaitDriver.Until<IWebElement>(d => d.FindElement(By.XPath(id)));
                    if (!string.IsNullOrEmpty(value)) { val3.SendKeys(value); }
                    return val3;
                case "CLASSNAME":

                    IWebElement val4 = WaitDriver.Until<IWebElement>(d =>d.FindElement(By.ClassName(id)));
                    if (!string.IsNullOrEmpty(value)) { val4.SendKeys(value); }
                    return val4;
                case "SELECT":

                    IList<IWebElement> ListaSelect = WaitDriver.Until<IList<IWebElement>>(d => d.FindElements(By.TagName(by)));
                    new SelectElement(ListaSelect[Convert.ToInt32(id)]).SelectByText(value);
                    return elementNull;
                case "TAGNAME":

                    IList<IWebElement> ListaElement = WaitDriver.Until<IList<IWebElement>>(d => d.FindElements(By.TagName(id)));
                    return ListaElement[Convert.ToInt32(value)];
                default:
                    return elementNull;
            }

        }
        /// <summary>
        ///  Método creado para generalizar las acciones en la página
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public List<string> ValorElemento(string By1,string Id1,string By2,string Id2)
        {
            //Dictionary<string, string> Dato = new Dictionary<string, string>();
            List<string> lista = new List<string>();
            IWebElement Lable = EncontrarElemento(By1, Id1,null);
            IWebElement Input = EncontrarElemento(By2, Id2, null);

            lista.Add(Lable.Text);
            lista.Add(Input.GetAttribute("value"));
            return lista;
        }
        /// <summary>
        ///  Método creado para capturar el popup de error cuando hay campos con errores
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void AbrirVentana(string url)
        {
            try
            {
                newDriver.Navigate().GoToUrl(url);
                newDriver.Manage().Window.Maximize();
               

            }
            catch (Exception ex)
            {
                EscribirLog.EscribirLog("Error abriendo ventana: " + ex.Message, rutaLog);

            }
        }
        /// <summary>
        ///  Método creado para capturar las pantallas de la aplicación
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void CapturarVentana(string formulario, string bandera,string NumCaso)
        {
            try
            {
                string CarpetaCaso = ConfigurationManager.AppSettings["RutaLog"]+@"\"+ NombrePagina + @"\Caso-"+NumCaso;

                if (bandera == "S")
                {
                    Directory.CreateDirectory(CarpetaCaso);
                    Screenshot ss = ((ITakesScreenshot)newDriver).GetScreenshot();
                    //Use it as you want now
                    string screenshot = ss.AsBase64EncodedString;
                    byte[] screenshotAsByteArray = ss.AsByteArray;
                    ss.SaveAsFile(CarpetaCaso + @"\" + formulario + ".png");
                    ss.ToString();
                }
            }
            catch (Exception ex)
            {
                EscribirLog.EscribirLog("Error capturando pantalla: " + ex.Message, rutaLog);
            }
        }
        /// <summary>
        ///  Método general para ejecutar try-catch
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void Controlador(Func<bool, bool> metodoGeneral, string formulario)
        {
            try
            {
                bool result = metodoGeneral(true);
            }
            catch (Exception ex)
            {
                string error = "Ha ocurrido un error en el formulario de " + formulario + ":" + ex.Message;
                EscribirLog.EscribirLog(error, rutaLog);
                throw new Exception(error);
            }
        }
        /// <summary>
        ///  Método para agregar tiempos de espera
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void Retardo(int segundos)
        {
            Thread.Sleep(segundos * 1000);
        }
        #endregion

    }
}
