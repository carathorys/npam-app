export type IfEquals<X, Y, A = X, B = never> = (<T>() => T extends X ? 1 : 2) extends <
  T
>() => T extends Y ? 1 : 2
  ? A
  : B;

export type WritableKeys<T> = Exclude<
  {
    [P in keyof T]-?: IfEquals<{ [Q in P]: T[P] }, { -readonly [Q in P]: T[P] }, P>;
  }[keyof T],
  'setValue'
>;
export type ReadonlyKeys<T> = Exclude<
  {
    [P in keyof T]-?: IfEquals<{ [Q in P]: T[P] }, { -readonly [Q in P]: T[P] }, never, P>;
  }[keyof T],
  'setValue'
>;
export type TAlias<T> = T | (new () => T);
