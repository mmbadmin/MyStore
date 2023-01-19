import { ILoginState } from "../pages/login/store/model";
import { IUserState } from "../pages/user/store/model";
import { IRoleState } from "../pages/role/store/model";
import { IDashboardPageState } from "../pages/dashboardPage/store/model";

export interface IApplicationState {
    login: ILoginState;
    user: IUserState;
    role: IRoleState;
    dashboardPage: IDashboardPageState;
}

export type AppAction<TAction> = (dispatch: (action: TAction) => void, getState: () => IApplicationState) => void;
