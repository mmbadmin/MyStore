import { Reducer } from "redux";
import { LoginActionTypes } from "./actionType";
import { ILoginState, KnownAction } from "./model";

const unloadedState: ILoginState = {
    loading: false,
    failed: false,
    changePassword:{
        loading: false,
        open: false,
    },
};

export const LoginReducer: Reducer<ILoginState> = (state: ILoginState = unloadedState, action: KnownAction) => {
    switch (action.type) {
        case LoginActionTypes.LoginFetch: {
            return {
                ...state,
                loading: true,
                failed: false,
            } as ILoginState;
        }
        case LoginActionTypes.LoginFetchSuccess: {
            return {
                ...state,
                loading: false,
            } as ILoginState;
        }
        case LoginActionTypes.LoginFetchFail: {
            return {
                ...state,
                loading: false,
                failed: true,
            } as ILoginState;
        }

        case LoginActionTypes.ChangePasswordModal: {
            return {
                ...state,
                changePassword: {
                    ...state.changePassword,
                    open: action.open,
                },
            } as ILoginState;
        }
    }
    return state;
};
