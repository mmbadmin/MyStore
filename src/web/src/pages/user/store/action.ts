import { AppAction } from "../../../store/state";
import { Request } from "./request";
import { UserActionTypes } from "./actionType";
import { MBox } from "../../../controls/mbox";
import { KnownAction } from "./model";

export const UserActions = {
    // list
    getPagedList: (
        p: number,
        ps: number = 10,
        sc: string = "",
        so: string = "",
        filters: any = {},
    ): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserTableLoading, loading: true });
        const res = await Request.getPagedList(p, ps, sc, so, filters);
        if (res.ok) {
            dp({ type: UserActionTypes.UserSetTableData, data: res.data, p, ps, sc, so, filters });
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
        }
    },
    getItem: (id: string): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserTableLoading, loading: true });
        const res = await Request.getItem(id);
        if (res.ok) {
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
            dp({ type: UserActionTypes.UserSetItemData, data: res.data });
            dp({ type: UserActionTypes.UserUpdateSetItemID, itemId: id });
            dp({ type: UserActionTypes.UserUpdateModal, open: true });
        } else {
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
            MBox.error(res.error);
        }
    },
    getRoles: (): AppAction<KnownAction> => async (dp, state) => {
        const res = await Request.getRoles();
        if (res.ok) {
            dp({ type: UserActionTypes.UserRoleSetRoles, data: res.data });
        }
    },
    getUserRoles: (id: string): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserTableLoading, loading: true });
        const res = await Request.getUserRoles(id);
        if (res.ok) {
            dp({ type: UserActionTypes.UserRoleSetData, data: res.data, itemId: id });
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
            dp({ type: UserActionTypes.UserRoleModal, open: true });
        } else {
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
            MBox.error(res.error);
        }
    },
    setPasswordItemId: (id: string): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserPasswordSetItemId, itemId: id });
        dp({ type: UserActionTypes.UserPasswordModal, open: true });
    },
    // create
    sendCreate: (item): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserCreateLoading, loading: true });
        const res = await Request.sendCreate(item);
        if (res.ok) {
            dp({ type: UserActionTypes.UserCreateLoading, loading: false });
            dp({ type: UserActionTypes.UserCreateModal, open: false });
            const { p, ps, sc, so, filters } = state().user.userList;
            UserActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserCreateLoading, loading: false });
        }
    },
    // update
    sendUpdate: (item): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserUpdateLoading, loading: true });
        const res = await Request.sendUpdate(item);
        if (res.ok) {
            dp({ type: UserActionTypes.UserUpdateLoading, loading: false });
            dp({ type: UserActionTypes.UserUpdateModal, open: false });
            const { p, ps, sc, so, filters } = state().user.userList;
            UserActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserUpdateLoading, loading: false });
        }
    },
    // delete
    sendDelete: (id): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserTableLoading, loading: true });
        const res = await Request.sendDelete(id);
        if (res.ok) {
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
            const { p, ps, sc, so, filters } = state().user.userList;
            UserActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
        }
    },
    sendActive: (id): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserTableLoading, loading: true });
        const res = await Request.sendActive(id);
        if (res.ok) {
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
            const { p, ps, sc, so, filters } = state().user.userList;
            UserActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
        }
    },
    sendDeactive: (id): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserTableLoading, loading: true });
        const res = await Request.sendDeactive(id);
        if (res.ok) {
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
            const { p, ps, sc, so, filters } = state().user.userList;
            UserActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
        }
    },
    sendUnlock: (id): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserTableLoading, loading: true });
        const res = await Request.sendUnlock(id);
        if (res.ok) {
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
            const { p, ps, sc, so, filters } = state().user.userList;
            UserActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserTableLoading, loading: false });
        }
    },
    // user roles
    sendUserRoles: (data): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserRoleSetLoading, loading: true });
        const res = await Request.sendUserRoles(data);
        if (res.ok) {
            dp({ type: UserActionTypes.UserRoleSetLoading, loading: false });
            dp({ type: UserActionTypes.UserRoleModal, open: false });
            const { p, ps, sc, so, filters } = state().user.userList;
            UserActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserRoleSetLoading, loading: false });
        }
    },
    // password
    sendUserPassword: (item): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: UserActionTypes.UserPasswordSetLoading, loading: true });
        const res = await Request.sendUserPassword(item);
        if (res.ok) {
            dp({ type: UserActionTypes.UserPasswordSetLoading, loading: false });
            dp({ type: UserActionTypes.UserPasswordModal, open: false });
            const { p, ps, sc, so, filters } = state().user.userList;
            UserActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: UserActionTypes.UserPasswordSetLoading, loading: false });
        }
    },
    // modals
    toggleUserCreateModal: (open: boolean): AppAction<KnownAction> => (dp, state) => {
        dp({ type: UserActionTypes.UserCreateModal, open });
    },
    toggleUserUpdateModal: (open: boolean): AppAction<KnownAction> => (dp, state) => {
        dp({ type: UserActionTypes.UserUpdateModal, open });
    },
    toggleUserRoleModal: (open: boolean): AppAction<KnownAction> => (dp, state) => {
        dp({ type: UserActionTypes.UserRoleModal, open });
    },
    toggleUserPasswordModal: (open: boolean): AppAction<KnownAction> => (dp, state) => {
        dp({ type: UserActionTypes.UserPasswordModal, open });
    },
};
