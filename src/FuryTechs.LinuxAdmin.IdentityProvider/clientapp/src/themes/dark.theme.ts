import { darken, ThemeOptions } from '@mui/material';
import { red } from '@mui/material/colors';

export const darkTheme: ThemeOptions = {
  palette: {
    primary: {
      main: darken('#00ACE9', 0.5),
      contrastText: '#fff',
    },
    secondary: {
      main: darken('#ADDA43', 0.2),
      contrastText: '#000',
    },
    error: {
      main: red.A700,
    },

    mode: 'dark',
  },
};
