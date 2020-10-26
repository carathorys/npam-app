import { createStyles, Theme } from '@material-ui/core/styles';

export const LayoutStyles = (theme: Theme) =>
  createStyles({
    root: {
      height: '100vh',
    },
    image: {
      backgroundColor: '#ddd',
      backgroundImage: 'url(https://source.unsplash.com/1920x1080/weekly?aurora,borealis)',
      backgroundRepeat: 'no-repeat',
      backgroundSize: 'cover',
      backgroundPosition: 'center center',
    },
    paper: {
      margin: theme.spacing(4, 2),
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
    },
  });
