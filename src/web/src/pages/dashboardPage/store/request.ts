import { BPromise, UrlLinkage } from "../../../helpers/base";
import { IPromiseResult } from "../../../helpers/model";
import Fetch from "../../../helpers/fetch";
import URL from "../../../helpers/url";

export const Request = {

    getCategoryDashboardItem: async (): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(URL.SLADBForm.GetCategorySelectedList).get().json());
    },

    getDashboardItem: async (id): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.SLADBForm.GetSelectList,id)).get().json());
    },
};
