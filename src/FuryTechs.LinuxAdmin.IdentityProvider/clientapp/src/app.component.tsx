import React from 'react';
import { Switch, Route } from 'react-router-dom';

import { imageStyle } from './app.styles';
import Box from '@mui/material/Box';

import { ThemeStore } from './contexts';
import { AuthenticationLayout, AuthorizedLayout, HeaderComponent } from './components';

import { AuthorizeRoute } from './components/common/authorize-route/authorize-route.component';

export const AppComponent = () => {

  return (
    <ThemeStore>
      <Box sx={imageStyle} />
      <HeaderComponent />
      <Switch>
        <Route path="/" exact>
          <AuthorizeRoute component={AuthorizedLayout} />
        </Route>
        <Route path="/authentication/">
          <AuthenticationLayout />
        </Route>
      </Switch>
    </ThemeStore>
  );
};
