using Core.DTO;
using Core.Entities;

namespace Infrastructure.Queries.interfaces
{
    public interface IAlumnoQuery
    {

        Task<List<AlumnoDTO>> ListarAlumnos();
        Task<AlumnoDTO> CrearAlumno(Alumno alumno);
        Task<AlumnoDTO> ActualizarAlumno(Alumno alumno);
        Task<bool> EliminarAlumno(int idAlumno);

        Task<List<AlumnoDTO>> ListarAlumnosByAula(int idAula);
        Task<List<AlumnoDTO>> ListarAlumnosByAulaAndDocente(int idAula, int idDocente);

        Task<bool> AsignarAula(AlumnoAula model);

        Task<bool> EliminarAula(int idAlumno, int idAula);




    }
}
