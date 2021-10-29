import { alpha } from '@mui/system';
import { Css } from '../../../utils/Css.type';

export const rootStyle: Css = {
  pt: 5,
};
export const imageStyle: Css = {
  backgroundColor: (theme) => theme.palette.background.default,
  backgroundImage: 'url(images/background.jpg)',
  backgroundRepeat: 'no-repeat',
  backgroundSize: 'cover',
  backgroundPosition: 'center center',
  width: '100vw',
  height: '100vh',
  top: 0,
  position: 'fixed',
  zIndex: -1,
};
export const paperStyle: Css = {
  background: (theme) => alpha(theme.palette.background.paper, 0.2),
  backdropFilter: 'blur(50px)',
};
