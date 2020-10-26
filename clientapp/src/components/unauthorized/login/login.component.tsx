import { withStyles } from '@material-ui/core';
import React, { PureComponent } from 'react';

import { LoginProps } from './login.props';
import { LoginState } from './login.state';
import { LoginStyles } from './login.styles';

class Login extends PureComponent<LoginProps, LoginState> {
  render() {
    return <h1>Login form will be here!</h1>;
  }
}

export const LoginComponent = withStyles(LoginStyles)(Login);
