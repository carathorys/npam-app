import { WebStorageStateStore } from 'oidc-client';
import { ChangeEvent, Component } from 'react';
import { BaseProps } from './base.props';
import { BaseState } from './base.state';
import { getMetadata } from './base.state.decorator';

export abstract class BaseComponent<
  TProps extends BaseProps,
  TState extends BaseState<TState>
> extends Component<TProps, TState> {
  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor(props: TProps) {
    super(props);

    let state = this.GetNewStateInstance();
    const metadata = getMetadata(state);
    if (!!metadata) {
      state.persistentProperties.push(...metadata?.persistProperties);
      const items = JSON.parse(localStorage.getItem(state.persistentStorage) ?? '{}');
      if (items instanceof Object)
        for (let key of Object.keys(items)) {
          if (!!items[key]) {
            state[key] = items[key];
          }
        }
    }
    this.state = state;
  }

  abstract GetNewStateInstance(): TState;

  protected fieldChanged(
    field: keyof TState,
    valueSelector: (e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => string | boolean = (
      e: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>,
    ) => e.target.value,
  ) {
    return (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
      this.setState({
        ...this.state,
        hasError: false,
        [field]: valueSelector(event),
      });
    };
  }

  setState(state: TState) {
    super.setState(state, () => this.persistState());
  }

  protected getKeysToStore(): Array<keyof TState> {
    return this.state.persistentProperties;
  }

  persistState(): void {
    let objectToPersist: Partial<TState> = {};
    for (let key of this.getKeysToStore()) {
      objectToPersist[key] = this.state[key];
    }

    localStorage.setItem(this.state.persistentStorage, JSON.stringify(objectToPersist));
  }
}
