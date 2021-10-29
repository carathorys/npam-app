
import { Grid } from '@mui/material';

import { rootStyle } from './layout.styles';
import { LoginComponent } from '../login/login.component';
import { Box } from '@mui/system';

export const LayoutComponent = () => {
  return (
    <Box sx={rootStyle}>
      <Grid container>
        <Grid item xs={false} sm={1} md={2}>
          &nbsp;
        </Grid>
        <Grid item xs={12} sm={10} md={8}>
          <LoginComponent />
        </Grid>
        <Grid item xs={false} sm={1} md={2} />
      </Grid>
    </Box>
  );
};
