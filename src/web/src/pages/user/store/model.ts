import { Action } from "redux";
import { UserActionTypes } from "./actionType";
import { IPagedList } from "../../../helpers/model";

export interface IUserItemModel {
    firstName: string;
    lastName: string;
    isActive: boolean;
}

export interface IUserState {
    userList: {
        loading: boolean;
        data: IPagedList;
        p: number;
        ps: number;
        sc?: string;
        so?: string;
        filters: any;
    };
    userItem: IUserItemModel;
    roles: any[];
    userCreate: {
        open: boolean;
        loading: boolean;
    };
    userUpdate: {
        itemId: string;
        open: boolean;
        loading: boolean;
    };
    userRole: {
        open: boolean;
        loading: false;
        data: any[];
        itemId: string;
    };
    userPassword: {
        open: boolean;
        loading: boolean;
        itemId: string;
    };
}
// List
interface IUserTableLoading extends Action<string> {
    type: UserActionTypes.UserTableLoading;
    loading: boolean;
}

interface IUserSetTableData extends Action<string> {
    type: UserActionTypes.UserSetTableData;
    data: any;
    p: number;
    ps: number;
    sc?: string;
    so?: string;
    filters: any;
}

interface IUserSetItemData extends Action<string> {
    type: UserActionTypes.UserSetItemData;
    data: IUserItemModel;
}

// Create
interface IUserCreateLoading extends Action<string> {
    type: UserActionTypes.UserCreateLoading;
    loading: boolean;
}

interface IUserCreateModal extends Action<string> {
    type: UserActionTypes.UserCreateModal;
    open: boolean;
}

// Update

interface IUserUpdateLoading extends Action<string> {
    type: UserActionTypes.UserUpdateLoading;
    loading: boolean;
}

interface IUserUpdateModal extends Action<string> {
    type: UserActionTypes.UserUpdateModal;
    open: boolean;
}

interface IUserUpdateSetItemID extends Action<string> {
    type: UserActionTypes.UserUpdateSetItemID;
    itemId: string;
}

// Role

interface IUserSetRoles extends Action<string> {
    type: UserActionTypes.UserRoleSetRoles;
    data: any[];
}

interface IUserRoleModal extends Action<string> {
    type: UserActionTypes.UserRoleModal;
    open: boolean;
}

interface IUserRoleSetData extends Action<string> {
    type: UserActionTypes.UserRoleSetData;
    data: any[];
    itemId: string;
}
interface IUserRoleSetLoading extends Action<string> {
    type: UserActionTypes.UserRoleSetLoading;
    loading: boolean;
}

// Password

interface IUserPasswordModal extends Action<string> {
    type: UserActionTypes.UserPasswordModal;
    open: boolean;
}

interface IUserPasswordSetItemId extends Action<string> {
    type: UserActionTypes.UserPasswordSetItemId;
    itemId: string;
}
interface IUserPasswordSetLoading extends Action<string> {
    type: UserActionTypes.UserPasswordSetLoading;
    loading: boolean;
}

export type KnownAction =
    | IUserTableLoading
    | IUserSetTableData
    | IUserSetItemData
    | IUserCreateLoading
    | IUserCreateModal
    | IUserUpdateLoading
    | IUserUpdateModal
    | IUserUpdateSetItemID
    | IUserSetRoles
    | IUserRoleModal
    | IUserRoleSetData
    | IUserRoleSetLoading
    | IUserPasswordModal
    | IUserPasswordSetItemId
    | IUserPasswordSetLoading;
