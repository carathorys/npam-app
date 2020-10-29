import React from 'react';

import {Grid, Paper, withStyles} from '@material-ui/core';
import {Route, Switch, withRouter} from 'react-router-dom';
import {BaseComponent} from '../../../base';

import {LayoutProps} from './layout.props';
import {LayoutState} from './layout.state';
import {LayoutStyles} from './layout.styles';
import {LoginComponent} from '../login/login.component';

class Layout extends BaseComponent<LayoutProps, LayoutState> {
    static displayName = Layout.name
    GetNewStateInstance(): LayoutState {
        return new LayoutState();
    }

    render() {
        const {classes} = this.props;

        return (
            <div className={classes.root}>
                <div className={classes.image}/>
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
                        <LoginComponent/>
                    </Grid>
                    <Grid item xs={false} sm={1} md={2}/>
                </Grid>
            </div>
        );
    }
}

export const LayoutComponent = withStyles(LayoutStyles)(withRouter(Layout));
