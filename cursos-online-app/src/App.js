import React from 'react';
import {ThemeProvider as MuithemeProvider} from '@material-ui/core/styles';
import theme from './theme/theme';
import PerfilUsuario from './components/seguridad/PerfilUsuario';
//import RegistrarUsuario from './components/seguridad/RegistrarUsuario';
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
