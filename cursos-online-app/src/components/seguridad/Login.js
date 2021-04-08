import React, { useState } from 'react';
import style from '../Tool/Style';
import {Avatar, Button, Container, TextField, Typography} from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import {loginUsuario} from '../../actions/UsuarioAction';
import { withRouter } from 'react-router-dom';
import {useStateValue} from '../../context/store';

const Login = (props) => {
    const [{usuarioSesion},dispatch] = useStateValue();

    const [usuario,setUsuario] = useState({
        Email:'',
        Password : ''
    })

    const ingresarValores = e => {
        const {name,value} = e.target;
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }))
    }

    const loginUsuarioBoton = e => {
        e.preventDefault();
        loginUsuario(usuario,dispatch).then(response => {
            console.log(response);
            if(response.status === 200){
                window.localStorage.setItem('token_seguridad',response.data.token);
                props.history.push("/");
            }
            else{
                dispatch({
                    type:"OPEN_SNACKBAR",
                    openMensaje:{
                        open:true,
                        mensaje : "Las credenciales del usuario son incorrectas"
                    }
                })
            }
        })
    }



    return (
        <Container maxWidth="xs">
            <div style={style.paper}>
                <Avatar style ={style.avatar}>
                    <LockOutlinedIcon style={style.icon}/>
                </Avatar>
                <Typography component="h1" variant="h5">
                    Login de Usuario
                </Typography>
                <form style={style.form}>
                    <TextField variant="outlined" name="Email" value={usuario.Email} onChange={ingresarValores} label="Ingrese email" fullWidth/>
                    <TextField variant="outlined" name="Password" value={usuario.Password} onChange={ingresarValores} type="password" label="Ingresa tu contraseña" fullWidth margin="normal"/>
                    <Button type="submit" onClick={loginUsuarioBoton} fullWidth variant="contained" color="primary" style={style.submit}>
                        Enviar
                    </Button>
                </form>
            </div>
        </Container>
    );
};

export default withRouter(Login);