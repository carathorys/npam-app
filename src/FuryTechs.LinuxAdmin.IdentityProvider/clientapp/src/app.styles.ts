import { createStyles, Theme } from '@material-ui/core/styles';
import { fade } from '@material-ui/core/styles';

export const AppStyles = (theme: Theme) =>
  createStyles({
    header: {
      backdropFilter: 'blur(25px)'
    },
    image: {
      backgroundColor: theme.palette.background.default,
      backgroundImage: `url(images/background.jpg)`,
      backgroundRepeat: 'no-repeat',
      backgroundSize: 'cover',
      backgroundPosition: 'center center',
      transition: theme.transitions.easing.easeInOut,
      width: '100vw',
      height: '100vh',
      top: 0,
      position: 'fixed',
      zIndex: -1,
    },
  });
