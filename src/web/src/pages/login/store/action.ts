import { AppAction } from "../../../store/state";
import { Request } from "./request";
import { KnownAction } from "./model";
import { LoginActionTypes } from "./actionType";
import BStorage from "../../../helpers/storage";
import { Const } from "../../../helpers/const";
import { MBox } from "../../../controls/mbox";

export const LoginActions = {
    loginSend: (item, setToken: (_: string) => void): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: LoginActionTypes.LoginFetch });
        const res = await Request.sendLogin(item);
        if (res.ok) {
            dp({ type: LoginActionTypes.LoginFetchSuccess });
            BStorage.setItem(Const.AuthName, res.data.token);
            setToken(res.data.token);
        } else {
            MBox.error(res.error);
            dp({ type: LoginActionTypes.LoginFetchFail });
        }
    },
    changePassword: (item, callback: () => void): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: LoginActionTypes.LoginFetch });
        const res = await Request.changePassword(item);
        if (res.ok) {
            dp({ type: LoginActionTypes.LoginFetchSuccess });
            callback();
        } else {
            MBox.error(res.error);
            dp({ type: LoginActionTypes.LoginFetchFail });
        }
    },
    toggleChanhePassWordModal: (open: boolean): AppAction<KnownAction> => (dp, state) => {
        dp({ type: LoginActionTypes.ChangePasswordModal, open });
    },
};
