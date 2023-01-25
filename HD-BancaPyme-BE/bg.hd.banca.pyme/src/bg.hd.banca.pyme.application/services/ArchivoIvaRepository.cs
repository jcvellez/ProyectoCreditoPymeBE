using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;
using bg.hd.banca.pyme.application.models.exeptions;

namespace bg.hd.banca.pyme.application.services
{
    public class ArchivoIvaRepository: IArchivoIvaRepository
    {
        private readonly IArchivoIvaRestRepository _archivoIvaRestRepository;

        public ArchivoIvaRepository(IArchivoIvaRestRepository _archivoIvaRestRepository)
        {
            this._archivoIvaRestRepository = _archivoIvaRestRepository;
        }

        public async Task<ArchivoImpuestoIvaResponse> ValidarArchivoIva(ArchivoImpuestoIvaRequest request)
        {
            if (request.identificacion is null)
            {
                throw new ArchivosImpuestoExeption("Identificación no valida", "Identificación no valida", 2);
            }

            if (request.file is null)
            {
                throw new ArchivosImpuestoExeption("Archivo no valido", "Archivo no valido", 5);
            }

            return await _archivoIvaRestRepository.ValidarArchivoIva(request);
        }

        public async Task<IngresoDeclaracionSemestralResponse> IngresoDeclaracionSemestral(IngresoDeclaracionSemestralRequest request)
        {
            return await _archivoIvaRestRepository.IngresoDeclaracionSemestral(request);
        }

    }
}
