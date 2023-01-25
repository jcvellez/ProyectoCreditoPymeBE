using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace bg.hd.banca.pyme.domain.Clases
{
    public class clsAgenteServicioPersona
    {
        
        public DataSet ConsultaDatosClientes(int idTipoIdentificacion, string numIdentificacion, ref string codTrans, ref string msjTrans)
        {           
            DataSet dsResultado = new DataSet(); 

            try
            {

            }
            catch (Exception ex)
            {
                codTrans = "001";
                msjTrans = "Ocurrió un error: " + ex.Message;
            }
            return dsResultado;
        }

        public DataSet ConsultaDatosClientes(string numIdentificacion, string opcion, ref string codTrans, ref string msjTrans)
        {
            DataSet dsResultado = new DataSet();
            try
            {

            }
            catch (Exception ex)
            {
                codTrans = "001";
                msjTrans = "Ocurrió un error: " + ex.Message;
            }
            return dsResultado;
        }

        public DataSet ConsultaDatosClientes(string numIdentificacion, string opcion, string idPersona, ref string codTrans, ref string msjTrans)
        {
            DataSet dsResultado = new DataSet();
            try
            {

            }
            catch (Exception ex)
            {
                codTrans = "001";
                msjTrans = "Ocurrió un error: " + ex.Message;
            }
            return dsResultado;
        }

    }
}
