import React from "react";
import { Switch, Route } from "react-router-dom";
import { IPermissionState } from "../../helpers/model";
import { permission } from "../../helpers/access";
import WithUserContext from "../../controls/hoc";
import User from "../user";
import Role from "../role";
import DashboardPage from "../dashboardPage";
import ChangePasswordPage from "../changePassword";


const DashboardRouter = (props: IPermissionState) => {
    const { havePermission } = props.permissions;
    const getRoute = (access: string | undefined, path: string, component: any) => {
        if (access === "AllAccess" || havePermission(access)) {
            return <Route path={path} component={component} />;
        }
        return null;
    };

    return (
        <React.Fragment>
            <Switch>
                {getRoute(permission.Access.user_UserGetPagedListQueryHandler, "/User", User)}
                {getRoute(permission.Access.role_RoleGetPagedListQueryHandler, "/Role", Role)}
                {getRoute("AllAccess", "/ChangePassword", ChangePasswordPage)} 
                {getRoute("AllAccess", "/", DashboardPage)}
            </Switch>
        </React.Fragment>
    );
};

export default WithUserContext(DashboardRouter);
