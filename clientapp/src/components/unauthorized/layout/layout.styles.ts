import { createStyles, Theme } from '@material-ui/core/styles';
import { Height } from '@material-ui/icons';

export const LayoutStyles = (theme: Theme) =>
  createStyles({
    root: {
      height: '100vh',
      position: 'relative'
    },
    image: {
      backgroundColor: '#ddd',
      backgroundImage: 'url(https://source.unsplash.com/1920x1080/weekly?aurora,borealis)',
      backgroundRepeat: 'no-repeat',
      backgroundSize: 'cover',
      backgroundPosition: 'center center',
      width: '100vw',
      height: '100vh',
      position: 'fixed',
    },
    paper: {
      position: 'absolute',
      height: '50%',
      width: '50%',
      left: '25%',
      top: '25%',
      backdropFilter: "blur(50px)"
    },
  });
