import { DesktopWindowsOutlined } from '@material-ui/icons';
import { RSA_PKCS1_PSS_PADDING } from 'constants';
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

  protected getKeysToStore(): Array<{ key: keyof TState; remove?: boolean }> {
    return this.state.persistentProperties.map((x) => ({ key: x, remove: false }));
  }

  async persistState(): Promise<void> {
    let objectToPersist: Partial<TState> = {};
    for (let { key, remove } of this.getKeysToStore()) {
      if (remove === true) {
        delete objectToPersist[key];
      } else {
        objectToPersist[key] = this.state[key];
      }
    }
    // var key = await this.getEncryptionKey();
    // const privateKey = await window.crypto.subtle.exportKey("jwk", key.privateKey)
    // const publicKey = await window.crypto.subtle.exportKey("jwk", key.publicKey);


    // console.log('Key:', privateKey, publicKey);
    localStorage.setItem(this.state.persistentStorage, JSON.stringify(objectToPersist));
    // localStorage.setItem(
    //   `${this.state.persistentStorage}_`,
    //   await this.encrypt(JSON.stringify(objectToPersist), key.publicKey),
    // );
  }

  async encrypt(s: string, key: CryptoKey) {
    const encoder = new TextEncoder();
    const data = encoder.encode(s);
    const cipherText = await window.crypto.subtle.encrypt({ name: 'RSA-OAEP' }, key, data);
    const buffer = new Uint8Array(cipherText);

    return buffer.toString();
  }

  async decrypt(data: Uint8Array, key: CryptoKey) {
    let decrypted = await window.crypto.subtle.decrypt(
      {
        name: 'RSA-OAEP',
      },
      key,
      data,
    );
    const dec = new TextDecoder();
    return dec.decode(decrypted);
  }

  async getEncryptionKey() {
    return await window.crypto.subtle.generateKey(
      {
        name: 'RSA-OAEP',
        modulusLength: 2048,
        publicExponent: new Uint8Array([1, 0, 1]),
        hash: 'SHA-256',
      },
      true,
      ['encrypt', 'decrypt'],
    );
  }
}
