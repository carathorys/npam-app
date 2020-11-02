export class BaseState<TState extends BaseState<TState>> {
  persistentStorage: string = '';
  persistentProperties: (keyof TState)[] = [];
}
