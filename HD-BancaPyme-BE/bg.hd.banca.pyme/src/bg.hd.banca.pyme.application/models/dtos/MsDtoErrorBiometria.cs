using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.models.dtos
{
    public class MsDtoErrorBiometria : MsDtoError
    {
        /// <summary>
        /// Identificador Trazabilidad.
        /// </summary>
        /// <example>24</example>
        public int numeroIntentosRestantesBloqueo { set; get; }
        public int tiempoBloqueoRestante { set; get; }
    }

    public class MsDtoErrorBiometriaPorcent : MsDtoError
    {
        /// <summary>
        /// Identificador Trazabilidad.
        /// </summary>
        /// <example>24</example>
        public int porcentajeCoincidencia { set; get; }
    }
    public class MsDtoErrorBiometriaIntero : MsDtoError
    {
        /// <summary>
        /// Identificador Trazabilidad.
        /// </summary>
        /// <example>24</example>
        public int numeroIntentosRestantesBloqueo { set; get; }
    }
}
