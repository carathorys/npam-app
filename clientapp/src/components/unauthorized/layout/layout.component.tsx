import React, { ChangeEvent } from 'react';

import { Box, CssBaseline, Grid, Paper, withStyles } from '@material-ui/core';
import { BaseComponent } from '../../../base';

import { LayoutProps } from './layout.props';
import { LayoutState } from './layout.state';
import { LayoutStyles } from './layout.styles';
import { Copyright } from '../../common';

class Layout extends BaseComponent<LayoutProps, LayoutState> {
  constructor(props: LayoutProps) {
    super(props, () => new LayoutState());
  }

  loginNameChange(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>): void {
    this.setState({ ...this.state, userName: event.target.value });
  }
  passwordChange(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>): void {
    this.setState({ ...this.state, password: event.target.value });
  }

  GetNewStateInstance(): LayoutState {
    return new LayoutState();
  }

  render() {
    const { children, classes } = this.props;
    return (
      <React.Fragment>
        <CssBaseline />
        <div className={classes.image} />
        <Paper className={classes.paper} elevation={25}>{children}</Paper>
      </React.Fragment>
    );
  }
}

export const LayoutComponent = withStyles(LayoutStyles)(Layout);
