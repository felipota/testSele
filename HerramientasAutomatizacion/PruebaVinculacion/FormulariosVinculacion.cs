using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Herramientas;
using System.Configuration;
using System.Linq;

namespace PruebaVinculacion
{
    public class FormulariosVinculacion: Inicializar
    {
        #region Variables
        private IWebDriver newDriver = null;
        private  static Funcionalidad Metodo;
        private  Dictionary<int,Dictionary<string, string>> Diccionario = null;
        private  Dictionary<string, string> Caso = null;
        public Dictionary<string, string> Datos { get { return Caso; } set { value = Caso; } }
        private string RutaLog = null;
        private string WaitTime=ConfigurationManager.AppSettings["TiempoDriver"];
        public int TiempoEspera { get { return Convert.ToInt32(WaitTime); } set { } }
        private static Log EscribirLog = new Log();
        public string Parametros = null;
        /// <summary>
        /// Constructor de los formularios de vinculación
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public FormulariosVinculacion()
        {

        }

        /// <summary>
        /// Método implementado de la interfaz
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void setData(IWebDriver driver,Dictionary<int,Dictionary<string, string>> CasosPrueba,string Ruta,Dictionary<string,string> caso,string parametros)
        {
            this.newDriver = driver;
            this.Diccionario = CasosPrueba;
            this.RutaLog = Ruta;
            this.Caso = caso;
            this.Parametros = parametros;
        }
        /// <summary>
        ///  Método implementado de la interfaz para inicializar la página
        /// </summary>
        /// <param></param>
        /// Autor: Gabriel Herrera Z
        public void IniciarPagina(string Url)
        {
            Dictionary<string, string> DatoExp = new Dictionary<string, string>();
            DatoExp = Datos;
            try
            {
                Metodo = new Funcionalidad(newDriver);
                Metodo.AbrirVentana(Url);
                List<string> objeto = DatoElemento();

                if (Parametros=="true") { 
                    //poner metodo que encuentra todo
                    Caso = ObtenerCaso(Diccionario,objeto);

                    if (Caso != null && objeto!=null)
                    {   //Acá se coloca el método que implementa la captura de los elementos de la página de prueba
                        MetodoGeneral();
                        EscribirLog.EscribirLog(string.Format("Caso de prueba {0} exitoso", Caso["NoCaso"]), RutaLog);
                    }
                    else
                    {
                        EscribirLog.EscribirLog(string.Format("Caso de prueba {0} No Exitoso - No se encontraron datos de entrada", DatoExp["NoCaso"]), RutaLog);
                    }
                }else
                {
                    MetodoGeneral();
                    EscribirLog.EscribirLog(string.Format("Caso de prueba {0} exitoso", Datos["NoCaso"]), RutaLog);
                }
            }
            catch(Exception ex)
            {
                string resultado = "Caso de prueba {0} {1}";
                resultado = (Datos["ResEsperado"] == "E") ? string.Format(resultado, Datos["NoCaso"], "Exitoso") : string.Format(resultado, Datos["NoCaso"], "Fallido:") + ex.Message;
                EscribirLog.EscribirLog(resultado, RutaLog);
            } 

        }
        #endregion

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

        public List<string> DatoElemento()
        {
            //return Metodo.ValorElemento("Name","","","");
            return null;
        }




