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

function App() {

  const [{sesionUsuario,openSnackbar},dispatch] = useStateValue();
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


  return(
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
              <Route exact path="/auth/perfil" component={PerfilUsuario}/>
              <Route exact path="/" component={PerfilUsuario}/>
            </Switch>
          </Grid>
        </MuithemeProvider>
      </Router>
    </React.Fragment>
  );
}

export default App;
