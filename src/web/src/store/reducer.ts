import { LoginReducer } from "../pages/login/store/reducer";
import { UserReducer } from "../pages/user/store/reducer";
import { RoleReducer } from "../pages/role/store/reducer";
import { DashboardPageReducer } from "../pages/dashboardPage/store/reducer";
export const reducers = {
    login: LoginReducer,
    user: UserReducer,
    role: RoleReducer,
    dashboardPage: DashboardPageReducer,
};
