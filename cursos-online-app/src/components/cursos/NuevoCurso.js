import React, { useState } from 'react';
import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import style from '../Tool/Style';
import {MuiPickersUtilsProvider,KeyboardDatePicker} from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import ImageUploader from 'react-images-upload';
import {v4 as uuidv4} from 'uuid';
import {obtenerDataImagen} from '../../actions/ImagenAction';
import { guardarCurso } from '../../actions/CursoAction';
const NuevoCurso = () => {

    const [fechaSeleccionada,setFechaSeleccionada] = useState(new Date());
    const [imagenCurso, setImagenCurso] = useState(null);
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

    const subirFoto = imagenes => {
        const foto = imagenes[0];
        obtenerDataImagen(foto)
        .then((respuesta)=>{
            setImagenCurso(respuesta)
        });
    }

    const guardarCursoBoton = e =>{
        e.preventDefault();
        const cursoId = uuidv4();
        const objetoCurso = {
            titulo: curso.titulo,
            descripcion : curso.descripcion,
            promocion: parseFloat(curso.promocion || 0.0),
            precio: parseFloat(curso.precio || 0.0),
            fechaPublicacion : fechaSeleccionada,
            cursoId : cursoId
        };

        const objetoImagen = {
            nombre : imagenCurso.nombre,
            data : imagenCurso.data,
            extension : imagenCurso.extension,
            objetoReferencia : cursoId
        }

        guardarCurso(objetoCurso,objetoImagen)
        .then(respuestas=>{
            console.log('respuestas arreglo',respuestas);
        });
    }

    const fotoKey = uuidv4();



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
                        <Grid item xs={12} md={6}>
                            <ImageUploader
                                withIcon = {false}
                                key={fotoKey}
                                singleImage = {true}
                                buttonText = "Selecciona Image del curso"
                                onChange = {subirFoto}
                                imgExtension= {[".jpg",".gif",".png",".jpeg"]}
                                maxFileSize = {5242880}
                            />
                        </Grid>
                    </Grid>
                    <Grid container justify="center">
                        <Grid item xs={12} md={6}>
                            <Button type="submit" fullWidth variant="outlined" size="large" style={style.submit} onClick={guardarCursoBoton}>
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