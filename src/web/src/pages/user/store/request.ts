import { UrlLinkage, BPromise } from "../../../helpers/base";
import { IPromiseResult } from "../../../helpers/model";
import Fetch from "../../../helpers/fetch";
import URL from "../../../helpers/url";
import { PageQuery } from "../../../helpers/base";

export const Request = {
    getPagedList: async (
        p: number = 1,
        ps: number = 10,
        sc?: string,
        so?: string,
        filters: any = {},
    ): Promise<IPromiseResult> => {
        return await BPromise(
            Fetch()
                .url(UrlLinkage(URL.User.GetPagedList) + PageQuery(p, ps, sc, so, filters))
                .get()
                .json(),
        );
    },
    getItem: async (id: string): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.GetItem, id)).get().json());
    },
    sendCreate: async (item): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.Insert)).post(item).res());
    },
    sendUpdate: async (item): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.Update)).put(item).res());
    },
    sendDelete: async (id): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.Update, id)).delete().res());
    },
    sendActive: async (id): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.Active)).put({ userId: id }).res());
    },
    sendDeactive: async (id): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.Deactive)).put({ userId: id }).res());
    },
    sendUnlock: async (id): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.Unlock)).put({ userId: id }).res());
    },
    getRoles: async (): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.Role.GetSelectList)).get().json());
    },
    getUserRoles: async (id: string): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.Role, id)).get().json());
    },
    sendUserRoles: async (data): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.Role)).post(data).res());
    },
    sendUserPassword: async (item): Promise<IPromiseResult> => {
        return await BPromise(Fetch().url(UrlLinkage(URL.User.ResetPassword)).put(item).res());
    },
};
