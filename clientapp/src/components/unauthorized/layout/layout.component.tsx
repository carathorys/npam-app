import React, { ChangeEvent } from 'react';

import { Grid, Paper, withStyles } from '@material-ui/core';
import { Route, Switch, withRouter } from 'react-router-dom';
import { BaseComponent } from '../../../base';

import { LayoutProps } from './layout.props';
import { LayoutState } from './layout.state';
import { LayoutStyles } from './layout.styles';
import { LoginComponent } from '../login/login.component';

class Layout extends BaseComponent<LayoutProps, LayoutState> {
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
    const { classes, match } = this.props;

    return (
      <div className={classes.root}>
        <div className={classes.image} />
        <Grid container>
          <Grid item xs={false} sm={1} md={2}>
            &nbsp;
          </Grid>
          <Grid
            item
            xs={12}
            sm={10}
            md={8}
            component={Paper}
            className={classes.paper}
            elevation={15}>
            <Switch>
              <Route path={match.path} exact>
                <LoginComponent />
              </Route>
            </Switch>
          </Grid>
          <Grid item xs={false} sm={1} md={2} />
        </Grid>
      </div>
    );
  }
}

export const LayoutComponent = withStyles(LayoutStyles)(withRouter(Layout));
