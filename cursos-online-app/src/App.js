import React from 'react';
import {ThemeProvider as MuithemeProvider} from '@material-ui/core/styles';
import theme from './theme/theme';
//import RegistrarUsuario from './components/seguridad/RegistrarUsuario';
import PerfilUsuario from './components/seguridad/PerfilUsuario';
//import Login from './components/seguridad/Login';

//<RegistrarUsuario/>
//<Login/>
function App() {
  return(
    <MuithemeProvider theme={theme}>
        <PerfilUsuario/>
    </MuithemeProvider>
    
  );
}

export default App;
