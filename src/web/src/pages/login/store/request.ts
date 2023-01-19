import { BPromise } from "../../../helpers/base";
import { IPromiseResult } from "../../../helpers/model";
import Fetch from "../../../helpers/fetch";
import URL from "../../../helpers/url";

export const Request = {
    sendLogin: async (item): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(URL.User.Login).post(item).json());
    },
    changePassword: async (item): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(URL.User.ChangePassword).put(item).json());
    },
}
