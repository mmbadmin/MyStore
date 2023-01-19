import { AppAction } from "../../../store/state";
import { Request } from "./request";
import { RoleActionTypes } from "./actionType";
import { MBox } from "../../../controls/mbox";
import { KnownAction } from "./model";

export const RoleActions = {
    // list
    getPagedList: (
        p: number,
        ps: number = 10,
        sc: string = "",
        so: string = "",
        filters: any = {},
    ): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: RoleActionTypes.RoleTableLoading, loading: true });
        const res = await Request.getPagedList(p, ps, sc, so, filters);
        if (res.ok) {
            dp({ type: RoleActionTypes.RoleSetTableData, data: res.data, p, ps, sc, so, filters });
            dp({ type: RoleActionTypes.RoleTableLoading, loading: false });
        } else {
            MBox.error(res.error);
            dp({ type: RoleActionTypes.RoleTableLoading, loading: false });
        }
    },
    getItem: (id: string): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: RoleActionTypes.RoleTableLoading, loading: true });
        const res = await Request.getItem(id);
        if (res.ok) {
            dp({ type: RoleActionTypes.RoleTableLoading, loading: false });
            dp({ type: RoleActionTypes.RoleSetItemData, data: res.data });
            dp({ type: RoleActionTypes.RoleUpdateSetItemID, itemId: id });
            dp({ type: RoleActionTypes.RoleUpdateModal, open: true });
        } else {
            dp({ type: RoleActionTypes.RoleTableLoading, loading: false });
            MBox.error(res.error);
        }
    },
    getPermissionList: (): AppAction<KnownAction> => async (dp, state) => {
        const res = await Request.getPermissionList();
        if (res.ok) {
            dp({ type: RoleActionTypes.RolePermissionSetPermissionData, data: res.data });
        }
    },
    getRolePermissionList: (id: string): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: RoleActionTypes.RoleTableLoading, loading: true });
        const res = await Request.getRolePermissionList(id);
        if (res.ok) {
            dp({ type: RoleActionTypes.RoleTableLoading, loading: false });
            dp({ type: RoleActionTypes.RolePermissionSetRoleData, data: res.data, roleId: id });
            dp({ type: RoleActionTypes.RolePermissionModal, open: true });
        } else {
            dp({ type: RoleActionTypes.RoleTableLoading, loading: false });
            MBox.error(res.error);
        }
    },
    // create
    sendCreate: (item): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: RoleActionTypes.RoleCreateLoading, loading: true });
        const res = await Request.sendCreate(item);
        if (res.ok) {
            dp({ type: RoleActionTypes.RoleCreateLoading, loading: false });
            dp({ type: RoleActionTypes.RoleCreateModal, open: false });
            const { p, ps, sc, so, filters } = state().role.roleList;
            RoleActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: RoleActionTypes.RoleCreateLoading, loading: false });
        }
    },
    // update
    sendUpdate: (item): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: RoleActionTypes.RoleUpdateLoading, loading: true });
        const res = await Request.sendUpdate(item);
        if (res.ok) {
            dp({ type: RoleActionTypes.RoleUpdateLoading, loading: false });
            dp({ type: RoleActionTypes.RoleUpdateModal, open: false });
            const { p, ps, sc, so, filters } = state().role.roleList;
            RoleActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: RoleActionTypes.RoleUpdateLoading, loading: false });
        }
    },
    // update
    sendDelete: (id): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: RoleActionTypes.RoleTableLoading, loading: true });
        const res = await Request.sendDelete(id);
        if (res.ok) {
            const { p, ps, sc, so, filters } = state().role.roleList;
            RoleActions.getPagedList(p, ps, sc, so, filters)(dp, state);
        } else {
            MBox.error(res.error);
            dp({ type: RoleActionTypes.RoleTableLoading, loading: false });
        }
    },
    // permission
    sendPermission: (values): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: RoleActionTypes.RolePermissionLoading, loading: true });
        const res = await Request.sendPermission(values);
        if (res.ok) {
            dp({ type: RoleActionTypes.RolePermissionLoading, loading: false });
            dp({ type: RoleActionTypes.RolePermissionModal, open: false });
        } else {
            dp({ type: RoleActionTypes.RolePermissionLoading, loading: false });
            MBox.error(res.error);
        }
    },
    // modals
    toggleRoleCreateModal: (open: boolean): AppAction<KnownAction> => (dp, state) => {
        dp({ type: RoleActionTypes.RoleCreateModal, open });
    },
    toggleRoleUpdateModal: (open: boolean): AppAction<KnownAction> => (dp, state) => {
        dp({ type: RoleActionTypes.RoleUpdateModal, open });
    },
    toggleRolePermissionModal: (open: boolean): AppAction<KnownAction> => (dp, state) => {
        dp({ type: RoleActionTypes.RolePermissionModal, open });
    },
};
