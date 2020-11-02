import { BaseState } from '../../../base';
import { Persistent, PersistState } from '../../../base/base.state.decorator';

@PersistState('LoginComponent')
export class LoginState extends BaseState<LoginState> {
  @Persistent<LoginState>(true)
  loginName: string;

  @Persistent<LoginState>(false)
  password: string;

  @Persistent<LoginState>(false)
  loading: boolean;

  @Persistent<LoginState>(false)
  hasError: boolean;

  @Persistent<LoginState>(false)
  errorMessage?: string;

  @Persistent<LoginState>(false)
  csrfTokenHeaderName?: string;

  @Persistent<LoginState>(false)
  pkcsCodeVerifier?: string;

  @Persistent<LoginState>(false)
  csrfTokenValue?: string;
}
