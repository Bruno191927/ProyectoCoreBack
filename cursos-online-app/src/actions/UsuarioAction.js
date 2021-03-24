import HttpClient from '../services/HttpClient';

export const registrarUsuario = usuario => {
    return new Promise ((resolve,eject)=>{
        HttpClient.post('/Usuario/registrar',usuario)
        .then(
            response => {
                resolve(response);
            }
        ).catch(error=>{
            resolve(error.response);
        })
    });
}

export const obtenerUsuarioActual = (dispatch) =>{
    return new Promise((resolve,reject)=>{
        HttpClient.get('/Usuario').then(response=>{
            if(typeof(dispatch) == 'function'){
                dispatch({
                    type:"INICIAR_SESION",
                    sesion : response.data,
                    autenticado : true
                });
            }
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
        .catch(error=>{
            resolve(error.response)
        })
    })
}

export const loginUsuario = usuario => {
    return new Promise((resolve,reject)=>{
        HttpClient.post('/Usuario/login',usuario).then(
            response => {
                resolve(response);
            }
        )
    })
}