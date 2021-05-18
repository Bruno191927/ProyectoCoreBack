import React, { useState } from 'react';
import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import style from '../Tool/Style';
import {MuiPickersUtilsProvider,KeyboardDatePicker} from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';

const NuevoCurso = () => {

    const [fechaSeleccionada,setFechaSeleccionada] = useState(new Date());
    const [curso,setCurso] = useState({
        titulo: '',
        descripcion: '',
        precio : 0.0,
        promocion : 0.0
    });

    const ingresarValoresMemoria = e => {
        const {name,value} = e.target;
        setCurso((anterior)=>({
            ...anterior,
            [name] : value
        }))
    }
    return (
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Typography component="h1" variant="h5">
                    Registro de nuevo curso
                </Typography>
                <form style={style.form}>
                    <Grid container spacing={2}>
                        <Grid item xs={12} md={12}>
                            <TextField name="titulo" value={curso.titulo} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese Titulo"/>
                        </Grid>
                        <Grid item xs={12} md={12}>
                            <TextField name="descripcion" value={curso.descripcion} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese Descripcion"/>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="precio" value={curso.precio} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese Precio Normal"/>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="promocion" value={curso.promocion} onChange={ingresarValoresMemoria} variant="outlined" fullWidth label="Ingrese Precio Promocion"/>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <MuiPickersUtilsProvider utils={DateFnsUtils}>
                                <KeyboardDatePicker
                                value={fechaSeleccionada}
                                onChange={setFechaSeleccionada}
                                margin="normal"
                                id="fecha-publicacion-id"
                                label="Seleccione Fecha de publicacion"
                                format="dd/MM/yyyy"
                                fullWidth
                                KeyboardButtonProps = {{
                                    "aria-label" : "change date"
                                }}/>
                            </MuiPickersUtilsProvider>
                        </Grid>
                    </Grid>
                    <Grid container justify="center">
                        <Grid item xs={12} md={6}>
                            <Button type="submit" fullWidth variant="outlined" size="large" style={style.submit}>
                                Guardar Curso
                            </Button>
                        </Grid>
                    </Grid>
                </form>
            </div>
        </Container>
    );
};

export default NuevoCurso;