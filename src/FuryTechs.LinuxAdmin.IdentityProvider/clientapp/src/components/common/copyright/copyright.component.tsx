import React from 'react';
import { Typography, Link } from '@mui/material';

export const Copyright = () => {
  return (
    <Typography variant="body2" color="textSecondary" align="center">
      {'Copyright © '}
      <Link color="inherit" href="/">
        Balazs Gallay
      </Link>{' '}
      {new Date().getFullYear()}
      {'.'}
    </Typography>
  );
};
