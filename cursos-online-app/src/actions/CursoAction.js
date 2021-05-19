import HttpClient from '../services/HttpClient';
export const guardarCurso = async (curso,imagen) => {
    const endPointCurso = '/cursos';
    const endPointImagen = '/documentos';

    const promesaCurso = HttpClient.post(endPointCurso,curso);
    const promesaImagen = HttpClient.post(endPointImagen,imagen);

    const responseArray = await Promise.all([promesaCurso,promesaImagen]);
    return responseArray;

}