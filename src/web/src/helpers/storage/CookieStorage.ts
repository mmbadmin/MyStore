import * as cookie from "cookie";

let prefix = "FD_";

export default class CookieStorage implements Storage {
    public length: number;
    private readonly cookieOptions: any;

    constructor(options: any = {}) {
        this.length = 0;
        this.cookieOptions = Object.assign({ path: "/" }, options);
        prefix = options.prefix === undefined ? prefix : options.prefix;
    }

    public getItem(key: string): string | null {
        const cookies = cookie.parse(document.cookie);
        if (!cookies || !cookies.hasOwnProperty(prefix + key)) {
            return null;
        }
        const val = cookies[prefix + key];
        if (val) {
            return JSON.parse(val);
        }
        return null;
    }

    public setItem(key: string, value: string): void {
        this.length++;
        document.cookie = cookie.serialize(prefix + key, value, this.cookieOptions);
    }

    public removeItem(key: string): void {
        this.length--;
        const options = Object.assign({}, this.cookieOptions, { maxAge: -1 });
        document.cookie = cookie.serialize(prefix + key, "", options);
    }

    public clear(): void {
        const cookies = cookie.parse(document.cookie);
        for (const key in cookies) {
            if (key.indexOf(prefix) === 0) {
                this.removeItem(key.substr(prefix.length));
            }
        }
    }

    public key(index: number): string | null {
        const cookies = cookie.parse(document.cookie);
        return cookies[index];
    }
}

export function hasCookies(): boolean {
    const storage = new CookieStorage();
    try {
        const TEST_KEY = "__test";
        storage.setItem(TEST_KEY, "1");
        const value = storage.getItem(TEST_KEY);
        storage.removeItem(TEST_KEY);
        return value === "1";
    } catch (e) {
        return false;
    }
}
