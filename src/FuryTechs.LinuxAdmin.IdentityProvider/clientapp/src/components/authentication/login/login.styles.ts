import { Theme, fade, createStyles } from '@material-ui/core/styles';

export const LoginStyles = (theme: Theme) =>
  createStyles({
    formControl: {
      'transform': 'translate3d(0,0,0)',
      'perspective': 1000,
      '&.error': {
        animation: 'shake 500ms  cubic-bezier(.36,.07,.19,.97) both',
      },
    },
    wrapper: {
      position: 'relative',
      margin: theme.spacing(3, 0, 2),
      overflow: 'hidden',
      borderRadius: theme.shape.borderRadius,
    },
    loader: {
      left: '50%',
      position: 'absolute',
      top: '50%',
      marginTop: theme.spacing(-1.5),
      marginLeft: theme.spacing(-1.5),
    },
    paper: {
      'marginTop': theme.spacing(8),
      'backgroundColor': fade(theme.palette.background.paper, 0.8),
      'backdropFilter': 'blur(25px)',
      'display': 'flex',
      'padding': theme.spacing(5),
      'flex-direction': 'column',
      'alignItems': 'center',
      'overflow': 'hidden',
    },
    avatar: {
      margin: theme.spacing(1),
      backgroundColor: theme.palette.secondary.main,
    },
    form: {
      width: '100%', // Fix IE 11 issue.
      marginTop: theme.spacing(1),
    },
  });
