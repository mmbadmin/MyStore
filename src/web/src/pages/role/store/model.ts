import { Action } from "redux";
import { RoleActionTypes } from "./actionType";
import { IPagedList } from "../../../helpers/model";

export interface IRoleItemModel {
    title: string;
    description: string;
}

export interface IRoleState {
    roleList: {
        loading: boolean;
        data: IPagedList;
        p: number;
        ps: number;
        sc?: string;
        so?: string;
        filters: any;
    };
    roleItem: IRoleItemModel;
    roleCreate: {
        open: boolean;
        loading: boolean;
    };
    roleUpdate: {
        itemId: string;
        open: boolean;
        loading: boolean;
    };
    rolePermission: {
        permissionData: any[];
        roleData: any[];
        roleId: string;
        open: boolean;
        loading: boolean;
    };
}

// List
interface IRoleTableLoading extends Action<string> {
    type: RoleActionTypes.RoleTableLoading;
    loading: boolean;
}

interface IRoleSetTableData extends Action<string> {
    type: RoleActionTypes.RoleSetTableData;
    data: any;
    p: number;
    ps: number;
    sc?: string;
    so?: string;
    filters: any;
}

interface IRoleSetItemData extends Action<string> {
    type: RoleActionTypes.RoleSetItemData;
    data: IRoleItemModel;
}

// Create
interface IRoleCreateLoading extends Action<string> {
    type: RoleActionTypes.RoleCreateLoading;
    loading: boolean;
}

interface IRoleCreateModal extends Action<string> {
    type: RoleActionTypes.RoleCreateModal;
    open: boolean;
}

// Update

interface IRoleUpdateLoading extends Action<string> {
    type: RoleActionTypes.RoleUpdateLoading;
    loading: boolean;
}

interface IRoleUpdateModal extends Action<string> {
    type: RoleActionTypes.RoleUpdateModal;
    open: boolean;
}

interface IRoleUpdateSetItemID extends Action<string> {
    type: RoleActionTypes.RoleUpdateSetItemID;
    itemId: string;
}

// Permission

interface IRolePermissionLoading extends Action<string> {
    type: RoleActionTypes.RolePermissionLoading;
    loading: boolean;
}

interface IRolePermissionSetPermissionData extends Action<string> {
    type: RoleActionTypes.RolePermissionSetPermissionData;
    data: any[];
}

interface IRolePermissionSetRoleData extends Action<string> {
    type: RoleActionTypes.RolePermissionSetRoleData;
    data: any[];
    roleId: string;
}

interface IRolePermissionModal extends Action<string> {
    type: RoleActionTypes.RolePermissionModal;
    open: boolean;
}

export type KnownAction =
    | IRoleTableLoading
    | IRoleSetTableData
    | IRoleSetItemData
    | IRoleCreateLoading
    | IRoleCreateModal
    | IRoleUpdateLoading
    | IRoleUpdateModal
    | IRoleUpdateSetItemID
    | IRolePermissionModal
    | IRolePermissionLoading
    | IRolePermissionSetPermissionData
    | IRolePermissionSetRoleData;
