import React from 'react';
import {Component} from 'react';
import {Route, Redirect} from 'react-router-dom';
import {CircularProgress} from '@material-ui/core'
import {ApplicationPaths, QueryParameterNames} from '../../../constants';
import {AuthorizeRouteState} from './authorize-route.state';
import {authService} from '../../../services';

export class AuthorizeRoute extends Component<any, AuthorizeRouteState> {
    constructor(props) {
        super(props);

        this.state = {
            ready: false,
            authenticated: false,
        };
    }

    _subscription?: number;

    componentDidMount() {
        this._subscription = authService.subscribe(() => this.authenticationChanged());
        this.populateAuthenticationState();
    }

    componentWillUnmount() {
        authService.unsubscribe(this._subscription);
    }

    render() {
        const {ready, authenticated} = this.state;
        var link = document.createElement('a');
        const returnUrl = `${window.location.protocol}//${window.location.host}${window.location.pathname ?? ''}${window.location.search ?? ''}${window.location.hash ?? ''}`;
        const redirectUrl = `${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURIComponent(
            returnUrl,
        )}`;
        if (!ready) {
            return <CircularProgress/>;
        } else {
            const {component: Component, ...rest} = this.props;
            return (
                <Route
                    {...rest}
                    render={(props) => {
                        if (authenticated) {
                            return <Component {...props} />;
                        } else {
                            return <Redirect to={redirectUrl}/>;
                        }
                    }}
                />
            );
        }
    }

    async populateAuthenticationState() {
        try {
            const authenticated = await authService.isAuthenticated();
            this.setState({ready: true, authenticated});
        } catch (error) {
            this.setState({ready: true, authenticated: false});
        }
    }

    async authenticationChanged() {
        this.setState({ready: false, authenticated: false});
        await this.populateAuthenticationState();
    }
}
