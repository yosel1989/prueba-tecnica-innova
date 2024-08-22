using Core.DTO;
using Core.Entities;
using Infrastructure.Queries.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {

        private readonly IAlumnoQuery _IAlumnoQuery;

        public AlumnoController(IAlumnoQuery IAlumnoQuery)
        {
            _IAlumnoQuery = IAlumnoQuery;
        }


        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            try
            {
                var lista = await _IAlumnoQuery.ListarAlumnos();
                return Ok(new
                {
                    data = lista,
                    message = "",
                    status = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new { },
                    message = ex.Message,
                    status = StatusCodes.Status400BadRequest
                });
            }
        }


        [HttpPost]
        public async Task<ActionResult> Crear(Alumno model)
        {

            try
            {
                AlumnoDTO alumno = await _IAlumnoQuery.CrearAlumno(model);
                return Ok(new
                {
                    data = alumno,
                    message = "",
                    status = StatusCodes.Status201Created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new { },
                    message = ex.Message,
                    status = StatusCodes.Status400BadRequest
                });
            }
        }


        [HttpPut]
        public async Task<ActionResult> Modificar(Alumno model)
        {

            try
            {
                AlumnoDTO alumno = await _IAlumnoQuery.ActualizarAlumno(model);
                return Ok(new
                {
                    data = alumno,
                    message = "",
                    status = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new { },
                    message = ex.Message,
                    status = StatusCodes.Status400BadRequest
                });
            }
        }


        [HttpDelete("idAlumno")]
        public async Task<ActionResult> Eliminar(int idAlumno)
        {

            try
            {
                Boolean exito = await _IAlumnoQuery.EliminarAlumno(idAlumno);
                return Ok(new
                {
                    data = exito,
                    message = "",
                    status = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new { },
                    message = ex.Message,
                    status = StatusCodes.Status400BadRequest
                });
            }
        }



        [HttpGet("aula/{idAula}")]
        public async Task<ActionResult> AlumnosByAula(int idAula)
        {
            try
            {
                var lista = await _IAlumnoQuery.ListarAlumnosByAula(idAula);
                return Ok(new
                {
                    data = lista,
                    message = "",
                    status = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new { },
                    message = ex.Message,
                    status = StatusCodes.Status400BadRequest
                });
            }
        }


        [HttpGet("aula/{idAula}/docente/{idDocente}")]
        public async Task<ActionResult> AlumnosByAulaAndDocente(int idAula, int idDocente)
        {
            try
            {
                var lista = await _IAlumnoQuery.ListarAlumnosByAulaAndDocente(idAula, idDocente);
                return Ok(new
                {
                    data = lista,
                    message = "",
                    status = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new { },
                    message = ex.Message,
                    status = StatusCodes.Status400BadRequest
                });
            }
        }



        [HttpPost("alumno-aula")]
        public async Task<ActionResult> AsignarAula(AlumnoAula model)
        {

            try
            {
                Boolean exito = await _IAlumnoQuery.AsignarAula(model);
                return Ok(new
                {
                    data = exito,
                    message = "",
                    status = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new { },
                    message = ex.Message,
                    status = StatusCodes.Status400BadRequest
                });
            }
        }



        [HttpDelete("alumno-aula/{idAlumno}/{idAula}")]
        public async Task<ActionResult> ElimnarAula(int idAlumno, int idAula)
        {

            try
            {
                Boolean exito = await _IAlumnoQuery.EliminarAula(idAlumno, idAula);
                return Ok(new
                {
                    data = exito,
                    message = "",
                    status = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new { },
                    message = ex.Message,
                    status = StatusCodes.Status400BadRequest
                });
            }
        }

    }
}
