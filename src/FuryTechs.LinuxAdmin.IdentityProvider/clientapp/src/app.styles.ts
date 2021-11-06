import { Css } from './utils/Css.type';

export const imageStyle: Css = {
  backgroundColor: (currentTheme) => currentTheme.palette.background.default,
  backgroundImage: 'url(images/background.jpg)',
  backgroundRepeat: 'no-repeat',
  backgroundSize: 'cover',
  backgroundPosition: 'center center',
  transition: (currentTheme) => currentTheme.transitions.easing.easeInOut,
  width: '100vw',
  height: '100vh',
  top: 0,
  position: 'fixed',
  zIndex: -1,
};
