import { Grid, Paper } from '@mui/material';
import { Route, Switch, useRouteMatch } from 'react-router-dom';

import { imageStyle, paperStyle, rootStyle } from './layout.styles';
import { Box } from '@mui/system';

export const LayoutComponent = () => {
  const match = useRouteMatch();
  return (
    <Box sx={rootStyle}>
      <Box sx={imageStyle} />
      <Grid container>
        <Grid item xs={false} sm={1} md={2}>
          &nbsp;
        </Grid>
        <Grid item xs={12} sm={10} md={8} component={Paper} sx={paperStyle} elevation={15}>
          <Switch>
            <Route path={match.path}>
              <h1>Authentication works!</h1>
            </Route>
          </Switch>
        </Grid>
        <Grid item xs={false} sm={1} md={2} />
      </Grid>
    </Box>
  );
};
