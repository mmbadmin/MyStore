import { Action } from "redux";
import { DashboardPageActionTypes } from "./actionType";

export interface IDashboardItemModel {
    title: string
    id: number
}

export interface ICategoryDashboardItemModel {
    title: string
    id: number
    details:IDashboardItemModel[]
}


export interface IDashboardPageState {
    dashboardItem?: IDashboardItemModel[];
    categoryDashboardItem?: ICategoryDashboardItemModel[];
    formInfo: {
        formId: number;
        formName: string;
    };
}

interface IDashboardPageSetDashboardItem extends Action<string> {
    type: DashboardPageActionTypes.DashboardPageSetDashboardItem;
    data: IDashboardItemModel[];
}


interface ICategoryDashboardPageSetDashboardItem extends Action<string> {
    type: DashboardPageActionTypes.CategoryDashboardItem;
    data: ICategoryDashboardItemModel[];
}



interface IFormID extends Action<string> {
    type: DashboardPageActionTypes.SetFormId;
    formId: number;
    formName: string;
}


export type KnownAction =
    | IDashboardPageSetDashboardItem
    | ICategoryDashboardPageSetDashboardItem
    | IFormID;
