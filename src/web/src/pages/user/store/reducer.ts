import { Reducer } from "redux";
import { UserActionTypes } from "./actionType";
import { IUserState, KnownAction } from "./model";

const unloadedState: IUserState = {
    userList: {
        loading: false,
        data: { data: [], total: 0 },
        p: 0,
        ps: 10,
        sc: "",
        so: "",
        filters: {},
    },
    userItem: {
        firstName: "",
        lastName: "",
        isActive: false,
    },
    roles: [],
    userCreate: {
        loading: false,
        open: false,
    },
    userUpdate: {
        itemId: "",
        loading: false,
        open: false,
    },
    userRole: {
        open: false,
        loading: false,
        data: [],
        itemId: "",
    },
    userPassword: {
        itemId: "",
        loading: false,
        open: false,
    },
};

export const UserReducer: Reducer<IUserState, KnownAction> = (
    state: IUserState = unloadedState,
    action: KnownAction,
) => {
    switch (action.type) {
        case UserActionTypes.UserTableLoading: {
            return {
                ...state,
                userList: {
                    ...state.userList,
                    loading: action.loading,
                },
            } as IUserState;
        }
        case UserActionTypes.UserSetTableData: {
            return {
                ...state,
                userList: {
                    ...state.userList,
                    loading: false,
                    data: action.data,
                    p: action.p,
                    ps: action.ps,
                    sc: action.sc,
                    so: action.so,
                    filters: action.filters,
                },
            } as IUserState;
        }
        case UserActionTypes.UserSetItemData: {
            return {
                ...state,
                userItem: action.data,
            } as IUserState;
        }
        case UserActionTypes.UserCreateLoading: {
            return {
                ...state,
                userCreate: {
                    ...state.userCreate,
                    loading: action.loading,
                },
            } as IUserState;
        }
        case UserActionTypes.UserUpdateLoading: {
            return {
                ...state,
                userUpdate: {
                    ...state.userUpdate,
                    loading: action.loading,
                },
            } as IUserState;
        }
        case UserActionTypes.UserCreateModal: {
            return {
                ...state,
                userCreate: {
                    ...state.userCreate,
                    open: action.open,
                },
            } as IUserState;
        }
        case UserActionTypes.UserUpdateModal: {
            return {
                ...state,
                userUpdate: {
                    ...state.userUpdate,
                    open: action.open,
                },
            } as IUserState;
        }
        case UserActionTypes.UserUpdateSetItemID: {
            return {
                ...state,
                userUpdate: {
                    ...state.userUpdate,
                    itemId: action.itemId,
                },
            } as IUserState;
        }
        case UserActionTypes.UserRoleSetRoles: {
            return {
                ...state,
                roles: action.data,
            } as IUserState;
        }
        case UserActionTypes.UserRoleModal: {
            return {
                ...state,
                userRole: {
                    ...state.userRole,
                    open: action.open,
                },
            } as IUserState;
        }
        case UserActionTypes.UserRoleSetData: {
            return {
                ...state,
                userRole: {
                    ...state.userRole,
                    data: action.data,
                    itemId: action.itemId,
                },
            } as IUserState;
        }
        case UserActionTypes.UserRoleSetLoading: {
            return {
                ...state,
                userRole: {
                    ...state.userRole,
                    loading: action.loading,
                },
            } as IUserState;
        }
        case UserActionTypes.UserPasswordModal: {
            return {
                ...state,
                userPassword: {
                    ...state.userPassword,
                    open: action.open,
                },
            } as IUserState;
        }
        case UserActionTypes.UserPasswordSetItemId: {
            return {
                ...state,
                userPassword: {
                    ...state.userPassword,
                    itemId: action.itemId,
                },
            } as IUserState;
        }
        case UserActionTypes.UserPasswordSetLoading: {
            return {
                ...state,
                userPassword: {
                    ...state.userPassword,
                    loading: action.loading,
                },
            } as IUserState;
        }
    }
    return state;
};
