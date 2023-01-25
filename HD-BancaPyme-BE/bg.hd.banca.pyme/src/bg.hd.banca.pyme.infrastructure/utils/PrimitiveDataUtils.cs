
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Configuration;
using System.Collections;
using bg.hd.banca.pyme.infrastructure.data.repositories;
using bg.hd.banca.pyme.domain.entities.persona;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.documento;
using System.ComponentModel;
using Serilog;

namespace bg.hd.banca.pyme.infrastructure.utils
{
    public class PrimitiveDataUtils
    {
        public static string objectToString(Object data)
        {
            string dataRetorno = String.Empty;
            foreach (PropertyDescriptor desc in TypeDescriptor.GetProperties(data))
            {
                if (desc.Name.ToLower() == "identificacion" || desc.Name.ToLower() == "cedula" || desc.Name.ToLower() == "numidentificacion")
                {
                    string identificacion = (string)desc.GetValue(data);
                    string identificacionFuscada =   ObfuscateType(identificacion, typeTag.Identificacion);
                    dataRetorno += desc.Name + ": " + identificacionFuscada + "\n";
                }
                else
                    dataRetorno += desc.Name + ": " + desc.GetValue(data) + "\n"; 
            }
            return dataRetorno;

        }

        public static string objectToString(Object data, List<string> elementosIncluidos)
        {
            string dataRetorno = String.Empty;
            if (elementosIncluidos is null)
            {
                foreach (PropertyDescriptor desc in TypeDescriptor.GetProperties(data))
                {
                    dataRetorno += desc.Name + ": " + desc.GetValue(data) + "\n";
                }
            }
            else
            {
                foreach (PropertyDescriptor desc in TypeDescriptor.GetProperties(data))
                {
                    if (elementosIncluidos.Contains(desc.Name))
                    {
                        dataRetorno += desc.Name + ": " + desc.GetValue(data) + "\n";

                    }
                }
            }
            return dataRetorno;

        }

        public static DataTable ConvertXmlElementToDataTable(XmlElement? xmlElement, string tagName)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName(tagName);

            DataTable dt = new DataTable(tagName);
            if (xmlNodeList.Count > 0)
            {
                int TempColumn = 0;
                foreach (XmlNode node in xmlNodeList.Item(0).ChildNodes)
                {
                    TempColumn++;
                    DataColumn dc = new DataColumn(node.Name, Type.GetType("System.String"));
                    if (dt.Columns.Contains(node.Name))
                    {
                        dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString());
                    }
                    else
                    {
                        dt.Columns.Add(dc);
                    }
                }
                int ColumnsCount = dt.Columns.Count;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if (xmlNodeList.Item(i).ChildNodes[j] != null)
                            dr[j] = xmlNodeList.Item(i).ChildNodes[j].InnerText;
                        else
                            dr[j] = "";
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static string GetDataTableStringColum(DataSet src, string table, string column)
        {
            return src.Tables[table] != null &&
                   src.Tables[table].Columns.Contains(column) ?
                   src.Tables[table].Rows[0][column].ToString() : string.Empty;
        }

        public static string GetDataStringXmlNode(XmlElement src, string node)
        {
            return src.GetElementsByTagName(node) != null ? src.GetElementsByTagName(node)[0].InnerText : "";
        }

        public enum typeTag
        {
            Telefono,
            Nombre,
            Correo,
            Identificacion,
            numeroCuenta
        }

