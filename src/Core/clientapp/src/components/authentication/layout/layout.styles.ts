import { createStyles, Theme } from '@material-ui/core/styles';
import { fade } from '@material-ui/core/styles';

export const LayoutStyles = (theme: Theme) =>
  createStyles({
    root: {
      paddingTop: theme.spacing(5),
    },
    image: {
      backgroundColor: theme.palette.background.default,
      backgroundImage: `url(images/background.jpg)`,
      backgroundRepeat: 'no-repeat',
      backgroundSize: 'cover',
      backgroundPosition: 'center center',
      width: '100vw',
      height: '100vh',
      top: 0,
      position: 'fixed',
      zIndex: -1,
    },
    paper: {
      background: fade(theme.palette.background.paper, 0.2),
      backdropFilter: 'blur(50px)',
    },
  });
