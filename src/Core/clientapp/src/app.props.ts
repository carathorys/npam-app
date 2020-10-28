import { WithStyles } from '@material-ui/core';
import { AppStyles } from './app.styles';
import { BaseProps } from './base/base.props';

export interface AppProps extends BaseProps, WithStyles<typeof AppStyles> {

}
