import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';
import style from '../Tool/Style';
import { useStateValue } from '../../context/store';


const PerfilUsuario = () =>{
    const [{ sesionUsuario}, dispatch] = useStateValue();
    const [usuario,setUsuario] = useState({
        nombreCompleto: '',
        username : '',
        email : '',
        password : '',
        confirmarPassword:''
    });

    const ingresarValoresMemoria = e => {
        const {name,value} = e.target;
        setUsuario(anterior=>({
            ...anterior,
            [name] : value
        }));
    }
    
    useEffect(()=>{
        obtenerUsuarioActual(dispatch).then(response => {
            console.log('Data',response);
            setUsuario(response.data);
        });
    },[]);

    const guardarUsuario = e =>{
        e.preventDefault();
        actualizarUsuario(usuario).then(response=>{
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
    return(
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Typography component="h1" variant="h5">
                    Perfil de Usuario
                </Typography>
            </div>
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
                </Grid>
                <Grid container justify="center">
                    <Grid item xs={12} md={6}>
                        <Button type="submit" onClick={guardarUsuario} fullWidth variant="contained" size="large" color="primary" style={style.submit}>
                            Guardar Datos
                        </Button>
                    </Grid>
                </Grid>
            </form>
        </Container>
    );
};

export default PerfilUsuario;