        public static string ObfuscateTags(string value, typeTag type, string cadena)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            switch (type)
            {
                case typeTag.Nombre:

                    string nombre1 = "";
                    string nombre2 = "";
                    string apellido1 = "";
                    string apellido2 = "";
                    string nombreCompleto = "";
                    string[] arregloNombre = new string[10];
                    ArrayList fullName = new ArrayList();
                    ArrayList fullName1 = new ArrayList();
                    string palabrasReservadas_ = cadena;// "da,de,del,la,las,los,san,santa,villa,torre,arco,von,und,di";//
                    ArrayList palabrasReservadas = new ArrayList();
                    Char separador = ',';
                    palabrasReservadas.AddRange(palabrasReservadas_.Split(separador));
                    string auxPalabra = "";//Variable auxiliar para concatenar los apellidos compuestos
                    string nameAux = "";
                    int index = 0;
                    nombreCompleto = value;

                    if (nombreCompleto == null)
                    {
                        nombre1 = "";
                        nombre2 = "";
                        apellido1 = "";
                        apellido2 = "";
                        break;
                    }

                    arregloNombre = nombreCompleto.Split(); //Array del nombre con las palabras separadas en cada posición 
                    int contador = 1;
                    foreach (string reg in arregloNombre)
                    {
                        nameAux = reg.ToLower();
                        index = palabrasReservadas.IndexOf(nameAux);
                        if (index != -1 && arregloNombre.Count() != contador)
                        {
                            auxPalabra += reg + " ";
                        }
                        else
                        {
                            fullName.Add(auxPalabra + reg);
                            auxPalabra = "";
                        }
                        contador++;
                    }
                    if (fullName.Count == 0)
                    {
                        nombre1 = "";
                        nombre2 = "";
                        apellido1 = "";
                        apellido2 = "";
                    }
                    apellido1 = fullName[0].ToString();
                    apellido2 = fullName[1].ToString();
                    fullName.RemoveAt(1);
                    fullName.RemoveAt(0);
                    contador = 1;
                    foreach (string reg in fullName)
                    {
                        nameAux = reg.ToLower();
                        index = palabrasReservadas.IndexOf(nameAux);
                        if (index != -1 && fullName.Count != contador)
                        {
                            auxPalabra += reg + " ";
                        }
                        else
                        {
                            fullName1.Add(auxPalabra + reg);
                            auxPalabra = "";
                        }
                        contador++;
                    }
                    if (fullName1.Count == 0)
                    {
                        nombre1 = "";
                        nombre2 = "";
                    }
                    else if (fullName1.Count < 2)
                    {
                        nombre1 = fullName1[0].ToString();
                        nombre2 = "";
                    }
                    else
                    {
                        nombre1 = fullName1[0].ToString();
                        nombre2 = fullName1[1].ToString();

                        if (fullName1.Count > 2)
                        {
                            fullName1.RemoveAt(1);
                            fullName1.RemoveAt(0);
                            foreach (string reg in fullName1)
                            {
                                nombre2 += " " + reg;
                            }

                        }

                    }

                    if (arregloNombre.Count() == 2)
                    {
                        nombre1 = apellido2;
                        apellido2 = "";
                    }

                    value = nombre1;
                    break;
            }
            return value;
        }

