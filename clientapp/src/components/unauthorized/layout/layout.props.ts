import { WithStyles } from '@material-ui/core';
import { BaseProps } from '../../../base';

import { LayoutStyles } from './layout.styles';

export interface LayoutProps extends WithStyles<typeof LayoutStyles>, BaseProps {}
