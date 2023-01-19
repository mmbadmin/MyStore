import { Action } from "redux";
import { LoginActionTypes } from "./actionType";

export interface ILoginState {
    loading: boolean;
    failed: boolean;
    changePassword: {
        open: boolean;
        loading: boolean;
    };
}

interface ILoginFetch extends Action<string> {
    type: LoginActionTypes.LoginFetch;
}

interface ILoginFetchSuccess extends Action<string> {
    type: LoginActionTypes.LoginFetchSuccess;
}

interface ILoginFetchFail extends Action<string> {
    type: LoginActionTypes.LoginFetchFail;
}

interface IChangePasswordModal extends Action<string> {
    type: LoginActionTypes.ChangePasswordModal;
    open: boolean;
}

export type KnownAction = ILoginFetch | ILoginFetchSuccess | ILoginFetchFail|IChangePasswordModal;