        public static object SepararNombres(string value, typeTag type, string cadena)
        {
            IdentificaNombres nombresSeparados = new IdentificaNombres();

       
            switch (type)
            {
                case typeTag.Nombre:

                    

                    string nombre1 = "";
                    string nombre2 = "";
                    string apellido1 = "";
                    string apellido2 = "";
                    string nombreCompleto = "";
                    string[] arregloNombre = new string[10];
                    ArrayList fullName = new ArrayList();
                    ArrayList fullName1 = new ArrayList();
                    string palabrasReservadas_ = cadena;// "da,de,del,la,las,los,san,santa,villa,torre,arco,von,und,di";
                    ArrayList palabrasReservadas = new ArrayList();
                    Char separador = ',';
                    palabrasReservadas.AddRange(palabrasReservadas_.Split(separador));
                    string auxPalabra = "";//Variable auxiliar para concatenar los apellidos compuestos
                    string nameAux = "";
                    int index = 0;
                    nombreCompleto = value;

                    if (nombreCompleto == null)
                    {
                        nombre1 = "";
                        nombre2 = "";
                        apellido1 = "";
                        apellido2 = "";
                        break;
                    }

                    arregloNombre = nombreCompleto.Split(); //Array del nombre con las palabras separadas en cada posición 
                    int contador = 1;
                    foreach (string reg in arregloNombre)
                    {
                        nameAux = reg.ToLower();
                        index = palabrasReservadas.IndexOf(nameAux);
                        if (index != -1 && arregloNombre.Count() != contador)
                        {
                            auxPalabra += reg + " ";
                        }
                        else
                        {
                            fullName.Add(auxPalabra + reg);
                            auxPalabra = "";
                        }
                        contador++;
                    }
                    if (fullName.Count == 0)
                    {
                        nombre1 = "";
                        nombre2 = "";
                        apellido1 = "";
                        apellido2 = "";
                    }
                    apellido1 = fullName[0].ToString();
                    apellido2 = fullName[1].ToString();
                    fullName.RemoveAt(1);
                    fullName.RemoveAt(0);
                    contador = 1;
                    foreach (string reg in fullName)
                    {
                        nameAux = reg.ToLower();
                        index = palabrasReservadas.IndexOf(nameAux);
                        if (index != -1 && fullName.Count != contador)
                        {
                            auxPalabra += reg + " ";
                        }
                        else
                        {
                            fullName1.Add(auxPalabra + reg);
                            auxPalabra = "";
                        }
                        contador++;
                    }
                    if (fullName1.Count == 0)
                    {
                        nombre1 = "";
                        nombre2 = "";
                    }
                    else if (fullName1.Count < 2)
                    {
                        nombre1 = fullName1[0].ToString();
                        nombre2 = "";
                    }
                    else
                    {
                        nombre1 = fullName1[0].ToString();
                        nombre2 = fullName1[1].ToString();

                        if (fullName1.Count > 2)
                        {
                            fullName1.RemoveAt(1);
                            fullName1.RemoveAt(0);
                            foreach (string reg in fullName1)
                            {
                                nombre2 += " " + reg;
                            }

                        }

                    }

                    if (arregloNombre.Count() == 2)
                    {
                        nombre1 = apellido2;
                        apellido2 = "";
                    }

                    nombresSeparados.primerNombre = nombre1;
                    nombresSeparados.segundoNombre = nombre2;
                    nombresSeparados.primerApellido = apellido1;
                    nombresSeparados.SegundoApellido = apellido2;

                    break;
            }
            return nombresSeparados;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool ValidarEstructuraEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                
                string DomainMapper(Match match)
                {
                
                    var idn = new IdnMapping();

                
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static Producto obtenerProducto(string nombreProducto, IConfiguration _configuration)
        {
            Producto productoSelect = new Producto();
            Producto[] producto = _configuration.GetSection("ProductoConfig").Get<Producto[]>();
            foreach (Producto productoAUX in producto)
            {
                if (productoAUX.nombreProducto == nombreProducto)
                {
                    productoSelect = productoAUX;
                }
            }

            return productoSelect;
        }
        public static Producto obtenerIdProducto(string idProducto, IConfiguration _configuration)
        {
            Producto productoSelect = new Producto();
            Producto[] producto = _configuration.GetSection("ProductoConfig").Get<Producto[]>();
            foreach (Producto productoAUX in producto)
            {
                if (productoAUX.idProducto == idProducto)
                {
                    productoSelect = productoAUX;
                }
            }

            return productoSelect;
        }
        public static string obtenerEdad(ref string fechaNacimiento)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(fechaNacimiento))
            { fechaNacimiento = DateTime.Today.ToString("dd-MM-yyyy"); }

            string[] fechaA = fechaNacimiento.Replace("/", "-").Split('-');

            if (fechaA.Count() == 3)
            {
                DateTime nacimiento = new DateTime(1900, 1, 1);

                if (fechaA[0].Length == 4) // si es el año que nos llega en la primera posicion del arreglo
                {
                    nacimiento = new DateTime(Convert.ToInt32(fechaA[0]), Convert.ToInt32(fechaA[1]), Convert.ToInt32(fechaA[2])); //Fecha de nacimiento año mes dia
                    fechaNacimiento = (fechaA[2] + "/" + fechaA[1] + "/" + fechaA[0]); //Fecha de nacimiento dia mes año
                }
                if (fechaA[0].Length == 2) // si es el año que nos llega en la última posicion del arreglo
                {
                    nacimiento = new DateTime(Convert.ToInt32(fechaA[2]), Convert.ToInt32(fechaA[1]), Convert.ToInt32(fechaA[0])); //Fecha de nacimiento año mes dia
                    fechaNacimiento = (fechaA[0] + "/" + fechaA[1] + "/" + fechaA[2]); //Fecha de nacimiento dia mes año
                }

                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                result = edad.ToString();
            }

