/* eslint-disable no-sequences */
import 'reflect-metadata';
import { BaseState } from './base.state';

const persistProperty = Symbol('persistProperty');

type PersistStateMetadata<T> = {
  persistProperties: (keyof T)[];
};

export function isPropertyPersistent<T>(target: T, propertyKey: keyof T): boolean {
  const data = Reflect.getMetadata(persistProperty, target, propertyKey.toString());
  return data === true;
}

export function getMetadata<T>(target: T): PersistStateMetadata<T> {
  const data = Reflect.getMetadata(persistProperty, target);
  return data;
}

export function PersistState<T extends BaseState<T>, TCtor extends { new (...args: any[]): any }>(
  persistentStorageName: string,
) {
  return (target: TCtor) => {
    return class extends target {
      persistentStorage: string = persistentStorageName;
    };
  };
}

export function Persistent<T>(isPersistent: boolean): {
  (target: T, key: keyof T): void;
  (target: T, key: keyof T): void;
} {
  function classLevelMetadata(target: T) {
    // ignore
  }

  function propertyLevelMetadata(target: T, key: keyof T): void {
    const existing = (Reflect.getOwnMetadata(persistProperty, target) as PersistStateMetadata<T>) || {
      persistProperties: [],
    };
    if (isPersistent) {
      existing.persistProperties.push(key);
      Reflect.defineMetadata(persistProperty, existing, target);
      Reflect.defineMetadata(persistProperty, isPersistent, target, key.toString());
    }
  }
  return (target: T, key: keyof T) => {
    // eslint-disable-next-line @typescript-eslint/no-unused-expressions
    classLevelMetadata(target), propertyLevelMetadata(target, key);
  };
}
