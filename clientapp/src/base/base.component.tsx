import { Component } from 'react';
import { BaseProps } from './base.props';
import { BaseState } from './base.state';
import { getMetadata } from './base.state.decorator';

export abstract class BaseComponent<
  TProps extends BaseProps,
  TState extends BaseState<TState>
> extends Component<TProps, TState> {
  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor(props: TProps, initialStateObject: () => TState) {
    super(props);

    let state = this.GetNewStateInstance();
    const metadata = getMetadata(state);
    state.persistentProperties.push(...metadata.persistProperties);
    const items = JSON.parse(localStorage.getItem(state.persistentStorage) ?? '{}');
    if (items instanceof Object)
      for (let key of metadata.persistProperties) {
        if (!!items[key]) {
          state[key] = items[key];
        }
      }
    this.state = state;
  }

  abstract GetNewStateInstance(): TState;

  setState(state: TState) {
    super.setState(state, () => this.persistState());
  }

  persistState(): void {
    let objectToPersist: Partial<TState> = {};
    for (let key of this.state.persistentProperties) {
      objectToPersist[key] = this.state[key];
    }
    localStorage.setItem(this.state.persistentStorage, JSON.stringify(objectToPersist));
  }
}
