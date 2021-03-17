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

export const obtenerUsuarioActual = () =>{
    return new Promise((resolve,reject)=>{
        HttpClient.get('/Usuario').then(response=>{
            resolve(response);
        })
    });
}

export const actualizarUsuario = (usuario) => {
    return new Promise((resolve,reject)=>{
        HttpClient.put('/Usuario',usuario).then(
            response => {
                resolve(response);
            }
        )
    })
}