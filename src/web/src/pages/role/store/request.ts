import { BPromise, UrlLinkage } from "../../../helpers/base";
import { IPromiseResult } from "../../../helpers/model";
import Fetch from "../../../helpers/fetch";
import URL from "../../../helpers/url";
import { PageQuery } from "../../../helpers/base";

export const Request = {
    getPagedList: async (
        p: number,
        ps: number = 10,
        sc?: string,
        so?: string,
        filters: any = {},
    ): Promise<IPromiseResult> => {
        return await BPromise(
            Fetch()
                .url(URL.Role.GetPagedList + PageQuery(p, ps, sc, so, filters))
                .get()
                .json(),
        );
    },
    getItem: async (id: string): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.Role.Update, id)).get().json());
    },
    getPermissionList: async (): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(URL.Permission.GetList).get().json());
    },
    getRolePermissionList: async (id: string): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.Role.GetPermission, id)).get().json());
    },
    sendCreate: async (item): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(URL.Role.Insert).post(item).res());
    },
    sendUpdate: async (item): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.Role.Update)).put(item).res());
    },
    sendDelete: async (id): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.Role.Update, id)).delete().res());
    },
    sendPermission: async (values): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.Role.GetPermission)).post(values).res());
    },
};
