import CookieStorage from "./CookieStorage";
import IsSupported from "./isSupported";
import MemoryStorage from "./MemoryStorage";

let BStorage: Storage;
if (IsSupported("localStorage")) {
    BStorage = window.localStorage;
} else if (IsSupported("sessionStorage")) {
    BStorage = window.sessionStorage;
} else if (IsSupported("cookieStorage")) {
    BStorage = new CookieStorage();
} else {
    BStorage = new MemoryStorage();
}

export default BStorage;
