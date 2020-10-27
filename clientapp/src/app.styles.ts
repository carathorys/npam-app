import { createStyles, Theme } from '@material-ui/core/styles';
import { fade } from '@material-ui/core/styles';

export const AppStyles = (theme: Theme) =>
  createStyles({
    header: {
      background: fade(theme.palette.primary.main, 0.45),
      backdropFilter: 'blur(25px)'
    },
  });
