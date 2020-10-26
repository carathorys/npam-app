import React, { ChangeEvent } from 'react';

import { TextField, ThemeProvider, withStyles } from '@material-ui/core';
import { BaseComponent } from '../../../base';

import { theme } from '../../../themes';

import { LayoutProps } from './layout.props';
import { LayoutState } from './layout.state';
import { LayoutStyles } from './layout.styles';

class Layout extends BaseComponent<LayoutProps, LayoutState> {
  constructor(props: LayoutProps) {
    super(props, () => new LayoutState());
  }

  loginNameChange(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>): void {
    this.setState({ ...this.state, loginName: event.target.value });
  }
  passwordChange(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>): void {
    this.setState({ ...this.state, password: event.target.value });
  }

  GetNewStateInstance(): LayoutState {
    return new LayoutState();
  }

  render() {
    return (
      <ThemeProvider theme={theme(false)}>
        <TextField
          value={this.state?.loginName ?? ''}
          title='Login name'
          onChange={this.loginNameChange.bind(this)}></TextField>{' '}
        <TextField
          value={this.state?.password ?? ''}
          title='Password'
          type='password'
          onChange={this.passwordChange.bind(this)}></TextField>
      </ThemeProvider>
    );
  }
}

export const LayoutComponent = withStyles(LayoutStyles)(Layout);
