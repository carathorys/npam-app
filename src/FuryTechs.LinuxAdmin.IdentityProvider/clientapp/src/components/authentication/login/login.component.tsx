import React, { MouseEvent } from 'react';

import {
  Avatar,
  Button,
  Checkbox,
  CircularProgress,
  Container,
  FormControlLabel,
  Paper,
  TextField,
  Typography,
  withStyles,
} from '@material-ui/core';
import { LockOutlined } from '@material-ui/icons';

import { BaseComponent } from '../../../base';

import { LoginProps } from './login.props';
import { LoginState } from './login.state';
import { LoginStyles } from './login.styles';

import { authService } from '../../../services';
import './login.styles.css';

class Login extends BaseComponent<LoginProps, LoginState> {
  GetNewStateInstance(): LoginState {
    return new LoginState();
  }

  async fetchCsrfToken() {
    const http = await fetch('/api/authentication');
    const json = await http.json();

    this.setState({
      ...this.state,
      csrfTokenHeaderName: json.headerName,
      csrfTokenValue: json.requestToken,
    });
  }

  async handleLogin(event: MouseEvent<HTMLButtonElement, MouseEvent>): Promise<void> {
    this.setState({ ...this.state, hasError: false, loading: true });
    var tokenRequest = await fetch('account/generate', { method: 'get' });
    if (tokenRequest.ok !== true) {
      this.setState({
        ...this.state,
        hasError: true,
        loading: false,
        errorMessage: 'Invalid username or password!',
      });
      return;
    }
    const cfrs = await tokenRequest.json();
    const data = new URLSearchParams();
    data.append('client_id', 'js');
    data.append('grant_type', 'password');
    data.append('scope', 'openid profile');
    data.append('username', this.state.loginName);
    data.append('password', this.state.password);
    data.append('rememberLogin', this.state.remember.toString());

    let httpResult = await fetch(`/account/login`, {
      method: 'post',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        [cfrs.headerName]: cfrs.requestToken,
      },
      body: data,
      redirect: 'manual',
    });

    // const loginResult = await authService.LogIn(this.state.loginName, this.state.password);
    // if (loginResult !== true) {
    this.setState({
      ...this.state,
      hasError: true,
      loading: false,
      errorMessage: 'Invalid username or password!',
    });
    // } else {
    //   this.setState({ ...this.state, hasError: false, loading: false });
    // }
  }

  async componentDidMount() {
    await this.fetchCsrfToken();
  }

  getKeysToStore(): Array<keyof LoginState> {
    const baseValue = super.getKeysToStore();
    if (this.state.remember === true) {
      baseValue.push('loginName');
      baseValue.push('password');
    }
    return baseValue;
  }

  render() {
    const { classes } = this.props;
    return (
      <Container component='main' maxWidth='xs'>
        <Paper className={classes.paper} elevation={15}>
          <Avatar className={classes.avatar}>
            <LockOutlined />
          </Avatar>
          <Typography component='h1' variant='h5'>
            Sign in
          </Typography>

          <form className={classes.form}>
            <TextField
              placeholder='User name'
              variant='outlined'
              margin='normal'
              required
              fullWidth
              autoFocus
              disabled={this.state.loading}
              value={this.state?.loginName ?? ''}
              onChange={this.fieldChanged('loginName').bind(this)}
            />
            <TextField
              placeholder='Password'
              variant='outlined'
              required
              fullWidth
              className={`${classes.formControl} ${this.state.hasError ? 'error' : ''}`}
              value={this.state?.password ?? ''}
              error={this.state.hasError}
              disabled={this.state.loading}
              helperText={this.state.errorMessage}
              onChange={this.fieldChanged('password').bind(this)}
              type='password'
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={this.state.remember === true}
                  disabled={this.state.loading}
                  onChange={this.fieldChanged('remember', (event) => !this.state.remember).bind(
                    this,
                  )}
                  color='primary'
                />
              }
              label='Remember me'
            />
            <div className={classes.wrapper}>
              <Button
                fullWidth
                variant='contained'
                color='primary'
                disabled={this.state.loading}
                onClick={this.handleLogin.bind(this)}>
                Login
              </Button>
              {this.state.loading && (
                <div className={classes.loader}>
                  <CircularProgress
                    thickness={5}
                    size={24}
                    color='primary'
                    variant='indeterminate'
                  />
                </div>
              )}
            </div>
          </form>
        </Paper>
      </Container>
    );
  }
}

export const LoginComponent = withStyles(LoginStyles)(Login);
