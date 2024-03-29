import createTheme from '@mui/material/styles/createTheme';
import { darkTheme } from './dark.theme';
import { lightTheme } from './light.theme';


// Create a theme instance.
export const theme = (prefersDarkMode: boolean) =>
  createTheme(prefersDarkMode ? darkTheme : lightTheme);
