import sesionUsuarioReducer from './sesionUsuarioReducer';
import openSnackbarReducer from './openSnackBarReducer';

export const mainReducer = ({sesionUsuario,openSnackbar},action) => {
    return {
        sesionUsuario   : sesionUsuarioReducer(sesionUsuario,action),
        openSnackbar    : openSnackbarReducer(openSnackbar,action)
    }
}