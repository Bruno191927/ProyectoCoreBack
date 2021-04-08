import HttpClient from '../services/HttpClient';
import axios from 'axios';

const instancia = axios.create();
instancia.CancelToken = axios.CancelToken;
instancia.isCancel = axios.isCancel;

export const registrarUsuario = usuario => {
    return new Promise ((resolve,eject)=>{
        instancia.post('/usuario/registrar',usuario)
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
        HttpClient.get('/usuario').then(response=>{
            console.log(response);
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
            resolve(response);
        }).catch((error) => {
            console.log('error actualizar', error);
            
            resolve(error);
          });
    });
}

export const actualizarUsuario = (usuario,dispatch) => {
    return new Promise((resolve,reject)=>{
        HttpClient.put('/usuario',usuario).then(
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

export const loginUsuario = (usuario, dispatch) => {
    return new Promise((resolve, eject) => {
      instancia.post("/usuario/login", usuario).then(
        response => {
          if(response.data && response.data.imagenPerfil) {
            let fotoPerfil = response.data.imagenPerfil;
            const nuevoFile = "data:image/" + fotoPerfil.extension + ";base64," + fotoPerfil.data;
            response.data.imagenPerfil = nuevoFile;
          }
        
        dispatch({
          type : "INICIAR_SESION",
          sesion : response.data,
          autenticado : true
        })
        
        resolve(response);
      }).catch(error => {
          resolve(error.response);
      });
    });
  };