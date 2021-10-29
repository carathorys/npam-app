import { Theme } from '@mui/material';
import { BaseState } from './base/base.state';
import { PersistState, Persistent } from './base/base.state.decorator';

@PersistState('AppState')
export class AppState extends BaseState<AppState> {
  @Persistent<AppState>(true)
  darkMode: boolean;

  @Persistent<AppState>(false)
  theme: Theme;
}
