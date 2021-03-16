import HttpClient from '../services/HttpClient';

export const registrarUsuario = usuario => {
    return new Promise ((resolve,eject)=>{
        HttpClient.post('/Usuario/registrar',usuario)
        .then(
            response => {
                resolve(response);
            }
        )
    });
}