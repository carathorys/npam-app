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
    this.setState({ ...this.state, loginName: event.target.value });
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
      <Grid container component='main' className={classes.root}>
        <CssBaseline />
        <Grid item xs={false} sm={4} md={7} className={classes.image} />
        <Grid item xs={12} sm={8} md={5} component={Paper} elevation={6}>
          <div className={classes.paper}>{children}</div>
          <Box mt={5}>
            <Copyright />
          </Box>
        </Grid>
      </Grid>
    );
  }
}

export const LayoutComponent = withStyles(LayoutStyles)(Layout);
