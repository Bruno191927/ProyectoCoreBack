import React from 'react';
import style from '../Tool/Style';
import {Avatar, Button, Container, TextField, Typography} from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
const Login = () => {
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
                    <TextField variant="outlined" label="Ingrese username" name="username" fullWidth/>
                    <TextField variant="outlined" type="password" name="password" label="Ingresa tu contraseña" fullWidth margin="normal"/>
                    <Button type="submit" fullWidth variant="contained" color="primary" style={style.submit}>
                        Enviar
                    </Button>
                </form>
            </div>
        </Container>
    );
};

export default Login;