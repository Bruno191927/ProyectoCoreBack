import { AppBar } from '@material-ui/core';
import React from 'react';
import { useStateValue } from '../../context/store';
import BarSesion from './bar/BarSesion';

const AppNavBar = () => {
    const [{sesionUsuario},dispatch] = useStateValue();

    return sesionUsuario 
    ? (sesionUsuario.autenticado == true ? <AppBar position="static"><BarSesion/></AppBar>:null)
    : null ;
};

export default AppNavBar;