import React, { createContext, useContext, useEffect, useState } from 'react';

import ThemeProvider from '@mui/material/styles/ThemeProvider';

import { theme } from '../themes';
import CssBaseline from '@mui/material/CssBaseline';

type ThemeType = 'light' | 'dark';

type ContextType = {
  initialized: boolean;
  themeType: ThemeType;
  setThemeType: (type: ThemeType) => void;
};


const ThemeContext = createContext<ContextType>({
  initialized: false,
  themeType: 'light',
  setThemeType: () => {
    throw new Error('Context not initialized yet!');
  },
});

export const ThemeStore: React.FC = ({ children }: React.PropsWithChildren<unknown>) => {
  const [themeType, setThemeType] = useState<ThemeType>('light');
  const currentTheme = React.useMemo(() => {
    return theme(themeType === 'dark');
  }, [themeType]);

  return (
    <ThemeContext.Provider
      value={{
        initialized: true,
        setThemeType,
        themeType,
      }}
    >
      <ThemeProvider theme={currentTheme}>
        <CssBaseline />

        {children}
      </ThemeProvider>
    </ThemeContext.Provider>
  );
};

type UseThemeHook = () => {
  themeType: ThemeType;
  setThemeType: (type: ThemeType) => void;
};

export const useTheme: UseThemeHook = () => {
  const ctx = useContext(ThemeContext);
  const [themeType, updateThemeType] = useState<ThemeType>();
  useEffect(() => {
    if (!ctx.initialized) {
      return;
    }
    updateThemeType(ctx.themeType);
  }, [ctx]);

  return {
    themeType,
    setThemeType: (t: ThemeType) => {
      if (!ctx.initialized) throw new Error('Context is not initialized!');
      ctx.setThemeType(t);
    },
  };
};
