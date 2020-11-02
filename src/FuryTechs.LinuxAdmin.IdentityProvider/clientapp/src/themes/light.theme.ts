import { ThemeOptions } from '@material-ui/core';
import { red } from '@material-ui/core/colors';

export const lightTheme: ThemeOptions = {
  palette: {
    primary: {
      main: '#00ACE9',
      contrastText: '#fff',
    },
    secondary: {
      main: '#ADDA43',
      contrastText: '#000',
    },
    error: {
      main: red.A400,
    },
    type: 'light',
  },
};
