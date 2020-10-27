import React, { PureComponent } from 'react';

import { Grid, TextField, withStyles } from '@material-ui/core';

import { LoginProps } from './login.props';
import { LoginState } from './login.state';
import { LoginStyles } from './login.styles';

class Login extends PureComponent<LoginProps, LoginState> {
  render() {
    const { classes } = this.props;
    return (
      <Grid container className={classes.root}>
        <Grid item xs={12}>
          <TextField placeholder='User name' fullWidth />
        </Grid>
        <Grid item xs={12}>
          <TextField placeholder='Password' type='password' fullWidth />
        </Grid>
        <Grid item xs={12}></Grid>
      </Grid>
    );
  }
}

export const LoginComponent = withStyles(LoginStyles)(Login);
