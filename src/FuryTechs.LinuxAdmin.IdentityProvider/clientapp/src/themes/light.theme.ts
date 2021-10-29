import { ThemeOptions } from '@mui/material';
import { red } from '@mui/material/colors';

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
    mode: 'light',
  },
};
