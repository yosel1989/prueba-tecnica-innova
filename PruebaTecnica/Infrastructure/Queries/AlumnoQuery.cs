using Core.DTO;
using Core.Entities;
using Infrastructure.Queries.interfaces;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
namespace Infrastructure.Queries
{
    public class AlumnoQuery : IAlumnoQuery
    {
        private readonly IConfiguration _configuration;
        private readonly string ConnectionString;
        public AlumnoQuery(IConfiguration configuration)
        {
            //Deberá modificar la cadena de conexión antes de ejecutar
            _configuration = configuration;
            ConnectionString = ConfigurationExtensions.GetConnectionString(this._configuration, "PruebaTecnicaDB"); ;
        }


        public async Task<List<AlumnoDTO>> ListarAlumnos()
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConnectionString);
                await conn.OpenAsync();
                using SqlCommand cmd = new SqlCommand("SP_Alumno_Listar", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                var reader = await cmd.ExecuteReaderAsync();
                var output = await ReadListar(reader);

                conn.Close();

                return output;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }
        public async Task<AlumnoDTO> CrearAlumno(Alumno alumno)
        {
            try
            {
                using SqlConnection conn = new(ConnectionString);
                await conn.OpenAsync();
                using SqlCommand cmd = new SqlCommand("SP_Alumno_Crear", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("pNombre", alumno.Nombre);
                cmd.Parameters.AddWithValue("pApellido", alumno.Apellido);
                cmd.Parameters.AddWithValue("pIdGenero", alumno.IdGenero);
                cmd.Parameters.AddWithValue("pFechaNacimiento", alumno.FechaNacimiento);
                cmd.Parameters.AddWithValue("pIdTipoDocumento", alumno.IdTipoDocumento);
                cmd.Parameters.AddWithValue("pNumeroDocumento", alumno.NumeroDocumento);
                var reader = await cmd.ExecuteReaderAsync();
                var output = await ReadCrear(reader);

                conn.Close();

                return output;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }


        public async Task<AlumnoDTO> ActualizarAlumno(Alumno alumno)
        {
            try
            {
                using SqlConnection conn = new(ConnectionString);
                await conn.OpenAsync();
                using SqlCommand cmd = new SqlCommand("SP_Alumno_Modificar", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("pId", alumno.Id);
                cmd.Parameters.AddWithValue("pNombre", alumno.Nombre);
                cmd.Parameters.AddWithValue("pApellido", alumno.Apellido);
                cmd.Parameters.AddWithValue("pIdGenero", alumno.IdGenero);
                cmd.Parameters.AddWithValue("pFechaNacimiento", alumno.FechaNacimiento);
                cmd.Parameters.AddWithValue("pIdTipoDocumento", alumno.IdTipoDocumento);
                cmd.Parameters.AddWithValue("pNumeroDocumento", alumno.NumeroDocumento);
                var reader = await cmd.ExecuteReaderAsync();
                var output = await ReadActualizar(reader);

                conn.Close();

                return output;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public async Task<bool> EliminarAlumno(int idAlumno)
        {
            try
            {
                using SqlConnection conn = new(ConnectionString);
                await conn.OpenAsync();
                using SqlCommand cmd = new SqlCommand("SP_Alumno_Eliminar", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("pId", idAlumno);
                var reader = await cmd.ExecuteReaderAsync();
                var output = await ReadEliminar(reader);

                conn.Close();

                return output;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }
        public async Task<List<AlumnoDTO>> ListarAlumnosByAula(int idAula)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConnectionString);
                await conn.OpenAsync();
                using SqlCommand cmd = new SqlCommand("SP_Alumno_ListarByAula", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("pIdAula", idAula);

                var reader = await cmd.ExecuteReaderAsync();
                var output = await ReadListarByAula(reader);

                conn.Close();

                return output;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public async Task<List<AlumnoDTO>> ListarAlumnosByAulaAndDocente(int idAula, int idDocente)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(ConnectionString);
                await conn.OpenAsync();
                using SqlCommand cmd = new SqlCommand("SP_Alumno_ListarByAulaAndDocente", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("pIdAula", idAula);
                cmd.Parameters.AddWithValue("pIdDocente", idDocente);

                var reader = await cmd.ExecuteReaderAsync();
                var output = await ReadListarByAulaAndDocente(reader);

                conn.Close();

                return output;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }
        public async Task<bool> AsignarAula(AlumnoAula model)
        {
            try
            {
                using SqlConnection conn = new(ConnectionString);
                await conn.OpenAsync();
                using SqlCommand cmd = new SqlCommand("SP_Alumno_AsignarAula", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("pIdAlumno", model.IdAlumno);
                cmd.Parameters.AddWithValue("pIdAula", model.IdAula);
                var reader = await cmd.ExecuteReaderAsync();
                var output = await ReadAsignarAula(reader);

                conn.Close();

                return output;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }
        public async Task<bool> EliminarAula(int idAlumno, int idAula)
        {
            try
            {
                using SqlConnection conn = new(ConnectionString);
                await conn.OpenAsync();
                using SqlCommand cmd = new SqlCommand("SP_Alumno_EliminarAula", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("pIdAlumno", idAlumno);
                cmd.Parameters.AddWithValue("pIdAula", idAula);
                var reader = await cmd.ExecuteReaderAsync();
                var output = await ReadEliminarAula(reader);

                conn.Close();

                return output;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }








        static async Task<List<AlumnoDTO>> ReadListar(DbDataReader reader)
        {
            try
            {
                List<AlumnoDTO> alumnos = new List<AlumnoDTO>();
                while (await reader.ReadAsync())
                {
                    AlumnoDTO alumno = new AlumnoDTO();
                    alumno.Id = Convert.ToInt32(reader["Id"]);
                    alumno.Nombre = Convert.ToString(reader["Nombre"]);
                    alumno.Apellido = Convert.ToString(reader["Apellido"]);
                    alumno.FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]);
                    alumno.IdGenero = Convert.ToInt32(reader["IdGenero"]);
                    alumno.Genero = Convert.ToString(reader["Genero"]);
                    alumno.IdTipoDocumento = Convert.ToInt32(reader["IdTipoDocumento"]);
                    alumno.TipoDocumento = Convert.ToString(reader["TipoDocumento"]);
                    alumno.NumeroDocumento = Convert.ToString(reader["NumeroDocumento"]);
                    alumnos.Add(alumno);
                }

                return alumnos;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }


        static async Task<AlumnoDTO> ReadCrear(DbDataReader reader)
        {
            try
            {
                AlumnoDTO alumno = new AlumnoDTO();
                while (await reader.ReadAsync())
                {
                    alumno.Id = Convert.ToInt32(reader["Id"]);
                    alumno.Nombre = Convert.ToString(reader["Nombre"]);
                    alumno.Apellido = Convert.ToString(reader["Apellido"]);
                    alumno.FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]);
                    alumno.IdGenero = Convert.ToInt32(reader["IdGenero"]);
                    alumno.Genero = Convert.ToString(reader["Genero"]);
                    alumno.IdTipoDocumento = Convert.ToInt32(reader["IdTipoDocumento"]);
                    alumno.TipoDocumento = Convert.ToString(reader["TipoDocumento"]);
                    alumno.NumeroDocumento = Convert.ToString(reader["NumeroDocumento"]);
                }

                return alumno;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }



        static async Task<AlumnoDTO> ReadActualizar(DbDataReader reader)
        {
            try
            {
                AlumnoDTO alumno = new AlumnoDTO();
                while (await reader.ReadAsync())
                {
   
                    alumno.Id = Convert.ToInt32(reader["Id"]);
                    alumno.Nombre = Convert.ToString(reader["Nombre"]);
                    alumno.Apellido = Convert.ToString(reader["Apellido"]);
                    alumno.FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]);
                    alumno.IdGenero = Convert.ToInt32(reader["IdGenero"]);
                    alumno.Genero = Convert.ToString(reader["Genero"]);
                    alumno.IdTipoDocumento = Convert.ToInt32(reader["IdTipoDocumento"]);
                    alumno.TipoDocumento = Convert.ToString(reader["TipoDocumento"]);
                    alumno.NumeroDocumento = Convert.ToString(reader["NumeroDocumento"]);
                }

                return alumno;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        static async Task<bool> ReadEliminar(DbDataReader reader)
        {
            try
            {
                Boolean response = false;
                while (await reader.ReadAsync())
                {
                    response = Convert.ToBoolean(reader["Exito"]);
                }

                return response;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        static async Task<List<AlumnoDTO>> ReadListarByAula(DbDataReader reader)
        {
            try
            {
                List<AlumnoDTO> alumnos = new List<AlumnoDTO>();
                while (await reader.ReadAsync())
                {
                    AlumnoDTO alumno = new AlumnoDTO();
                    alumno.Id = Convert.ToInt32(reader["Id"]);
                    alumno.Nombre = Convert.ToString(reader["Nombre"]);
                    alumno.Apellido = Convert.ToString(reader["Apellido"]);
                    alumno.FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]);
                    alumno.IdGenero = Convert.ToInt32(reader["IdGenero"]);
                    alumno.Genero = Convert.ToString(reader["Genero"]);
                    alumno.IdTipoDocumento = Convert.ToInt32(reader["IdTipoDocumento"]);
                    alumno.TipoDocumento = Convert.ToString(reader["TipoDocumento"]);
                    alumno.NumeroDocumento = Convert.ToString(reader["NumeroDocumento"]);
                    alumnos.Add(alumno);
                }

                return alumnos;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }



        static async Task<List<AlumnoDTO>> ReadListarByAulaAndDocente(DbDataReader reader)
        {
            try
            {
                List<AlumnoDTO> alumnos = new List<AlumnoDTO>();
                while (await reader.ReadAsync())
                {
                    AlumnoDTO alumno = new AlumnoDTO();
                    alumno.Id = Convert.ToInt32(reader["Id"]);
                    alumno.Nombre = Convert.ToString(reader["Nombre"]);
                    alumno.Apellido = Convert.ToString(reader["Apellido"]);
                    alumno.FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]);
                    alumno.IdGenero = Convert.ToInt32(reader["IdGenero"]);
                    alumno.Genero = Convert.ToString(reader["Genero"]);
                    alumno.IdTipoDocumento = Convert.ToInt32(reader["IdTipoDocumento"]);
                    alumno.TipoDocumento = Convert.ToString(reader["TipoDocumento"]);
                    alumno.NumeroDocumento = Convert.ToString(reader["NumeroDocumento"]);
                    alumnos.Add(alumno);
                }

                return alumnos;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }


        static async Task<bool> ReadAsignarAula(DbDataReader reader)
        {
            try
            {
                Boolean response = false;
                while (await reader.ReadAsync())
                {
                    response = Convert.ToBoolean(reader["Exito"]);
                }

                return response;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }


        static async Task<bool> ReadEliminarAula(DbDataReader reader)
        {
            try
            {
                Boolean response = false;
                while (await reader.ReadAsync())
                {
                    response = Convert.ToBoolean(reader["Exito"]);
                }

                return response;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

    }
}
