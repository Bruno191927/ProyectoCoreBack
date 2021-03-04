using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;

namespace WebAPI.Controllers
{
    public class InstructorController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores(){
            return await Mediator.Send(new Consulta.Lista());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerInstructorId(Guid id){
            return await Mediator.Send(new ConsultaId.Ejecuta{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta parametros){
            return await Mediator.Send(parametros);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id ,Editar.Ejecuta data){
            data.InstructorId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id){
            return await Mediator.Send(new Eliminar.Ejecuta{Id = id});
        }
    }
}