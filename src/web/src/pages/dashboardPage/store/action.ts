import { AppAction } from "../../../store/state";
import { Request } from "./request";
import { KnownAction } from "./model";
import { DashboardPageActionTypes } from "./actionType";

export const DashboardPageActions = {
    getDashboardItem: (id): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: DashboardPageActionTypes.DashboardPageSetDashboardItem, data: [] });
        const res = await Request.getDashboardItem(id);
        if (res.ok) {
            dp({ type: DashboardPageActionTypes.DashboardPageSetDashboardItem, data: res?.data });
        }
    },
    setFormId: (formId,formName): AppAction<KnownAction> => async (dp, state) => {
        dp({ type: DashboardPageActionTypes.SetFormId, formId: formId, formName:formName });
    },

    getCategoryDashboardItem: (): AppAction<KnownAction> => async (dp, state) => {
        const res = await Request.getCategoryDashboardItem();
        if (res.ok) {
            dp({ type: DashboardPageActionTypes.CategoryDashboardItem, data: res.data });
        }
    },
};
