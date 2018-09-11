using System.Collections.Generic;
using OpenQA.Selenium;

namespace Herramientas
{
    public interface Inicializar
    {
        void setData(IWebDriver driver,Dictionary<int, Dictionary<string, string>> CasosPrueba,string ruta,Dictionary<string,string>Dato,string Parametro);
        void IniciarPagina(string url);
    }
}
