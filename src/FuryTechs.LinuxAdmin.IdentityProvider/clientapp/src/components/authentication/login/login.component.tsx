import React, {useState} from 'react';

import LockOutlined from '@mui/icons-material/LockOutlined';

import {BaseComponent} from '../../../base';

import {LoginState} from './login.state';
import {avatarStyle, formControlStyle, formStyle, loaderStyle, paperStyle, wrapperStyle} from './login.styles';

import {authService} from '../../../services';
import './login.styles.css';
import Container from '@mui/material/Container';
import Paper from '@mui/material/Paper';
import Avatar from '@mui/material/Avatar';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Button from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';

export const LoginComponent = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(false);
  const [remember, setRemember] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [password, setPassword] = useState('');
  const [loginName, setLoginName] = useState('');

  const logIn = async (): Promise<void> => {
    setLoading(true);
    setError(false);

    const loginResult = await authService.LogIn(loginName, password, remember);

    setLoading(false);
    if (loginResult !== true) {
      setError(true);
      setErrorMessage('Invalid username or password!');
    } else {
      // TODO: fetch Access Token
    }
  }

  return (
    <Container component="main" maxWidth="xs">
      <Paper sx={paperStyle} elevation={15}>
        <Avatar sx={avatarStyle}>
          <LockOutlined/>
        </Avatar>
        <Typography component="h1" variant="h5">
          Sign in
        </Typography>

        <Box sx={formStyle}>
          <form>
            <TextField
              placeholder="User name"
              variant="outlined"
              margin="normal"
              required
              fullWidth
              autoFocus
              disabled={loading}
              value={loginName ?? ''}
              onChange={(evt) => setLoginName(evt.target.value)}
            />
            <TextField
              placeholder="Password"
              variant="outlined"
              required
              fullWidth
              sx={formControlStyle}
              className={`${error ? 'error' : ''}`}
              value={password ?? ''}
              error={error}
              disabled={loading}
              helperText={errorMessage}
              onChange={evt => setPassword(evt.target.value)}
              type="password"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={remember}
                  disabled={loading}
                  onChange={() => setRemember((p) => !p)}
                  color="primary"
                />
              }
              label="Remember me"
            />
            <Box sx={wrapperStyle}>
              <Button
                fullWidth
                variant="contained"
                color="primary"
                disabled={loading}
                onClick={logIn}
              >
                Login
              </Button>
              {loading && (
                <Box sx={loaderStyle}>
                  <CircularProgress thickness={5} size={24} color="primary" variant="indeterminate"/>
                </Box>
              )}
            </Box>
          </form>
        </Box>
      </Paper>
    </Container>
  );

}

// export class LoginComponent extends BaseComponent<unknown, LoginState> {
//   GetNewStateInstance(): LoginState {
//     return new LoginState();
//   }
//
//   async handleLogin(/*_: MouseEvent<HTMLButtonElement, MouseEvent>*/): Promise<void> {
//     this.setState({ ...this.state, hasError: false, loading: true });
//
//     const loginResult = await authService.LogIn(this.state.loginName, this.state.password, this.state.remember);
//     if (loginResult !== true) {
//       this.setState({
//         ...this.state,
//         hasError: true,
//         loading: false,
//         errorMessage: 'Invalid username or password!',
//       });
//     } else {
//       this.setState({ ...this.state, hasError: false, loading: false });
//     }
//   }
//
//   getKeysToStore(): Array<{ key: keyof LoginState; remove?: boolean }> {
//     const baseValue = super.getKeysToStore();
//     const shouldRemove = this.state.remember !== true;
//     baseValue.push({ key: 'loginName', remove: shouldRemove });
//     baseValue.push({ key: 'password', remove: shouldRemove });
//     return baseValue;
//   }
//
//   render() {
//   }
// }
