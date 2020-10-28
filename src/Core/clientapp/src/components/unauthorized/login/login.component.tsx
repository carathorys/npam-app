import React, { ChangeEvent, MouseEvent } from 'react';

import { Button, Grid, TextField, withStyles } from '@material-ui/core';

import { BaseComponent } from '../../../base';

import { LoginProps } from './login.props';
import { LoginState } from './login.state';
import { LoginStyles } from './login.styles';

import './login.styles.css';

class Login extends BaseComponent<LoginProps, LoginState> {
  GetNewStateInstance(): LoginState {
    return new LoginState();
  }
  async handleLogin(event: MouseEvent<HTMLButtonElement, MouseEvent>): Promise<void> {
    this.setState({ ...this.state, hasError: false });
    let httpResult = await fetch('/api/login', {
      method: 'post',
      headers: {
        'content-type': 'application/json',
      },
      body: JSON.stringify({
        UserName: this.state.loginName,
        Password: this.state.password,
      }),
    });
    let result = await httpResult.json();
    if (result?.success !== true) {
      this.setState({ ...this.state, hasError: true, errorMessage: result.message });
    }
    console.log(result);
  }

  fieldChanged(field: keyof LoginState) {
    return (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
      this.setState({
        ...this.state,
        hasError: false,
        errorMessage: undefined,
        [field]: event.target.value,
      });
    };
  }

  render() {
    const { classes } = this.props;
    return (
      <Grid container className={classes.root}>
        <Grid item xs={12}>
          <TextField
            placeholder='User name'
            variant='outlined'
            value={this.state?.loginName ?? ''}
            onChange={this.fieldChanged('loginName').bind(this)}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            placeholder='Password'
            variant='outlined'
            className={`${classes.formControl} ${this.state.hasError ? 'error' : ''}`}
            value={this.state?.password ?? ''}
            error={this.state.hasError}
            helperText={this.state.errorMessage}
            onChange={this.fieldChanged('password').bind(this)}
            type='password'
          />
        </Grid>
        <Grid item xs={12}>
          <Button onClick={this.handleLogin.bind(this)}>Login</Button>
        </Grid>
      </Grid>
    );
  }
}

export const LoginComponent = withStyles(LoginStyles)(Login);
