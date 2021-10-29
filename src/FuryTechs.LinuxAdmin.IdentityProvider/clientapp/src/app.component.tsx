import { AppBar, Box, CssBaseline, IconButton, Theme, ThemeProvider, Toolbar, Typography } from '@mui/material';
import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { Brightness3 as Dark, Brightness7 as Bright } from '@mui/icons-material';

import { theme } from './themes';

import { AuthenticationLayout, AuthorizedLayout } from './components';
import { AuthorizeRoute } from './components/common/authorize-route/authorize-route.component';

import { headerStyle, imageStyle } from './app.styles';

export const AppComponent = () => {
  const [darkMode, updateDarkMode] = React.useState(false);
  const currentTheme = React.useMemo(() => {
    return theme(darkMode);
  }, [darkMode]);

  return (
    <React.Fragment>
      <Box sx={imageStyle} />
      <ThemeProvider theme={currentTheme}>
        <CssBaseline />
        <AppBar position="static" sx={headerStyle} elevation={15}>
          <Toolbar>
            <Typography variant="h6">Some site title</Typography>
            <Box sx={{ flex: 1 }} />
            <IconButton color="inherit" onClick={() => updateDarkMode((prev) => !prev)}>
              {!darkMode ? <Dark /> : <Bright />}
            </IconButton>
          </Toolbar>
        </AppBar>
        <CssBaseline />
        <Switch>
          <Route path="/" exact>
            <AuthorizeRoute component={AuthorizedLayout} />
          </Route>
          <Route path="/authentication/">
            <AuthenticationLayout />
          </Route>
        </Switch>
      </ThemeProvider>
    </React.Fragment>
  );
};
