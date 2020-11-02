import {
    AppBar,
    CssBaseline,
    IconButton,
    ThemeProvider,
    Toolbar,
    Typography,
    withStyles,
} from '@material-ui/core';
import React from 'react';
import {Switch, Route} from 'react-router-dom';
import {Brightness3 as Dark, Brightness7 as Bright} from '@material-ui/icons';

import {theme} from './themes';
import {BaseComponent} from './base';

import {AppState} from './app.state';
import {AppProps} from './app.props';

import {AuthenticationLayout, AuthorizedLayout} from './components';
import {AppStyles} from './app.styles';
import {AuthorizeRoute} from './components/common/authorize-route/authorize-route.component';

class App extends BaseComponent<AppProps, AppState> {
    constructor(props) {
        super(props);
        this.state = {...this.state, theme: theme(this.state?.darkMode ?? false)};
    }

    GetNewStateInstance(): AppState {
        return new AppState();
    }

    toggleDarkMode() {
        this.setState({
            ...this.state,
            darkMode: !this.state.darkMode,
            theme: theme(!this.state.darkMode),
        });
    }

    render() {
        const {darkMode, theme} = this.state;
        const {classes} = this.props;
        return (
            <ThemeProvider theme={theme}>
                <AppBar position='static' className={classes.header} elevation={15}>
                    <Toolbar>
                        <Typography variant='h6'>Some site title</Typography>
                        <IconButton color='inherit' onClick={this.toggleDarkMode.bind(this)}>
                            {!darkMode ? <Dark/> : <Bright/>}
                        </IconButton>
                    </Toolbar>
                </AppBar>
                <CssBaseline/>
                <Switch>
                    <Route path='/' exact>
                        <AuthorizeRoute component={AuthorizedLayout}/>
                    </Route>
                    <Route path='/authentication/'>
                        <AuthenticationLayout/>
                    </Route>
                </Switch>
            </ThemeProvider>
        );
    }
}

export const AppComponent = withStyles(AppStyles)(App);
