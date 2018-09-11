using System.Collections.Generic;
using OpenQA.Selenium;
using Herramientas;
using System.Configuration;
using System;

namespace PruebaAutomatizacion
{
    public class prueba : UriBuilder
    {
        private static Funcionalidad metodo = new Funcionalidad();

        public void metodoPrueba(Dictionary<string, string> DatosConfig)
        {

            ClaseDePrueba miPrueba = new ClaseDePrueba(); //Acá debe ir la clase que se creó
            metodo.IniciarPrueba(miPrueba, DatosConfig);
        }
    }

    public class ClaseDePrueba : Inicializar
    {

        private IWebDriver newDriver = null;
        private static Funcionalidad Metodo;
        private Dictionary<int, Dictionary<string, string>> Diccionario = null;
        private Dictionary<string, string> Caso = null;
        public Dictionary<string, string> Datos { get { return Caso; } set { value = Caso; } }
        private string RutaLog = null;
        private string WaitTime = ConfigurationManager.AppSettings["TiempoDriver"];
        public int TiempoEspera { get { return Convert.ToInt32(WaitTime); } set { } }
        private static Log EscribirLog = new Log();
        public string Parametros = null;

        public ClaseDePrueba() { }

        public void setData(IWebDriver driver, Dictionary<int, Dictionary<string, string>> CasosPrueba, string Ruta, Dictionary<string, string> caso, string parametros)
        {
            this.newDriver = driver;
            this.Diccionario = CasosPrueba;
            this.RutaLog = Ruta;
            this.Caso = caso;
            this.Parametros = parametros;
        }
        public void IniciarPagina(string Url)
        {
            try
            {
                Metodo = new Funcionalidad(newDriver);
                Metodo.AbrirVentana(Url);
                List<string> objeto = DatoElemento();
                if (Parametros == "true")
                {
                    //poner metodo que encuentra todo
                    Caso = ObtenerCaso(Diccionario, objeto);

                    if (Caso != null && objeto != null)
                    {   //Acá se coloca el método que implementa la captura de los elementos de la página de prueba
                        MetodoGeneral();
                        EscribirLog.EscribirLog("Caso de prueba " + Datos["NoCaso"] + " exitoso", RutaLog);
                    }
                    else
                    {
                        EscribirLog.EscribirLog("Caso de prueba " + Datos["NoCaso"] + " No Exitoso - No se encontraron datos de entrada", RutaLog);
                    }
                }
                else
                {
                    MetodoGeneral();
                    EscribirLog.EscribirLog("Caso de prueba " + Datos["NoCaso"] + " Exitoso", RutaLog);
                }
            }
            catch (Exception ex)
            {
                string resultado = (Datos["ResEsperado"] == "E") ? "Caso de prueba " + Datos["NoCaso"] + " Exitoso" : "Caso de prueba " + Datos["NoCaso"] + " Fallido:" + ex.Message;
                EscribirLog.EscribirLog(resultado, RutaLog);
            }
        }
        public Dictionary<string, string> ObtenerCaso(Dictionary<int, Dictionary<string, string>> Datos, List<string> objeto)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i <= Datos.Count; i++)
            {
                if (objeto != null)
                {
                    if (Datos[i][objeto[0]] == objeto[1])
                    {
                        return Datos[i];
                    }
                }
            }
            return null;
        }
        public List<string> DatoElemento()
        {
            //return Metodo.ValorElemento("Name","","","");
            return null;
        }

        /// <summary>
        ///  M�todo que se encarga de llenar los formularios de la p�gina
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void MetodoGeneral()
        {
            Metodo.Retardo(25 * TiempoEspera);
            Metodo.Controlador(Autenticacion, "Autenticacion");

            //---------------------------Productos--------------------------------------
            Metodo.Controlador(Producto, "Producto");
            //---------------- Formulario Datos b�sicos cliente ------------------------
            Metodo.Controlador(DatosBasicos, "DatosBasicos");
            //-------------------- Datos b�sicos ingresos ------------------------------
            Metodo.Controlador(DatosBasicosIngresos, "DatosBasicosIngresos");
            //------------------- Datos b�sicos adicionales ----------------------------
            Metodo.Controlador(DatosBasicosAdicionales, "DatosBasicosAdicionales");
            //----------------------- Actividad econ�mica ------------------------------
            Metodo.Controlador(ActividadEconomica, "ActividadEconomica");
            //-------------------------- Referencia ------------------------------------
            Metodo.Controlador(Referencias, "Referencias");
            //----------------------- Datos extranjeros --------------------------------
            Metodo.Controlador(DatosExtranjeros, "DatosExtranjeros");
            //----------------------- Moneda Extranjera --------------------------------
            Metodo.Controlador(MonedaExtranjera, "MonedaExtranjera");

            //Autorizaciones y finalizaci�n de solicitu
            Metodo.Retardo(3 * TiempoEspera);
            Metodo.Accion("Id", "chkA", "Click", null);
            Metodo.Accion("XPath", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/div/div[3]/div/button", "Click", null);
            Metodo.Retardo(3 * TiempoEspera);
            Metodo.CapturarVentana("Final", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("Xpath", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/div[2]/div/button", "Click", null);
            Metodo.Retardo(3 * TiempoEspera);
            Metodo.Controlador(CerrarSesion, "Cerrar");
        }
        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de autenticacion
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool Autenticacion(bool ent)
        {
            string Usuario = ConfigurationManager.AppSettings["Usuario"];
            string Contrasena = ConfigurationManager.AppSettings["Contrasena"];
            Metodo.EncontrarElemento("name", "AUTENTICAR_ID_ASE", Usuario);
            Metodo.EncontrarElemento("name", "AUTENTICAR_CLAVE", Contrasena);
            Metodo.EncontrarElemento("name", "OFI_ASES_value", "HOME");
            Metodo.Accion("name", "OFI_ASES_value", "TAB", null);
            Metodo.CapturarVentana("Autenticacion", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("classname", "btn-falabella", "Click", null);
            return true;
        }

        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de productos
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool Producto(bool ent)
        {
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion("XPATH", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/div[2]/div/div[1]/div[1]", "Click", null);
            Metodo.Accion("XPATH", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/div[3]/div/a[1]/button", "Click", null);
            //Metodo.Retardo(4 * TiempoEspera);
            //Metodo.Accion("XPATH", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]", "Scroll-fin",null);
            Metodo.Accion("Id", "formAut", "Scroll-fin", null);
            //Metodo.Accion(null, null, "SCROLL", "150");
            //-------------------CheckBox------------------------------------------------
            Metodo.Accion("XPATH", "//*[@id=\"chkA\"]", "Click", null);
            Metodo.Accion("XPATH", "//*[@id=\"formAut\"]/div/div/div[3]/div/a/button", "Click", null);
            //Metodo.Retardo(1 * TiempoEspera);
            return true;
            //----------------------------------------------------------------------------
        }
        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de datos basico del cliente
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool DatosBasicos(bool ent)
        {

            Metodo.Retardo(2);
            Metodo.EncontrarElemento("Select", "0", Datos["TipId"]);
            //Metodo.Retardo(2 * TiempoEspera);
            Metodo.EncontrarElemento("name", "ING_CLI_NUM_ID_CLIE", Datos["NoId"]);
            Metodo.Accion("name", "ING_CLI_NUM_ID_CLIE", "TAB", null);
            //Metodo.Retardo(2 * TiempoEspera);
            Metodo.EncontrarElemento("name", "DATO_BAS_PRI_APE_CLI", Datos["PApellido"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_SEG_APE_CLI", Datos["SApellido"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_PRI_NOM_CLI", Datos["PNombre"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_SEG_NOM_CLI", Datos["SNombre"]);
            //Metodo.Retardo(1 * TiempoEspera);
            if (Datos["Genero"] == "M")
            {
                Metodo.Accion("name", "DATO_BAS_GENERO_2", "Click", null);
            }
            else
            {
                Metodo.Accion("name", "DATO_BAS_GENERO_1", "Click", null);
            }

            Metodo.EncontrarElemento("xpath", "//*[@id=\"9\"]", Datos["FechaNacimiento"]);
            Metodo.CapturarVentana("DatosBasicos-1", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "25_2", "SCROLL-ELEMENT", null);
            Metodo.EncontrarElemento("name", "DATO_BAS_NACION_value", Datos["Nacionalidad"]);
            //Metodo.Retardo(5 * TiempoEspera);
            Metodo.Accion("name", "DATO_BAS_NACION_value", "TAB", null);
            Metodo.EncontrarElemento("name", "DATO_BAS_CIU_NACI_value", Datos["CiudadNaci"]);
            Metodo.Accion("name", "DATO_BAS_CIU_NACI_value", "TAB", null);
            Metodo.EncontrarElemento("name", "DATO_BAS_FECH_EXP_DOC", Datos["FechaExpe"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_DPTO_RESID_value", Datos["CiudadResi"]);
            //Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion("name", "DATO_BAS_DPTO_RESID_value", "TAB", null);
            Metodo.EncontrarElemento("name", "DATO_BAS_TEL_CEL", Datos["Celular"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_CORREO_SEG_C", Datos["CorreoPP"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_CORREO_SEG_value", Datos["CorreoSP"]);
            Metodo.EncontrarElemento("select", "1", Datos["Ocupacion"]);
            Metodo.CapturarVentana("DatosBasicos-2", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "21", "SCROLL-ELEMENT", null);
            switch (Datos["Ocupacion"])
            {
                case "EMPLEADO PRIVADO":
                    EmpleadoPrivado();
                    break;
                case "MILITAR":
                    Militar();
                    break;
                case "POLICIA":
                    Militar();
                    break;
                case "PENSIONADO":
                    Pensionado();
                    break;
                default:
                    Otros();
                    break;
            }
            Metodo.Retardo(6 * TiempoEspera);
            return true;

        }
        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de datos b�sicos de ingreso
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool DatosBasicosIngresos(bool ent)
        {
            Metodo.EncontrarElemento("name", "DATO_BAS_ING_ACT_PRI", Datos["ActividadP"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_OTR_ING", Datos["OtroIn"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_OTR_EGRE", Datos["OtroEgre"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_COS_GAST_ACT_PRI", Datos["GastoPer"]);
            Metodo.EncontrarElemento("name", "INF_FIN_DESC_OTR_EGR", Datos["DescOtroEgre"]);
            Metodo.EncontrarElemento("name", "INF_FIN_DESC_OTR_ING", Datos["DescIng"]);
            Metodo.CapturarVentana("Ingresos-1", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "44", "SCROLL-ELEMENT", null);
            Metodo.EncontrarElemento("name", "DATO_BAS_TOTAL_ACT", Datos["TotalAct"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_TOTAL_PAS", Datos["TotalPas"]);
            Metodo.CapturarVentana("Ingresos-2", Datos["Captura"], Datos["NoCaso"]);
            EstadoCivil();
            Metodo.Retardo(3 * TiempoEspera);
            Metodo.Accion("XPATH", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/div/div/div/div[3]/div/div/div/div[2]/div/div[1]", "Click", null);
            Metodo.Accion("XPath", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[2]/div", "Scroll-fin", null);
            Metodo.Accion("Xpath", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/div/div/div/div[4]/div/a/button", "Click", null);
            return true;

        }
        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de datos b�sicos adicionales
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool DatosBasicosAdicionales(bool ent)
        {
            string DiaPago = "5";
            string Corres = "0";
            Metodo.Retardo(2 * TiempoEspera);
            Metodo.EncontrarElemento("name", "DATO_BAS_DIR_RESI", Datos["DirecResi"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_BARRIO", Datos["BarrioResi"]);
            Metodo.EncontrarElemento("name", "DATO_BAS_TEL_FIJ", Datos["TelResi"]);
            //if (Datos["CuentaNo"] == "true")
            //{
            //    Metodo.Accion("name","ACT_ECO_CUE_NOM_2", "Click",null);
            //}
            //if (Datos["Gmf"] == "true")
            //{
            //    Metodo.Accion("name", "EXC_GMF_2", "Click",null);
            //}
            switch (Datos["Corres"])
            {
                case "OF":
                    Corres = "1";
                    break;
                case "EN":
                    Corres = "2";
                    break;
            }

            switch (Datos["DiaPago"])
            {
                case "5":
                    DiaPago = "0";
                    break;
                case "10":
                    DiaPago = "1";
                    break;
                case "15":
                    DiaPago = "2";
                    break;
                case "20":
                    DiaPago = "3";
                    break;
                case "25":
                    DiaPago = "4";
                    break;
            }

            Metodo.Accion("TagName", "Button", "Click", DiaPago);
            Metodo.Accion("TagName", "a", "Click", Corres);
            Metodo.EncontrarElemento("name", "DATO_ADI_NOM_EPS_value", Datos["Eps"]);
            Metodo.Accion("name", "DATO_ADI_NOM_EPS_value", "TAB", null);
            Metodo.CapturarVentana("DatosAdicionales", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("XPATH", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/form/div[2]/div[2]/button", "Click", null);
            return true;
        }
        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de actividad econ�mica
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool ActividadEconomica(bool ent)
        {

            Metodo.Retardo(2 * TiempoEspera);
            switch (Datos["Ocupacion"])
            {
                case "MILITAR":
                    ActividadEconomicaMilitar();
                    Metodo.CapturarVentana("ActividadEconomica", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion("TAGNAME", "Button", "Click", "2");
                    break;
                case "POLICIA":
                    ActividadEconomicaMilitar();
                    Metodo.CapturarVentana("ActividadEconomica", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion("TAGNAME", "Button", "Click", "2");
                    break;
                case "PRESTADOR DE SERVICIOS":
                    ActividadEconoPrestador();
                    Metodo.CapturarVentana("ActividadEconomica", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion("TAGNAME", "Button", "Click", "2");
                    break;
                case "TRANSPORTADOR":
                    ActividadEconoTranspor();
                    Metodo.Accion("TAGNAME", "Button", "Click", "2");
                    break;
                case "INDEPENDIENTE":
                    ActividadEconoIndepe();
                    Metodo.Accion("TAGNAME", "Button", "Click", "2");
                    break;
                case "RENTISTA":
                    ActividadRentista();
                    Metodo.Accion("TAGNAME", "Button", "Click", "4");
                    break;
                case "EMPLEADO PRIVADO":
                    ActividadEconoEmpleado();
                    Metodo.Accion("TAGNAME", "Button", "Click", "2");
                    break;
                case "EMPLEADO P�BLICO":
                    ActividadEconoEmpleado();
                    Metodo.Accion("TAGNAME", "Button", "Click", "2");
                    break;
                case "PENSIONADO":
                    Metodo.EncontrarElemento("name", "ACT_ECO_NOM_EMP", Datos["NombreEmp"]);
                    Metodo.CapturarVentana("ActividadEconomica", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion("TAGNAME", "Button", "Click", "2");
                    break;
            }
            Metodo.Retardo(8 * TiempoEspera);
            return true;
        }
        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de referencias
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool Referencias(bool ent)
        {

            Metodo.EncontrarElemento("name", "REF_NOM_FAM", Datos["NombreRef"]);
            Metodo.EncontrarElemento("name", "REF_TEL_FAM", Datos["NoRef"]);
            Metodo.EncontrarElemento("select", "0", Datos["Parentesco"]);
            Metodo.EncontrarElemento("name", "REF_CEL_FAM", Datos["CelRef"]);
            Metodo.EncontrarElemento("name", "REF_CIU_FAM_value", Datos["CiudadRef"]);
            Metodo.Accion("name", "REF_CIU_FAM_value", "TAB", null);
            Metodo.CapturarVentana("Referencia", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("XPath", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/form/div[2]/div[2]/button", "Click", null);
            return true;
        }
        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de datos extranjeros
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool DatosExtranjeros(bool ent)
        {
            Metodo.Retardo(5 * TiempoEspera);

            if (Datos["Pep"] == "true")
            {
                Metodo.Accion("Name", "PREGUNTA2_2", "Click", null);
                if (Datos["AcPep"] == "true")
                {
                    Metodo.EncontrarElemento("name", "DAT_NORM_FECH_INI_PRE_DOS", Datos["FInPep"]);
                    Metodo.CapturarVentana("DatosExtranjeros-1", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion(null, "217", "SCROLL-ELEMENT", null);
                }
                else
                {
                    Metodo.Accion("Name", "DAT_EXT_ACT_PEP_1", "Click", null);
                    Metodo.EncontrarElemento("name", "DAT_NORM_FECH_INI_PRE_DOS", Datos["FInPep"]);
                    Metodo.EncontrarElemento("name", "DAT_NORM_FECH_FIN_PRE_DOS", Datos["FFinPep"]);
                    Metodo.CapturarVentana("DatosExtranjeros-2", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion(null, "217", "SCROLL-ELEMENT", null);
                }
            }
            //Relacion Persona Expuesta
            if (Datos["RelaPep"] == "true")
            {
                IWebElement Button6 = Metodo.EncontrarElemento("Name", "PREGUNTA7_2", null);
                Metodo.Accion("Name", "PREGUNTA7_2", "Click", null);
                Metodo.EncontrarElemento("Select", "0", Datos["TipoRela"]);
                Metodo.EncontrarElemento("name", "DAT_EXT_CARGO", Datos["CaAcPep"]);
                if (Datos["ActRelaPep"] == "true")
                {
                    Metodo.EncontrarElemento("name", "DAT_NORM_FECH_INI_PRE_SIETE", Datos["FeIniPep"]);
                    Metodo.CapturarVentana("DatosExtranjeros-3", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion(null, "138_1", "SCROLL-ELEMENT", null);

                }
                else
                {
                    Metodo.Accion("Name", "DAT_EXT_ACT_PEP_RELA_1", "Click", null);
                    Metodo.EncontrarElemento("name", "DAT_NORM_FECH_INI_PRE_SIETE", Datos["FeIniPep"]);
                    Metodo.EncontrarElemento("name", "DAT_NORM_FECH_FIN_PRE_SIETE", Datos["FeFinPep"]);
                    Metodo.CapturarVentana("DatosExtranjeros-4", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion(null, "138_1", "SCROLL-ELEMENT", null);
                }
            }
            //Otra nacionalidad
            if (Datos["OtraNacio"] == "true")
            {
                Metodo.Accion("Name", "PREGUNTA_2", "Click", null);
                Metodo.EncontrarElemento("name", "DATO_BAS_NACION2_value", Datos["CualNacio"]);
                Metodo.Accion("Name", "DATO_BAS_NACION2_value", "TAB", null);
                Metodo.CapturarVentana("DatosExtranjeros-5", Datos["Captura"], Datos["NoCaso"]);
            }


            //Residencia fiscal
            if (Datos["ResiFiscal"] == "true")
            {
                Metodo.Accion("Name", "DAT_EXT_NOR_RES_FIS_2", "Click", null);
                Metodo.EncontrarElemento("name", "DAT_EXT_NOR_PAI_value", Datos["CualResi"]);
                Metodo.Accion("Name", "DAT_EXT_NOR_PAI_value", "TAB", null);
                Metodo.EncontrarElemento("name", "DAT_EXTRANJ_TIN", Datos["NoTin"]);
                Metodo.CapturarVentana("DatosExtranjeros-6", Datos["Captura"], Datos["NoCaso"]);
                Metodo.Accion(null, "140_1", "SCROLL-ELEMENT", null);
            }

            Metodo.Accion("Xpath", "/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/form/div[2]/div[2]/button", "Click", null);
            Metodo.Retardo(4 * TiempoEspera);
            return true;

        }
        /// <summary>
        ///  M�todo que captura las etiquetas de la pantalla de moneda extranjeros
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool MonedaExtranjera(bool ent)
        {

            if (Datos["TranMoExt"] == "true")
            {
                Metodo.Accion("Name", "PREGUNTA4_2", "Click", null);
                Metodo.EncontrarElemento("Select", "0", Datos["TipoTran"]);
                if (Datos["CuMoExtra"] == "true")
                {
                    if (Datos["TiMoExtra"] == "A")
                    {
                        Metodo.Accion("Name", "PREGUNTA6_1", "Click", null);
                    }
                    else
                    {
                        Metodo.Accion("Name", "PREGUNTA6_2", "Click", null);
                    }
                    Metodo.EncontrarElemento("name", "TME_CUENT", Datos["NoCuentExt"]);
                    Metodo.EncontrarElemento("name", "TME_BANC", Datos["Banco"]);
                    Metodo.EncontrarElemento("Select", "1", Datos["PaisMonExt"]);
                    Metodo.EncontrarElemento("NAME", "TME_CIU", Datos["CiudadExt"]);
                    Metodo.Accion("Name", "TME_CIU", "TAB", null);
                    Metodo.CapturarVentana("MonedaExtranjera-1", Datos["Captura"], Datos["NoCaso"]);
                    Metodo.Accion(null, "131", "SCROLL-ELEMENT", null);
                    Metodo.EncontrarElemento("Select", "2", Datos["Moneda"]);
                    Metodo.EncontrarElemento("name", "TME_MONT_PROM", Datos["MonProme"]);
                    Metodo.CapturarVentana("MonedaExtranjera-2", Datos["Captura"], Datos["NoCaso"]);
                }
            }
            newDriver.FindElement(By.XPath("/html/body/ion-pane/ion-nav-view/ion-view/ion-content/div[1]/div/form/div[2]/div[2]/button")).Click();
            return true;
        }
        /// <summary>
        ///  M�todo que captura el dom para la ocupaci�n empleado privado
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void EmpleadoPrivado()
        {
            int NumeroMeses = Convert.ToInt32(Datos["AntigMes"]);
            int NumeroAnos = Convert.ToInt32(Datos["AntigAno"]);
            int NumeroMesesE = Convert.ToInt32(Datos["AntOtraM"]);
            int NumeroAnosE = Convert.ToInt32(Datos["AntiOtraA"]);

            if (Datos["EmpFala"] == "true")
            {
                Metodo.Accion("TAGNAME", "button", "Click", "3");
            }
            if (NumeroAnos >= 1 || NumeroMeses > 6)
            {
                //Antiguedad en la actividad
                for (int i = 1; i <= NumeroAnos; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "5"); }
                for (int i = 1; i <= NumeroMeses; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "7"); }
                Metodo.EncontrarElemento("name", "CED_REF_ONLINE", Datos["NoCedRef"]);
                Metodo.Accion("TAGNAME", "button", "Click", "9");
            }
            else
            {
                //Antiguedad en la actividad
                for (int i = 1; i <= NumeroAnos; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "5"); }
                for (int i = 1; i <= NumeroMeses; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "7"); ; }
                //Duraci�n empresa anterior
                for (int i = 1; i <= NumeroAnosE; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "9"); }
                for (int i = 1; i <= NumeroMesesE; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "11"); }
                Metodo.EncontrarElemento("name", "ACT_ECO_FEC_RET", Datos["FechaRet"]);
                Metodo.EncontrarElemento("name", "CED_REF_ONLINE", Datos["NoCedRef"]);
                Metodo.Accion("TAGNAME", "button", "Click", "13");
            }
            Metodo.CapturarVentana("DatosBasicos-3", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Retardo(4 * TiempoEspera);

        }
        /// <summary>
        ///  M�todo que captura el dom para la ocupaci�n militar
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void Militar()
        {
            Metodo.EncontrarElemento("select", "2", Datos["FuerzaUsu"]);
            Metodo.Accion(null, "21", "SCROLL-ELEMENT", null);
            Metodo.EncontrarElemento("select", "3", Datos["RangoUsu"]);
            Otros();
        }
        /// <summary>
        ///  M�todo que captura el dom para la ocupaci�n pensionado
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void Pensionado()
        {
            int NumeroMeses = Convert.ToInt32(Datos["AntigMes"]);
            int NumeroAnos = Convert.ToInt32(Datos["AntigAno"]);
            for (int i = 1; i <= NumeroAnos; i++)
            { Metodo.Accion("TAGNAME", "button", "Click", "3"); }
            for (int i = 1; i <= NumeroMeses; i++)
            { Metodo.Accion("TAGNAME", "button", "Click", "5"); }
            Metodo.EncontrarElemento("name", "CED_REF_ONLINE", Datos["NoCedRef"]);
            Metodo.CapturarVentana("DatosBasicos-3", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("TAGNAME", "button", "Click", "7");
        }
        /// <summary>
        ///  M�todo que captura el dom para el resto de ocupaciones
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void Otros()
        {
            int NumeroMeses = Convert.ToInt32(Datos["AntigMes"]);
            int NumeroAnos = Convert.ToInt32(Datos["AntigAno"]);
            int NumeroMesesE = Convert.ToInt32(Datos["AntOtraM"]);
            int NumeroAnosE = Convert.ToInt32(Datos["AntiOtraA"]);
            if (NumeroAnos >= 1 || NumeroMeses > 6)
            {
                //Antiguedad en la actividad
                for (int i = 1; i <= NumeroAnos; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "3"); }
                for (int i = 1; i <= NumeroMeses; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "5"); }
                Metodo.EncontrarElemento("name", "CED_REF_ONLINE", Datos["NoCedRef"]);
                Metodo.CapturarVentana("DatosBasicos-3", Datos["Captura"], Datos["NoCaso"]);
                Metodo.Accion("TAGNAME", "button", "Click", "7");
            }
            else
            {
                //Antiguedad en la actividad
                for (int i = 1; i <= NumeroAnos; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "3"); }
                for (int i = 1; i <= NumeroMeses; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "5"); }
                //Duraci�n empresa anterior
                for (int i = 1; i <= NumeroAnosE; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "7"); }
                for (int i = 1; i <= NumeroMesesE; i++)
                { Metodo.Accion("TAGNAME", "button", "Click", "9"); }
                Metodo.EncontrarElemento("name", "ACT_ECO_FEC_RET", Datos["FechaRet"]);
                Metodo.EncontrarElemento("name", "CED_REF_ONLINE", Datos["NoCedRef"]);
                Metodo.CapturarVentana("DatosBasicos-3", Datos["Captura"], Datos["NoCaso"]);
                Metodo.Accion("TAGNAME", "button", "Click", "11");
            }
        }
        /// <summary>
        ///  M�todo que captura el dom para el estado civil del usuario
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void EstadoCivil()
        {
            Metodo.EncontrarElemento("select", "0", Datos["EstadoCiv"]);
            if (Datos["EstadoCiv"] == "CASADO/A" || Datos["EstadoCiv"] == "UNION LIBRE")
            {
                Metodo.Accion(null, "35", "SCROLL-ELEMENT", null);
                Metodo.EncontrarElemento("select", "1", Datos["TipoIdCony"]);
                Metodo.EncontrarElemento("name", "DATO_BAS_NUM_ID_CONY", Datos["NoIdCony"]);
                Metodo.EncontrarElemento("name", "DATO_BAS_PRI_NOM_CONY", Datos["PNombreC"]);
                Metodo.EncontrarElemento("name", "DATO_BAS_PRI_APE_CONY", Datos["PApellidoC"]);
                Metodo.EncontrarElemento("select", "2", Datos["ActiCony"]);
                Metodo.EncontrarElemento("name", "DATO_BAS_TEL_CEL_CONY", Datos["NoCelCony"]);
                Metodo.EncontrarElemento("name", "ING_PRI_CYG", Datos["IngCony"]);
            }
            Metodo.CapturarVentana("Ingresos-3", Datos["Captura"], Datos["NoCaso"]);

            Metodo.Accion("TAGNAME", "button", "Click", "2");
        }
        /// <summary>
        ///  M�todo que captura el dom para el estado civil del usuario
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void ActividadEconomicaMilitar()
        {
            Metodo.EncontrarElemento("name", "ACT_ECO_NOM_EMP", Datos["NombreEmp"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_DIR_EMP", Datos["DireccionEmp"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_TEL_EMP", Datos["NoTelEmp"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_EXT_EMP", Datos["Ext"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_DPTO_EMP_value", Datos["CiudadEmp"]);
            Metodo.Accion("name", "ACT_ECO_DPTO_EMP_value", "TAB", null);

        }
        /// <summary>
        ///  M�todo que captura el dom para el estado civil del usuario
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void ActividadEconoPrestador()
        {
            Metodo.EncontrarElemento("name", "ACT_ECO_CARG", Datos["CargoActual"]);
            ActividadEconomicaMilitar();

        }
        /// <summary>
        ///  M�todo que captura el dom para la actividad economica del transportador 
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void ActividadEconoTranspor()
        {
            ActividadEconomicaMilitar();
            Metodo.EncontrarElemento("select", "0", Datos["TipoVh"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_PLACA", Datos["Placa"]);
            Metodo.CapturarVentana("ActividadEconomica", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "48", "SCROLL-ELEMENT", null);
        }
        /// <summary>
        ///  M�todo que captura el dom para la actividad economica del empleado
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void ActividadEconoEmpleado()
        {
            ActividadEconoPrestador();
            Metodo.EncontrarElemento("select", "0", Datos["TipoContra"]);
            Metodo.CapturarVentana("ActividadEconomica", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "48", "SCROLL-ELEMENT", null);
        }
        /// <summary>
        ///  M�todo que captura el dom la atividad economica del independiente
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void ActividadEconoIndepe()
        {
            Metodo.EncontrarElemento("name", "ACT_ECO_NOM_NIT", Datos["Nit"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_DESC_ACT", Datos["DescActv"]);
            ActividadEconomicaMilitar();
            Metodo.EncontrarElemento("name", "ACT_ECO_NOM_PROVEE", Datos["Proveedor"]);
            Metodo.CapturarVentana("ActividadEconomica-1", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "51", "SCROLL-ELEMENT", null);
            Metodo.EncontrarElemento("name", "ACT_ECO_TEL_PROVEE", Datos["NoTelProv"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_EXT_PTOVEE", Datos["ExtProvee"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_CIU_PROVEE_value", Datos["CiudProv"]);
            Metodo.Accion("Name", "ACT_ECO_CIU_PROVEE_value", "TAB", null);
            Metodo.CapturarVentana("ActividadEconomica-2", Datos["Captura"], Datos["NoCaso"]);
        }
        /// <summary>
        ///  M�todo que captura el dom para las actividades del rentista
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void ActividadRentista()
        {
            //Arrendatario 1
            Metodo.EncontrarElemento("name", "ACT_ECO_ARRENDA", Datos["NombreAr"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_TEL_ARRENDA", Datos["NoTelAr"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_EXT_ARRENDA", Datos["ExtAr"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_CIU_ARRENDA_value", Datos["CiuAr"]);
            Metodo.Accion("Name", "ACT_ECO_CIU_ARRENDA_value", "TAB", null);
            Metodo.EncontrarElemento("name", "ACT_ECO_DIR_INMUE", Datos["BienesDir"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_VLR_COMER", Datos["ValComercial"]);
            Metodo.CapturarVentana("ActividadEconomica-1", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "190", "SCROLL-ELEMENT", null);
            //Arrendatario 2
            Metodo.EncontrarElemento("name", "ACT_ECO_ARRENDA2", Datos["NombreAr2"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_TEL_ARRENDA2", Datos["NoTelAr2"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_EXT_ARRENDA2", Datos["ExtAr2"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_CIU_ARRENDA2_value", Datos["CiuAr2"]);
            Metodo.Accion("Name", "ACT_ECO_CIU_ARRENDA2_value", "TAB", null);
            Metodo.EncontrarElemento("name", "ACT_ECO_DIR_INMUE2", Datos["BienesDir2"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_VLR_COMER2", Datos["ValComercial2"]);
            Metodo.CapturarVentana("ActividadEconomica-2", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "196", "SCROLL-ELEMENT", null);
            //Arrendatario 3
            Metodo.EncontrarElemento("name", "ACT_ECO_ARRENDA3", Datos["NombreAr3"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_TEL_ARRENDA3", Datos["NoTelAr3"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_EXT_ARRENDA3", Datos["ExtAr3"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_CIU_ARRENDA3_value", Datos["CiuAr3"]);
            Metodo.Accion("Name", "ACT_ECO_CIU_ARRENDA3_value", "TAB", null);
            Metodo.EncontrarElemento("name", "ACT_ECO_DIR_INMUE3", Datos["BienesDir3"]);
            Metodo.EncontrarElemento("name", "ACT_ECO_VLR_COMER3", Datos["ValComercial3"]);
            Metodo.CapturarVentana("ActividadEconomica-3", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "196", "SCROLL-ELEMENT", null);
            if (Datos["RentiCap"] == "true")
            {
                Metodo.Accion("Tagname", "Button", "Click", "1");
            }
            else
            {
                Metodo.Accion("Tagname", "Button", "Click", "0");
            }
            Metodo.EncontrarElemento("name", "TIP_INV_REN_CAP", Datos["TipInver"]);
            Metodo.EncontrarElemento("name", "MONT_RENT_CAP", Datos["MonInver"]);
            Metodo.EncontrarElemento("name", "ENT_RENT_CAP", Datos["EntiInver"]);
            Metodo.CapturarVentana("ActividadEconomica-4", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, "198", "SCROLL-ELEMENT", null);
        }
        /// <summary>
        ///  M�todo para cerrar la sesi�n
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public bool CerrarSesion(bool ent)
        {
            Metodo.Retardo(2 * TiempoEspera);
            Metodo.Accion("XPath", "/html/body/ion-pane/ion-nav-view/ion-view/ion-header-bar/header/div/div[3]/div/div[2]/div/i", "Click", "0");
            Metodo.Accion("XPath", "/html/body/ion-pane/ion-nav-view/ion-view/ion-header-bar/header/div/div[3]/div/div[1]/div[2]/div[1]", "Click", null);
            return true;
        }

    }
}