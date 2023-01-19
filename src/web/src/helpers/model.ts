export interface IUserModel {
    firstName: string;
    lastName: string;
    userName: string;
    permissions: string[];
}

export interface IPermissionModel {
    user: IUserModel;
    isAuth: boolean;
    token: string;
    setToken: (_: string) => void;
    havePermission: (_: string | undefined) => boolean;
    haveAnyPermissions: (...policies: string[]) => boolean;
}

export interface IPermissionState {
    permissions: IPermissionModel;
}

export interface IPromiseResult {
    ok: boolean;
    error?: string;
    data: any;
}

export interface IPagedList {
    data: any[];
    total: number;
}

export interface IGridFilterModel {
    field: string;
    operator: string;
    value: string;
}
