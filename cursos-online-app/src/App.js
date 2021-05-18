import React, { useEffect, useState } from 'react';
import {ThemeProvider as MuithemeProvider} from '@material-ui/core/styles';
import theme from './theme/theme';
import {BrowserRouter as Router,Switch,Route} from 'react-router-dom';
import RegistrarUsuario from './components/seguridad/RegistrarUsuario';
import PerfilUsuario from './components/seguridad/PerfilUsuario';
import { Grid, Snackbar } from '@material-ui/core';
import Login from './components/seguridad/Login';
import AppNavBar from './components/navegacion/AppNavBar';
import {useStateValue} from './context/store';
import { obtenerUsuarioActual } from './actions/UsuarioAction';
import RutaSegura from './components/navegacion/RutaSegura';
import NuevoCurso from './components/cursos/NuevoCurso';

function App() {

  const [{openSnackbar},dispatch] = useStateValue();
  const [iniciaApp,setIniciaApp] = useState(false);

  useEffect(()=>{
    if(!iniciaApp){
      obtenerUsuarioActual(dispatch).then(response=>{
        setIniciaApp(true);
      }).catch(error=>{
        setIniciaApp(true);
      });
    }
  },[iniciaApp]);


  return iniciaApp === false ? null : (
    <React.Fragment>
      <Snackbar 
        anchorOrigin={{vertical:"bottom",horizontal:"center"}}
        open={openSnackbar ? openSnackbar.open : false}
        autoHideDuration={3000}
        ContentProps={{"aria-describedby":"message-id"}}
        message={
          <span id="message-id">{openSnackbar?openSnackbar.mensaje : ""}</span>
        }
        onClose={
          ()=> dispatch({
            type : "OPEN_SNACKBAR",
            openMensaje:{
              open:false,
              mensaje :""
            }
          })
        }
      >
      </Snackbar>
      <Router>
        <MuithemeProvider theme={theme}>
          <AppNavBar/>
          <Grid container>
            <Switch>
              <Route exact path="/auth/login" component={Login}/>
              <Route exact path="/auth/registrar" component={RegistrarUsuario}/>
              <RutaSegura
                exact
                path="/auth/perfil"
                component={PerfilUsuario}
              />
              <RutaSegura
                exact
                path="/"
                component={PerfilUsuario}
              />
              <RutaSegura
                exact
                path="/curso/nuevo"
                component={NuevoCurso}
              />
            </Switch>
          </Grid>
        </MuithemeProvider>
      </Router>
    </React.Fragment>
  );
}

export default App;
