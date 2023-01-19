import { Reducer } from "redux";
import { RoleActionTypes } from "./actionType";
import { IRoleState, KnownAction } from "./model";

const unloadedState: IRoleState = {
    roleList: {
        loading: false,
        data: { data: [], total: 0 },
        p: 0,
        ps: 10,
        sc: "",
        so: "",
        filters: {},
    },
    roleItem: {
        title: "",
        description: "",
    },
    roleCreate: {
        loading: false,
        open: false,
    },
    roleUpdate: {
        itemId: "",
        loading: false,
        open: false,
    },
    rolePermission: {
        permissionData: [],
        roleData: [],
        roleId: "",
        open: false,
        loading: false,
    },
};

export const RoleReducer: Reducer<IRoleState, KnownAction> = (
    state: IRoleState = unloadedState,
    action: KnownAction,
) => {
    switch (action.type) {
        case RoleActionTypes.RoleTableLoading: {
            return {
                ...state,
                roleList: {
                    ...state.roleList,
                    loading: action.loading,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RoleSetTableData: {
            return {
                ...state,
                roleList: {
                    ...state.roleList,
                    loading: false,
                    data: action.data,
                    p: action.p,
                    sc: action.sc,
                    so: action.so,
                    filters: action.filters,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RoleSetItemData: {
            return {
                ...state,
                roleItem: action.data,
            } as IRoleState;
        }
        case RoleActionTypes.RoleCreateLoading: {
            return {
                ...state,
                roleCreate: {
                    ...state.roleCreate,
                    loading: action.loading,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RoleUpdateLoading: {
            return {
                ...state,
                roleUpdate: {
                    ...state.roleUpdate,
                    loading: action.loading,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RoleCreateModal: {
            return {
                ...state,
                roleCreate: {
                    ...state.roleCreate,
                    open: action.open,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RoleUpdateModal: {
            return {
                ...state,
                roleUpdate: {
                    ...state.roleUpdate,
                    open: action.open,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RoleUpdateSetItemID: {
            return {
                ...state,
                roleUpdate: {
                    ...state.roleUpdate,
                    itemId: action.itemId,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RolePermissionModal: {
            return {
                ...state,
                rolePermission: {
                    ...state.rolePermission,
                    open: action.open,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RolePermissionLoading: {
            return {
                ...state,
                rolePermission: {
                    ...state.rolePermission,
                    loading: action.loading,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RolePermissionSetPermissionData: {
            return {
                ...state,
                rolePermission: {
                    ...state.rolePermission,
                    permissionData: action.data,
                },
            } as IRoleState;
        }
        case RoleActionTypes.RolePermissionSetRoleData: {
            return {
                ...state,
                rolePermission: {
                    ...state.rolePermission,
                    roleData: action.data,
                    roleId: action.roleId,
                },
            } as IRoleState;
        }
    }
    return state;
};
