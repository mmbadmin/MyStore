import React from "react";
import { ReactComponent as Loader } from "../styles/images/loader.svg";
import PermissionContext from "../helpers/permission";
import BStorage from "../helpers/storage";
import { Const } from "../helpers/const";
import { IPermissionModel } from "../helpers/model";
import { permission } from "../helpers/access";
import { BPromise } from "../helpers/base";
import Fetch from "../helpers/fetch";
import URL from "../helpers/url";

interface IProps {
    children?: any;
}

interface IStates {
    context: IPermissionModel;
    loading: boolean;
}

class EmptyLayout extends React.Component<IProps, IStates> {
    constructor(props: IProps) {
        super(props);
        const temp = BStorage.getItem(Const.AuthName);
        let token = "";
        if (temp !== null) {
            token = temp;
        }
        this.state = {
            context: {
                user: {
                    firstName: "",
                    lastName: "",
                    userName: "",
                    permissions: [],
                },
                token: token,
                isAuth: token !== "",
                setToken: this.setInfo,
                havePermission: this.havePermission,
                haveAnyPermissions: this.haveAnyPermissions,
            },
            loading: true,
        };
    }

    public componentDidMount() {
        if (this.state.context.isAuth) {
            this.getUser();
        } else {
            this.setState({ loading: false });
        }
    }

    public render() {
        const { loading } = this.state;
        return (
            <PermissionContext.Provider value={this.state.context}>
                {loading ? (
                    <div className="loader-container">
                        <Loader />
                    </div>
                ) : (
                    <div>{this.props.children}</div>
                )}
            </PermissionContext.Provider>
        );
    }

    private setInfo = (token: string) => {
        this.setState(
            (state: IStates) => {
                return {
                    ...state,
                    context: {
                        ...state.context,
                        token: token,
                        isAuth: token !== "",
                    },
                };
            },
            () => {
                if (token !== "") {
                    this.setState({ loading: true });
                    this.getUser();
                }
            },
        );
    };

    private havePermission = (policy: string | undefined): boolean => {
        if (!policy) {
            return false;
        }
        const { permissions } = this.state.context.user;
        if (permissions.length > 0) {
            for (const item of permissions) {
                if (item === policy) {
                    return true;
                }
            }
        }
        return false;
    };

    private haveAnyPermissions = (...policies: string[]): boolean => {
        const { permissions } = this.state.context.user;
        if (permissions && permissions.length > 0 && policies.length > 0) {
            return permissions.some((x) => policies.indexOf(x) >= 0);
        }
        return false;
    };

    private getUser = async () => {
        const resUser = await BPromise(Fetch().url(URL.User.GetData).get().json());
        if (resUser.ok) {
            this.setState((state: IStates) => {
                return {
                    ...state,
                    context: {
                        ...state.context,
                        user: resUser.data,
                    },
                };
            });
        }
        const resPermission = await BPromise(Fetch().url(URL.Permission.All).get().json());
        if (resPermission.ok) {
            permission.Access = resPermission.data;
        }
        this.setState((state: IStates) => {
            return {
                ...state,
                loading: false,
            };
        });
    };
}

export default EmptyLayout;
