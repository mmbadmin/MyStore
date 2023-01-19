export default class MemoryStorage implements Storage {
    public length: number;
    private data: any;

    constructor() {
        this.data = {};
        this.length = 0;
    }

    public getItem(key: string): string {
        return this.data.hasOwnProperty(key) ? this.data[key] : null;
    }

    public setItem(key: string, value: string): void {
        this.data[key] = value;
    }

    public removeItem(key: string): void {
        delete this.data[key];
    }

    public clear(): void {
        this.data = {};
    }

    public key(index: number): string | null {
        return this.data[index];
    }
}