        public void MetodoGeneral()
        {
            Metodo.Retardo(2 * TiempoEspera);
            Metodo.Controlador(otp, "otp");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(productosofertados, "productosofertados");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(legal, "legal");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(finger, "finger");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(doc, "doc");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(basicdata, "basicdata");
            Metodo.Retardo(5 * TiempoEspera);
            Metodo.Controlador(productos, "productos");
            Metodo.Retardo(5 * TiempoEspera);
            Metodo.Controlador(aditionabasicdata, "aditionabasicdata");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(actividadeconomica, "actividadeconomica");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(referencias, "referencias");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(normativedata, "normativedata");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(tarjetaadicional, "tarjetaadicional");
            Metodo.Retardo(1 * TiempoEspera);
           // Metodo.Controlador(monedaextranjera, "monedaextranjera");
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Controlador(anexardocumentos, "anexardocumentos");
            Metodo.Retardo(5 * TiempoEspera);
            Metodo.Controlador(evident, "evident");
            Metodo.Retardo(10 * TiempoEspera);
            Metodo.CapturarVentana("15.2 Fin", Datos["Captura"], Datos["NoCaso"]);
        }
        public bool evident(bool ent)
        {
            Metodo.CapturarVentana("14.1 Evidente", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/div[2]/div/button", "Click", null);
            Metodo.Retardo(3 * TiempoEspera);
            Metodo.Accion("xpath", "/html/body/div[5]/div/div[3]/button[1]", "Click", null);
            Metodo.CapturarVentana("14.2 Evidente Fin", Datos["Captura"], Datos["NoCaso"]);
            return true;
        }
        public bool anexardocumentos(bool ent)
        {
            Metodo.CapturarVentana("13. Anexar Documentos", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/form/div/div[2]/div/button[2]", "Click", null);
            return true;
        }
        public bool monedaextranjera(bool ent)
        {
            Metodo.CapturarVentana("12. Operacion Moneda Extranjera", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/form/div/div[2]/div/button[2]", "Click", null);
            return true;
        }
        public bool tarjetaadicional(bool ent)
        {
            Metodo.CapturarVentana("11. Tarjeta Adicional", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/form/div/div[2]/div/button[2]", "Click", null);
            return true;
        }
        public bool normativedata(bool ent)
        {
            Metodo.CapturarVentana("10. Datos Normativos", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/form/div/div[2]/div/button[2]", "Click", null);
            return true;
        }
        public bool referencias(bool ent)
        {
            Metodo.EncontrarElemento("Select", "0", Datos["kinship"]);
            Metodo.EncontrarElemento("name", "residenceLocalization", Datos["residenceLocalization"]);
            Metodo.Accion("name", "residenceLocalization", "TAB", null);
            Metodo.EncontrarElemento("id", "nameReference", Datos["nameReference"]);
            Metodo.EncontrarElemento("id", "phone", Datos["phoneReference"]);
            Metodo.EncontrarElemento("id", "cellphone", Datos["cellphone"]);
            Metodo.CapturarVentana("9. Referencias", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/form/div/div[2]/div/button[2]", "Click", null);

            return true;
        }
        public bool actividadeconomica(bool ent) {
            Metodo.EncontrarElemento("name", "companyLocalization", Datos["companyLocalization"]);
            Metodo.Accion("name", "companyLocalization", "TAB", null);
            Metodo.EncontrarElemento("Select", "0", Datos["contractType"]);
            Metodo.EncontrarElemento("id", "companyName", Datos["companyName"]);
            Metodo.EncontrarElemento("id", "phoneCompany", Datos["phoneCompany"]);
            Metodo.EncontrarElemento("id", "extensionCompany", Datos["extensionCompany"]);
            Metodo.EncontrarElemento("id", "companyAddress", Datos["companyAddress"]);
            Metodo.EncontrarElemento("id", "actualCharge", Datos["actualCharge"]);
            Metodo.CapturarVentana("8. Actividad Economica", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/form/div/div[2]/div/button[2]", "Click", null);
            return true;
        }
        public bool aditionabasicdata(bool ent)
        {
            Metodo.EncontrarElemento("id", "address", Datos["address"]);
            Metodo.EncontrarElemento("id", "district", Datos["district"]);
            Metodo.EncontrarElemento("id", "residencePhone", Datos["residencePhone"]);
            Metodo.EncontrarElemento("name", "eps", Datos["eps"]);
            Metodo.Accion("name", "eps", "TAB", null);
            Metodo.CapturarVentana("7. Informacion Basica Adicional", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/form/div/div[3]/div/button[2]", "Click", null);
            return true;
        }
        public bool productos(bool ent)
        {
            //pac
            Metodo.Accion("TAGNAME", "i", "Click", "23");
            //Cuenta corriente / sobregiro   
            Metodo.Accion("TAGNAME", "i", "Click", "22");
            //Credito de consumo
            Metodo.Accion("TAGNAME", "i", "Click", "21");
            //Cuenta de ahorro
            Metodo.Accion("TAGNAME", "i", "Click", "20");
            //tarjeta de credito
                Metodo.Accion("TAGNAME", "i", "Click", "19");      
                Metodo.Accion("TAGNAME", "i", "Click", "18");       
            //    Metodo.Accion("TAGNAME", "i", "Click", "17");       
            Metodo.CapturarVentana("6. Productos ofertados", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, null, "SCROLL", "700");
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[2]/div[4]/button", "Click", null);
            // Metodo.Retardo(1 * TiempoEspera);
            return true;
        }
        public bool basicdata(bool ent)
        {
            //IJavaScriptExecutor js = (IJavaScriptExecutor)newDriver;
            Metodo.EncontrarElemento("id", "name1", Datos["1nombre"]);
            Metodo.EncontrarElemento("id", "name2", Datos["2nombre"]);
            Metodo.EncontrarElemento("id", "surname1", Datos["1apellido"]);
            Metodo.EncontrarElemento("id", "surname2", Datos["2apellido"]);
            Metodo.CapturarVentana("5.1. Basicasic data", Datos["Captura"], Datos["NoCaso"]);
            Metodo.EncontrarElemento("xpath", "//*[@id=\"datetimepicker1\"]/input", Datos["fchnacimiento"]);
            Metodo.EncontrarElemento("name", "birthLocalization", Datos["ciudadnacimiento"]);
            //Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion("name", "birthLocalization", "TAB", null);
            Metodo.EncontrarElemento("name", "residenceLocalization", Datos["ciudadresidencia"]);
          //  Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion("name", "residenceLocalization", "TAB", null);
            Metodo.Accion(null, null, "SCROLL", "300");
            Metodo.EncontrarElemento("xpath", "//*[@id=\"datetimepicker2\"]/input", Datos["fchexpedicion"]);
            Metodo.EncontrarElemento("name", "expeditionLocalization", Datos["ciudadexpedicion"]);
            //Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion("name", "expeditionLocalization", "TAB", null);
            switch (Datos["genero"])
            {
                case "M":
                    {
                        Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div/div[2]/form/div/div[2]/div[1]/div[2]/div/div/div[10]/div/fb-control-condition/div/div[1]/label", "Click", null);
                        break;
                    }
            }
            Metodo.EncontrarElemento("id", "cellphone", Datos["celular"]);
            //Metodo.EncontrarElemento("name", "nationality", Datos["nacionalidad"]);
            //Metodo.Retardo(1 * TiempoEspera);
            //Metodo.Accion("name", "nationality", "TAB", null);
            Metodo.EncontrarElemento("xpath", "//*[@id=\"product\"]/div/div/div[1]/div/div[2]/form/div/div[2]/div[1]/div[2]/div/div/div[13]/div/div/fb-control-email/div/div[1]/div[1]/input", Datos["email1"]);
            
            Metodo.EncontrarElemento("xpath", "//*[@id=\"product\"]/div/div/div[1]/div/div[2]/form/div/div[2]/div[1]/div[2]/div/div/div[13]/div/div/fb-control-email/div/div[1]/div[3]/input", Datos["email2"]);
            
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div/div[2]/form/div/div[2]/div[1]/div[2]/div/div/div[13]/div/div/fb-control-email/div/div[1]/div[3]/input", "TAB", null);
            Metodo.CapturarVentana("5.2. Basicasic data", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, null, "SCROLL", "500");
            Metodo.EncontrarElemento("Select", "0", Datos["actividad"]);
            int antiguedad = Int32.Parse(Datos["antiguedad"]);
            for(int x = 0; x < antiguedad; x++)
            {
                Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div/div[2]/form/div/div[2]/div[2]/div[2]/div/div/div[2]/div/fb-control-twonumeric/div/div[1]/div[1]/div[2]/button", "CLICK", null);
                
            }
            if (Datos["empleadofalabella"] == "SI")//empleado falabella
            {
                Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div/div[2]/form/div/div[2]/div[2]/div[2]/div/div/div[3]/div/fb-control-condition/div/div[1]/label", "CLICK", null);
                
            }

            Metodo.EncontrarElemento("id", "mainActivityIncome", Datos["ingresosprincipal"]);
            Metodo.EncontrarElemento("id", "otherIncome", Datos["ingresosotros"]);
            Metodo.EncontrarElemento("id", "otherIncomeDescription", Datos["otherIncomeDescription"]);
            Metodo.EncontrarElemento("id", "CEActivityIncome", Datos["expend"]);
            Metodo.EncontrarElemento("id", "otherExpend", Datos["otherExpend"]);
            Metodo.EncontrarElemento("id", "otherExpendDescription", Datos["otherExpendDescription"]);
            Metodo.Accion(null, null, "SCROLL", "900");
            Metodo.EncontrarElemento("id", "totalAssets", Datos["totalAssets"]);
            Metodo.EncontrarElemento("id", "totalLiabilities", Datos["totalLiabilities"]);
            Metodo.EncontrarElemento("Select", "1", Datos["estadocivil"]);
            Metodo.CapturarVentana("5.3. Basicasic data", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div/div[2]/form/div/div[3]/div/button", "CLICK", null);

            return true;
        }
        public bool doc(bool ent)
        {
            Metodo.CapturarVentana("4.1. Doc Verification", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div/div/form/div/div[1]/div/div[1]/div[2]/div/div[1]/div/div[3]/div/button", "Click", null);
            Metodo.Retardo(1 * TiempoEspera);
            Metodo.CapturarVentana("4.2. Doc Verification", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div/div/form/div/div[2]/div/button[2]", "Click", null);
            return true;
        }
        public bool finger(bool ent)
        {
            Metodo.EncontrarElemento("Select", "0", "CEDULA DE CIUDADANIA");
            Metodo.EncontrarElemento("id", "documentId", Datos["cedula"]);
            Metodo.CapturarVentana("3. Finger", Datos["Captura"], Datos["NoCaso"]);
            //Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div/form/div/div[1]/div/div[2]/div/div[2]", "Click", null);
            return true;
        }
        public bool legal(bool ent)
        {
            Metodo.CapturarVentana("2.1. terminos y condiciones", Datos["Captura"], Datos["NoCaso"]);
           // Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion(null, null, "SCROLL", "700");
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[3]/div/label", "Click", null);
            Metodo.CapturarVentana("1.2. terminos y condiciones checked", Datos["Captura"], Datos["NoCaso"]);
           // Metodo.Retardo(1 * TiempoEspera);
            Metodo.Accion("xpath", "//*[@id=\"product\"]/div/div/div[1]/div[4]/button[2]", "Click", null);
            return true;
        }
        public bool productosofertados(bool ent)
        {
            if (Datos["TC"] == "1")
            {
                Metodo.Accion("TAGNAME", "i", "Click", "17");       //tarjeta de credito
            }
            if (Datos["CA"] == "1")
            {
                Metodo.Accion("TAGNAME", "i", "Click", "18");       //Cuenta de ahorro
            }
            if (Datos["CC"] == "1")
            {
                Metodo.Accion("TAGNAME", "i", "Click", "19");       //Credito de consumo
            }
            if (Datos["CCR"] == "1")
            {
                Metodo.Accion("TAGNAME", "i", "Click", "20");       //Cuenta corriente
            }
            if (Datos["PAC"] == "1")
            {
                Metodo.Accion("TAGNAME", "i", "Click", "21");       //pac
            }
            Metodo.CapturarVentana("1. Productos lista", Datos["Captura"], Datos["NoCaso"]);
            Metodo.Accion(null, null, "SCROLL", "700");
            Metodo.Accion("Id", "W_PROD_DISP_STEP_6", "Click", null);
           // Metodo.Retardo(1 * TiempoEspera);
            return true;
        }
        public bool otp(bool ent)
        {
            Metodo.Accion("xpath", "//*[@id=\"nav-accordion\"]/li[2]/a", "Click",null );
            Metodo.Accion("xpath", "//*[@id=\"nav-accordion\"]/li[2]/ul/li[1]/a", "Click", null);
          
            //Metodo.CapturarVentana("NuevaAdmisiones Solicitud", Datos["Captura"], Datos["NoCaso"]);
            // Metodo.EncontrarElemento("id", "CodHu", Datos["CodHu"]);
            // Metodo.EncontrarElemento("Select", "0", Datos["Trans"]);
            // Metodo.EncontrarElemento("id", "Nombre", Datos["Nombre"]);
            // Metodo.EncontrarElemento("id", "MontoMinimo", Datos["MonMinimo"]);
            // Metodo.EncontrarElemento("id", "CuotasMinimo", Datos["CuoMinimo"]);
            // Metodo.EncontrarElemento("id", "CuotasMaximo", Datos["CuoMaximo"]);
            // Metodo.EncontrarElemento("id", "TasaPromocional", Datos["TasaProm"]);
            // Metodo.EncontrarElemento("xpath", "/html/body/app-root/div[1]/div/app-visualize-form/app-dynamic-form/div[1]/div[2]/div/form/div[1]/div[8]/app-control/div/div/div/div/input", Datos["FechaIni"]);
            // Metodo.EncontrarElemento("xpath", "/html/body/app-root/div[1]/div/app-visualize-form/app-dynamic-form/div[1]/div[2]/div/form/div[1]/div[9]/app-control/div/div/div/div/input", Datos["FechaFin"]);
            // Metodo.Retardo(1 * TiempoEspera);
            // Metodo.EncontrarElemento("xpath", "/html/body/app-root/div[1]/div/app-visualize-form/app-dynamic-form/div[1]/div[2]/div/form/div[1]/div[10]/app-control/div/div/ngb-timepicker/fieldset/div/div[5]/input", Datos["HoraIn"]);
            // Metodo.Retardo(1 * TiempoEspera);
            // Metodo.EncontrarElemento("xpath", "/html/body/app-root/div[1]/div/app-visualize-form/app-dynamic-form/div[1]/div[2]/div/form/div[1]/div[11]/app-control/div/div/ngb-timepicker/fieldset/div/div[5]/input", Datos["HoraFin"]);
            // Metodo.Retardo(1 * TiempoEspera);
            // Metodo.Accion("xpath", "/html/body/app-root/div[1]/div/app-visualize-form/app-dynamic-form/div[1]/div[2]/div/form/div[1]/div[11]/app-control/div/div/ngb-timepicker/fieldset/div/div[5]/input", "TAB", null);
            // Metodo.EncontrarElemento("id", "prueba", @"C:\Documento\prueba.txt");
            // Metodo.Accion("TAGNAME", "Button", "Click", "17");
            // Metodo.Retardo(1 * TiempoEspera);
            // Metodo.Accion(null, "CuotasMaximo", "SCROLL-ELEMENT", null);
            // Metodo.CapturarVentana("Formas Dinamicas", Datos["Captura"], Datos["NoCaso"]);


            // Metodo.Retardo(1 * TiempoEspera);

            return true;
        }
    }
}
