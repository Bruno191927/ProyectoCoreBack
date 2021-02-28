using System;

namespace Dominio
{
    public class Comentario
    {
        public Guid ComentarioId {get;set;}//guid usa un valor unico se usa como llave primaria
        public string Alumno {get;set;}
        public int Puntaje {get;set;}
        public string ComentarioTexto {get;set;}
        public Guid CursoId {get;set;}
        public Curso Curso {get;set;}
    }
}