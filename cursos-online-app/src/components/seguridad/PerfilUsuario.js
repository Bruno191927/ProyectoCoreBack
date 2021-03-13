import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import React from 'react';
import style from '../Tool/Style';

const PerfilUsuario = () =>{
    return(
        <Container component="main" masWidth="md" justify="center">
            <div style={style.paper}>
                <Typography component="h1" variant="h5">
                    Perfil de Usuario
                </Typography>
            </div>
            <form style={style.form}>
                <Grid container spacing={2}>
                    <Grid item xs={12} md={6}>
                        <TextField name="nombreCompleto" variant="outlined" fullWidth label="Ingrese su nombre y apellido"/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="email" variant="outlined" fullWidth label="Ingrese su email"/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="password" type="password" variant="outlined" fullWidth label="Ingrese su contraseña"/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="confirmepassword" type="password" variant="outlined" fullWidth label="Confirme tu contraseña"/>
                    </Grid>
                </Grid>
                <Grid container justify="center">
                    <Grid item xs={12} md={6}>
                        <Button type="submit" fullWidth variant="contained" size="large" color="primary" style={style.submit}>
                            Guardar Datos
                        </Button>
                    </Grid>
                </Grid>
            </form>
        </Container>
    );
};

export default PerfilUsuario;