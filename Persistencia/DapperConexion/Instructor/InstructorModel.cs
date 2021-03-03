using System;

namespace Persistencia.DapperConexion.Instructor
{
    //modelo del mapeo de la bd
    public class InstructorModel
    {
        public Guid InstructorId {get;set;}
        public string Nombre {get;set;}
        public string Apellidos {get;set;}
        public string Grado {get;set;}
    }
}