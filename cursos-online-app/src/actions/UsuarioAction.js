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
            console.log(response);
            if(typeof(dispatch) == 'function'){
                if(response.data && response.data.imagenPerfil){
                    let fotoPerfil = response.data.imagenPerfil;
                    const nuevoFile = 'data:image/'+fotoPerfil.extension+';base64,'+fotoPerfil.data;
                    response.data.imagenPerfil = nuevoFile
                }
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

export const actualizarUsuario = (usuario,dispatch) => {
    return new Promise((resolve,reject)=>{
        HttpClient.put('/Usuario',usuario).then(
            response => {
                //actualizar el reducer
                if(response.data && response.data.imagenPerfil){
                    let fotoPerfil = response.data.imagenPerfil;
                    const nuevoFile = 'data:image/'+fotoPerfil.extension+';base64,'+fotoPerfil.data;
                    response.data.imagenPerfil = nuevoFile;
                }
                
                dispatch({
                    type:'INICIAR_SESION',
                    sesion:response.data,
                    autenticado:true
                });

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