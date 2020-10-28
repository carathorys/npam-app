import { WithStyles } from '@material-ui/core';
import { RouteComponentProps } from 'react-router-dom';
import { BaseProps } from '../../../base';

import { LayoutStyles } from './layout.styles';

export interface LayoutProps
  extends WithStyles<typeof LayoutStyles>,
    BaseProps,
    RouteComponentProps {}
