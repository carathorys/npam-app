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
    const data = new URLSearchParams();
    data.append('client_id', 'js');
    data.append('grant_type', 'password');
    data.append('scope', 'openid profile');
    data.append('username', this.state.loginName);
    data.append('password', this.state.password);
    data.append('client_secret',  (await this.sha256('secret123')).toString());

    let httpResult = await fetch(`/connect/token`, {
      method: 'post',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        [this.state.csrfTokenHeaderName]: this.state.csrfTokenValue,
      },
      body: data,
      redirect: 'manual'
    });
    try {
      let result = await httpResult.json();
      if (result?.success !== true) {
        this.setState({
          ...this.state,
          hasError: true,
          errorMessage: result.message,
          loading: false,
        });
      } else {
        // TODO: Logged in!?
      }
    } catch {
      this.setState({
        ...this.state,
        hasError: true,
        errorMessage: 'Possible server error!',
        loading: false,
      });
      return;
    }
  }

  fieldChanged(field: keyof LoginState) {
    return (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
      this.setState({
        ...this.state,
        hasError: false,
        [field]: event.target.value,
      });
    };
  }

  //////////////////////////////////////////////////////////////////////
  // PKCE HELPER FUNCTIONS

  // Generate a secure random string using the browser crypto functions
  generateRandomString(length: number = 128) {
    var array = new Uint32Array(length);
    window.crypto.getRandomValues(array);
    return Array.from(array, (dec) => ('0' + dec.toString(16)).substr(-2)).join('');
  }

  // Calculate the SHA256 hash of the input text.
  // Returns a promise that resolves to an ArrayBuffer
  async sha256(plain: string): Promise<ArrayBuffer> {
    const encoder = new TextEncoder();
    const data = encoder.encode(plain);
    return await window.crypto.subtle.digest('SHA-256', data);
  }

  // Base64-urlencodes the input string
  base64urlencode(str: ArrayBuffer): string {
    // Convert the ArrayBuffer to string using Uint8 array to conver to what btoa accepts.
    // btoa accepts chars only within ascii 0-255 and base64 encodes them.
    // Then convert the base64 encoded to base64url encoded
    //   (replace + with -, replace / with _, trim trailing =)
    return btoa(String.fromCharCode.apply(null, new Uint8Array(str)))
      .replace(/\+/g, '-')
      .replace(/\//g, '_')
      .replace(/=+$/, '');
  }

  // Return the base64-urlencoded sha256 hash for the PKCE challenge
  async pkceChallengeFromVerifier(v) {
    const hashed = await this.sha256(v);
    return this.base64urlencode(hashed);
  }

  async componentDidMount() {
    const verifier = this.generateRandomString(32);
    this.setState({ ...this.state, pkcsCodeVerifier: verifier });
    await this.fetchCsrfToken();
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
