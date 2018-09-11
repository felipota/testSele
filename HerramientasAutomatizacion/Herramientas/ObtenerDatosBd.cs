using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herramientas
{
    public class ObtenerDatosBd
    {
        public Dictionary<int, Dictionary<string, string>> ConectarBd(string Query,string parametros)
        {
           
            string CadenaConexion = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                string[] Campos = parametros.Split('-');
                Dictionary<string, string> Fila = null;
                Dictionary<int, Dictionary<string, string>> Registros = new Dictionary<int, Dictionary<string, string>>();
                string val = null;

                using (SqlConnection conn = new SqlConnection(CadenaConexion))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(Query, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    int key = 0;
                    while (dr.Read())
                    {
                        Fila = new Dictionary<string, string>();
                        for (int i = 0; i <= Campos.Length - 1; i++)
                        {
                            string vs = dr.GetName(i);
                            if (Campos[i]== vs)
                            {
                                val = dr[Campos[i]].ToString();
                                Fila.Add(Campos[i], val);
                            }
                        }
                        Registros.Add(key, Fila);
                        key++;
                    }
                    dr.Close();
                }
                return Registros;
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                throw ex;
            }

        }
    }
}
