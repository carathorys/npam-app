import { BaseState } from './../../../base/base.state';
import { PersistState, Persistent } from '../../../base/base.state.decorator';

@PersistState("UnauthorizedLayoutState")
export class LayoutState extends BaseState<LayoutState> {
  @Persistent<LayoutState>(true)
  loginName?: string;
  @Persistent<LayoutState>(false)
  password?: string;
}
