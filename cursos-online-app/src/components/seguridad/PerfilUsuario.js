import { Avatar, Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';
import style from '../Tool/Style';
import { useStateValue } from '../../context/store';
import reactFoto from '../../logo.svg';
import {v4 as uuidv4} from 'uuid';
import ImageUploader from 'react-images-upload';
import { obtenerDataImagen } from '../../actions/ImagenAction';
const PerfilUsuario = () =>{
    const [{ sesionUsuario}, dispatch] = useStateValue();
    const [usuario,setUsuario] = useState({
        nombreCompleto: '',
        username : '',
        email : '',
        password : '',
        confirmarPassword:'',
        imagenPerfil: null,
        fotoUrl:''
    });

    const ingresarValoresMemoria = e => {
        const {name,value} = e.target;
        setUsuario(anterior=>({
            ...anterior,
            [name] : value
        }));
    }
    
    useEffect(()=>{
        setUsuario(sesionUsuario.usuario);
        setUsuario(anterior =>({
            ...anterior,
            fotoUrl:sesionUsuario.usuario.imagenPerfil
        }));
    },[]);

    const guardarUsuario = e =>{
        e.preventDefault();
        actualizarUsuario(usuario,dispatch).then(response=>{
            if(response.status == 200){
                dispatch({
                    type:"OPEN_SNACKBAR",
                    openMensaje:{
                        open : true,
                        mensaje : "Se guardaron los cambios de manera exitosa"
                    }
                })
                window.localStorage.setItem("token_seguridad",response.data.token);
            }
            else{
                dispatch({
                    type : "OPEN_SNACKBAR",
                    openMensaje:{
                        open:true,
                        mensaje:"Error al intentar actualizar: " + Object.keys(response.data.errors)
                    }
                })
            }
        });
    }
    
    const subirFoto = imagenes =>{
        const foto = imagenes[0];
        const fotoUrl = URL.createObjectURL(foto);
        obtenerDataImagen(foto)
        .then(respuesta => {
            console.log(respuesta);
            setUsuario(anterior=>({
                ...anterior,
                imagenPerfil : respuesta,
                fotoUrl: fotoUrl
            }));
        })


        
    }

    const fotoKey = uuidv4();

    return(
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Avatar style={style.avatar} src={usuario.fotoUrl || reactFoto}/>
                <Typography component="h1" variant="h5">
                    Perfil de Usuario
                </Typography>
                <form style={style.form}>
                    <Grid container spacing={2}>
                        <Grid item xs={12} md={12}>
                            <TextField name="nombreCompleto" value={usuario.nombreCompleto} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese su nombre y apellido"/>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="username" value={usuario.username} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese su Username"/>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="email" value={usuario.email} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese su email"/>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="password" value={usuario.password || "" } onChange={ingresarValoresMemoria} type="password" variant="outlined" fullWidth label="Ingrese su contraseña"/>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="confirmarPassword" value={usuario.confirmarPassword || "" } onChange={ingresarValoresMemoria} type="password" variant="outlined" fullWidth label="Confirme tu contraseña"/>
                        </Grid>
                        <Grid item xs={12} md={12}>
                            <ImageUploader 
                                withIcon={false} 
                                key={fotoKey} 
                                singleImage={true} 
                                buttonText="Seleccione una imagen de perfil" 
                                onChange={subirFoto} 
                                imgExtension={[".png",".jpg",".gif",".jpeg"]}
                                maxFileSize={5242880}
                            />
                        </Grid>
                    </Grid>
                    <Grid container justify="center">
                        <Grid item xs={12} md={6}>
                            <Button type="submit" onClick={guardarUsuario} fullWidth variant="contained" size="large" color="primary" style={style.submit}>
                                Guardar Datos
                            </Button>
                        </Grid>
                    </Grid>
                </form>
            </div>
        </Container>
    );
};

export default PerfilUsuario;