            return result;
        }
        public static Documento obtenerDocumento(string id, IConfiguration _configuration)
        {

            Documento documento = new Documento();
            Documento[] documentos = _configuration.GetSection("DocumentoConfig").Get<Documento[]>();
            foreach (Documento documentoAux in documentos)
            {
                if (documentoAux.id == id)
                {
                    documento = documentoAux;
                }
            }
            return documento;
        }
        public static void saveLogsInformation(string resourceRequestPath, string identificador, Object requestDataO, Object responseDataO)
        {
            string requestData = objectToString(requestDataO);
            string responseData = objectToString(responseDataO);
            string indetificacionFuscate = ObfuscateType(identificador, typeTag.Identificacion);
            Log.Information("{ResourceRequestPath} {identificador} {requestData} {ResponseData}", resourceRequestPath, indetificacionFuscate, requestData, responseData);
        }

        public static void saveLogsInformation(string resourceRequestPath, string identificador, List<string> elementosIncluidosRequest, Object requestDataO, List<string> elementosIncluidosResponse, Object responseDataO)
        {
            string requestData = objectToString(requestDataO, elementosIncluidosRequest);
            string responseData = objectToString(responseDataO, elementosIncluidosResponse);
            Log.Information("{ResourceRequestPath} {identificador} {requestData} {ResponseData}", resourceRequestPath, identificador, requestData, responseData);

        }
        public static string ObfuscateType(string value, typeTag type)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            switch (type)
            {
                case typeTag.Correo:
                    if (value.Contains("@"))
                    {
                        int idx = 0;

                        StringBuilder tempValueEmail = new StringBuilder();
                        string[] splitEmail = value.Split('@');
                        int limitemail = splitEmail[0].Length - 1;
                        foreach (var tag in splitEmail[0].ToArray())
                        {
                            if (idx >= 2 && idx < limitemail)
                            {
                                tempValueEmail.Append('X');
                            }
                            else
                            {
                                tempValueEmail.Append(tag);
                            }
                            idx++;
                        }
                        value = string.Format("{0}@{1}", tempValueEmail, splitEmail[1]);
                    }
                    break;
                case typeTag.Identificacion:
                    int i = 0;
                    int limit = value.Length - 3;
                    StringBuilder tempValue = new StringBuilder();
                    foreach (var tag in value.ToArray())
                    {
                        if (i >= 3 && i < limit)
                        {
                            tempValue.Append('X');
                        }
                        else
                        {
                            tempValue.Append(tag);
                        }
                        i++;
                    }
                    value = tempValue.ToString();
                    break;
                case typeTag.numeroCuenta:
                    int y = 0;
                    int limite = value.Length - 3;
                    StringBuilder tempValor = new StringBuilder();
                    foreach (var tag in value.ToArray())
                    {
                        if (y >= 3 && y < limite)
                        {
                            tempValor.Append('X');
                        }
                        else
                        {
                            tempValor.Append(tag);
                        }
                        y++;
                    }
                    value = tempValor.ToString();
                    break;
               case typeTag.Telefono:
                    int r = 0;
                    int limiteT = value.Length - 2;
                    StringBuilder tempValueT = new StringBuilder();
                    foreach (var tag in value.ToArray())
                    {
                        if (r >= 2 && r < limiteT)
                        {
                            tempValueT.Append('X');
                        }
                        else
                        {
                            tempValueT.Append(tag);
                        }
                        r++;
                    }
                    value = tempValueT.ToString();
                    break;
            }
            return value;
        }

    }
}
