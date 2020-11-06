import { createStyles, Theme } from '@material-ui/core/styles';
import { fade } from '@material-ui/core/styles';

export const LayoutStyles = (theme: Theme) =>
  createStyles({
    root: {
      paddingTop: theme.spacing(5),
    },
    
    paper: {
      background: fade(theme.palette.background.paper, 0.3),
      backdropFilter: 'blur(50px)',
    },
  });
