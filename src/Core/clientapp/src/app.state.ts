import { Theme } from '@material-ui/core';
import { BaseState } from './base/base.state';
import { PersistState, Persistent } from './base/base.state.decorator';

@PersistState('AppState')
export class AppState extends BaseState<AppState> {
  @Persistent<AppState>(true)
  darkMode: boolean;

  @Persistent<AppState>(true)
  theme: Theme;
}
