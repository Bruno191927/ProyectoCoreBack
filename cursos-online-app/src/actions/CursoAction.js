import HttpClient from '../services/HttpClient';
export const guardarCurso = async (curso, imagen) => {
    const endPointCurso = '/cursos';
    const promesaCurso = HttpClient.post(endPointCurso, curso);
    
    if(imagen){
        const endPointImagen = '/documentos';    
        const promesaImagen = HttpClient.post(endPointImagen, imagen);
        return await Promise.all([promesaCurso, promesaImagen]);
    }else{
        return await Promise.all([promesaCurso]);
    }
};