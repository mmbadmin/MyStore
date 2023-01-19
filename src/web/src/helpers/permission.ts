import React from "react";
import { IPermissionModel } from "./model";
export const unloaded: IPermissionModel = {
    user: {
        firstName: "",
        lastName: "",
        userName: "",
        permissions: [],
    },
    token: "",
    isAuth: false,
    setToken: (_: string) => {
        return;
    },
    havePermission: (_: string | undefined) => false,
    haveAnyPermissions: (..._: string[]) => false,
};

const PermissionContext = React.createContext<IPermissionModel>(unloaded);

export default PermissionContext;
