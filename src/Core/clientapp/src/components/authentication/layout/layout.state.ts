import { BaseState } from './../../../base/base.state';
import { PersistState, Persistent } from '../../../base/base.state.decorator';

@PersistState('LayoutState')
export class LayoutState extends BaseState<LayoutState> {}
