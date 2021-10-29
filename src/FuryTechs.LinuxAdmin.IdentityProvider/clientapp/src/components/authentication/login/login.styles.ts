import { alpha } from '@mui/material';
import { Css } from '../../../utils/Css.type';

export const formStyle: Css = {
  width: '100%', // Fix IE 11 issue.
  marginTop: 1,
};

export const formControlStyle: Css = {
  transform: 'translate3d(0,0,0)',
  perspective: 1000,
  '&.error': {
    animation: 'shake 500ms  cubic-bezier(.36,.07,.19,.97) both',
  },
};
export const wrapperStyle: Css = {
  position: 'relative',
  margin: (theme) => theme.spacing(3, 0, 2),
  overflow: 'hidden',
  borderRadius: (theme) => theme.shape.borderRadius,
};

export const loaderStyle: Css = {
  left: '50%',
  position: 'absolute',
  top: '50%',
  marginTop: -1.5,
  marginLeft: -1.5,
};
export const paperStyle: Css = {
  mt: 8,
  backgroundColor: (theme) => alpha(theme.palette.background.paper, 0.8),
  backdropFilter: 'blur(25px)',
  display: 'flex',
  p: 5,
  flexDirection: 'column',
  alignItems: 'center',
  overflow: 'hidden',
};
export const avatarStyle = {
  m: 1,
  backgroundColor: (theme) => theme.palette.secondary.main,
};
