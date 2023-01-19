import { Reducer } from "redux";
import { DashboardPageActionTypes } from "./actionType";
import { IDashboardPageState, KnownAction } from "./model";

const unloadedState: IDashboardPageState = {
    dashboardItem: undefined,
    categoryDashboardItem:undefined,
    formInfo: {
        formId: 0,
        formName: "",
    },
};

export const DashboardPageReducer: Reducer<IDashboardPageState, KnownAction> = (
    state: IDashboardPageState = unloadedState,
    action: KnownAction,
): IDashboardPageState => {
    switch (action.type) {
        case DashboardPageActionTypes.DashboardPageSetDashboardItem: {
            return {
                ...state,
                dashboardItem: action.data,
            };
        }
        case DashboardPageActionTypes.CategoryDashboardItem: {
            return {
                ...state,
                categoryDashboardItem: action.data,
            };
        }
        case DashboardPageActionTypes.SetFormId: {
            return {
                ...state,
                formInfo: {
                    ...state.formInfo,
                    formId: action.formId,
                    formName: action.formName,
                },
            } as IDashboardPageState;
        }
    }
    return state;
};
