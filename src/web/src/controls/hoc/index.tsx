import React from "react";
import PermissionContext from "../../helpers/permission";

export default function WithUserContext(Component: any) {
    return function PermissionComponent(props: any) {
        return (
            <PermissionContext.Consumer>
                {(permissions) => <Component {...props} permissions={permissions} />}
            </PermissionContext.Consumer>
        );
    };
}
