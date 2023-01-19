import React from "react";
import PermissionContext from "../../helpers/permission";

interface IProps {
    policy?: string;
    children?: any;
}

const Auth = ({ policy, children }: IProps) => {
    if (!policy) {
        return null;
    }
    return (
        <PermissionContext.Consumer>
            {({ havePermission }) => (havePermission(policy) ? children : null)}
        </PermissionContext.Consumer>
    );
};

export default Auth;
