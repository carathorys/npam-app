import { darkTheme } from './dark.theme';
import { lightTheme } from './light.theme';
import { createMuiTheme } from '@material-ui/core/styles';

// Create a theme instance.
export const theme = (prefersDarkMode: boolean) =>
  createMuiTheme(prefersDarkMode ? darkTheme : lightTheme);
