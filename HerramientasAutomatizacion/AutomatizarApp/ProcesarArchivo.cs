using Herramientas;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace AutomatizarApp
{
    public class ProcesarArchivo
    {
        public void CrearInstancia(string codigo, Dictionary<string, string> DatosConfig)
        {
            try
            {
                CompilerParameters objParametros = new CompilerParameters(new string[] { "System.dll" })
                {
                    GenerateInMemory = true,
                    GenerateExecutable = false,
                    IncludeDebugInformation = false,

                };

                objParametros.ReferencedAssemblies.Add("System.dll");
                objParametros.ReferencedAssemblies.Add("System.configuration.dll");
                objParametros.ReferencedAssemblies.Add("System.Drawing.dll");
                objParametros.ReferencedAssemblies.Add("Herramientas.dll");
                objParametros.ReferencedAssemblies.Add("WebDriver.dll");
                objParametros.ReferencedAssemblies.Add("WebDriver.Support.dll");


                CodeDomProvider objCompiler = CodeDomProvider.CreateProvider("CSharp");
                CompilerResults objResultados = objCompiler.CompileAssemblyFromSource(objParametros, new string[] { codigo });
                object objClase = objResultados.CompiledAssembly.CreateInstance("PruebaAutomatizacion.prueba");
                var metodo = objClase.GetType().GetMethod("metodoPrueba").Invoke(objClase,new object[] { DatosConfig });
                //var objeto = objClase.GetType().InvokeMember("metodoPrueba", BindingFlags.InvokeMethod, null, objClase, null);
            
            }
            catch (Exception ex)
            {
                string error = "Ha ocurrido un error de compilación, por favor revisa la sintaxis de tu código : "+ex.Message;
                Console.WriteLine(error);
                Console.ReadKey();
            }
        }


        public string leerTexto(string path)
        {
            try
            {
                
                string readText = File.ReadAllText(path);
                string nueva = Constantes.EstructuraClase.Replace("{", "{{");
                nueva = nueva.Replace("}", "}}");
                nueva = nueva.Replace("{{0}}", "{0}");
                nueva = string.Format(nueva, readText.ToString(), 0);
                nueva = nueva.Replace("{{", "{");
                nueva = nueva.Replace("}}", "}");
                nueva = nueva + "}";
                return nueva ;
            }
            catch (Exception ex)
            {
                string error = "Error cargando el archivo: "+ex.Message;
                Console.WriteLine(error);
                Console.ReadKey();
                throw;
            }
        }
    }
}
