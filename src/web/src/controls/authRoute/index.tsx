import React from "react";
import { Route, RouteProps } from "react-router-dom";
import Login from "../../pages/login";
import WithUserContext from "../hoc";
import { IPermissionState } from "../../helpers/model";

type IProps = RouteProps & IPermissionState;

const AuthRoute = ({ component: Component, ...rest }: IProps) => {
    const auth = rest.permissions.isAuth;
    if (!auth) {
        return <Route {...rest} component={Login} />;
    }
    return <Route {...rest} component={Component} />;
};

export default WithUserContext(AuthRoute);
