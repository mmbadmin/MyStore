import { fetch as WhatwgFetch } from "whatwg-fetch";
import wretch from "wretch";
import { Const } from "./const";
import BStorage from "./storage";
import { MBox } from "../controls/mbox";

const Fetch = () => {
    let bFetch = wretch().polyfills({
        fetch: WhatwgFetch,
    });

    const auth = BStorage.getItem(Const.AuthName);
    if (auth && auth !== "") {
        bFetch = bFetch.auth(`Bearer ${auth}`);
    }

    bFetch = bFetch.catcher(401, (error, request) => {
        BStorage.removeItem(Const.AuthName);
        window.location.href = Const.PublicURI;
    });

    bFetch = bFetch.catcher(403, (error, request) => {
        MBox.error("You do not have access to this part of the system");
        throw new Error();
    });

    return bFetch;
};

export default Fetch;
