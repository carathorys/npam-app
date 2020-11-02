import { Theme } from '@material-ui/core/styles';

export const LoginStyles = (theme: Theme) => ({
  root: {
    padding: theme.spacing(3),
  },
  formControl: {
    transform: 'translate3d(0,0,0)',
    perspective: 1000,
    '&.error': {
      animation: 'shake 500ms  cubic-bezier(.36,.07,.19,.97) both'
    }
  },
});
