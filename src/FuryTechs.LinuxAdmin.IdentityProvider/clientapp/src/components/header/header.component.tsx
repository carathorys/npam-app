import React from 'react';

import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Bright from '@mui/icons-material/Brightness7';
import Dark from '@mui/icons-material/Brightness3';

import { headerStyle } from './header.styles';
import { useTheme } from '../../contexts';

export const HeaderComponent = () => {
  const { themeType, setThemeType } = useTheme();
  return (
    <AppBar position="static" sx={headerStyle} elevation={15}>
      <Toolbar>
        <Typography variant="h6">Some site title</Typography>
        <Box sx={{ flex: 1 }} />
        <IconButton color="inherit" onClick={() => setThemeType(themeType === 'light' ? 'dark' : 'light')}>
          {themeType === 'light' ? <Dark /> : <Bright />}
        </IconButton>
      </Toolbar>
    </AppBar>
  );
};
