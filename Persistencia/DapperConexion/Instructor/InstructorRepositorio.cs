using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection _factoryConnection;
        public InstructorRepositorio(IFactoryConnection factoryConnection){
            _factoryConnection = factoryConnection;
        }
        public Task<int> Actualizar(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public Task<int> Eliminar(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Nuevo(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            //nombre del procedimiento almacenado
            var storeProcedure = "usp_Obtener_Instructores";
            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure,null,commandType : CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta",e);
            }finally{
                _factoryConnection.CloseConnection();
            }

            return instructorList;
        }

        public Task<InstructorModel> ObtenerPorId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}