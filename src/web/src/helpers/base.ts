import { IPromiseResult, IGridFilterModel } from "./model";
import isJson from "is-json";

export const HasErrors = (fieldsError): boolean => {
    return Object.keys(fieldsError).some((field) => fieldsError[field]);
};

export const BPromise = (promise): Promise<IPromiseResult> => {
    return promise
        .then((data: any) => {
            return { data, error: undefined, ok: true } as IPromiseResult;
        })
        .catch((err) => ({ data: null, error: ParseError(err), ok: false } as IPromiseResult));
};

const ParseError = (error): string => {
    let errStr: string;
    if (error.toString) {
        errStr = error.toString();
    } else {
        errStr = `${error}`;
    }
    if (errStr === "Error") {
        return "An error has occurred";
    }
    if (errStr.startsWith("Error:")) {
        errStr = errStr.replace("Error:", "");
    }
    if (!isJson(errStr)) {
        return errStr;
    }
    const obj = JSON.parse(errStr);
    return (obj.message as string[]).join("\n");
};




export const UrlLinkage = (...parts: Array<string | number>): string => {
    if (parts.length === 0) {
        return "";
    }
    const finalUrl: string[] = [];
    for (const part of parts) {
        let tempUrl: string = typeof part === "string" ? part : part.toString();
        if (tempUrl.startsWith("/")) {
            tempUrl = tempUrl.substring(1, tempUrl.length);
        }
        if (tempUrl.endsWith("/")) {
            tempUrl = tempUrl.substring(0, tempUrl.length - 1);
        }
        if (tempUrl.trim() !== "") {
            finalUrl.push(tempUrl);
        }
    }
    return finalUrl.join("/");
};

export const PageQuery = (p: number, ps: number = 10, sc?: string, so?: string, filters: any = undefined): string => {
    if (!p) {
        p = 1;
    }
    if (!p && !ps) {
        return "";
    }
    const arrQuery: string[] = [];
    if (p) {
        arrQuery.push(`p=${p}`);
    }
    if (ps) {
        arrQuery.push(`ps=${ps}`);
    }
    if (sc) {
        arrQuery.push(`sc=${sc}`);
    }
    if (so) {
        arrQuery.push(`so=${so.replace("end", "")}`);
    }
    const filter = ParseFilter(filters);
    if (filter) {
        arrQuery.push(`filter=${JSON.stringify(filter)}`);
    }
    let query = "";
    if (arrQuery.length > 0) {
        query = `?${arrQuery.join("&")}`;
    }
    return query;
};

const ParseFilter = (filters: any): IGridFilterModel[] | undefined => {
    if (!filters) {
        return undefined;
    }
    if (filters) {
        const list: IGridFilterModel[] = [];
        for (const key in filters) {
            if (filters.hasOwnProperty(key)) {
                const element = filters[key][0];
                if (element) {
                    list.push({ field: key, value: element.value, operator: element.operator });
                }
            }
        }
        return list;
    }
    return undefined;
};

const collectionHas = (a, b) => {
    for (let i = 0, len = a.length; i < len; i++) {
        if (a[i] === b) {
            return true;
        }
    }
    return false;
};

export const FindParentBySelector = (elm, selector) => {
    const all = document.querySelectorAll(selector);
    let cur = elm.parentNode;
    while (cur && !collectionHas(all, cur)) {
        cur = cur.parentNode;
    }
    return cur;
};

export const PriceParser = (value: string | number): string => {
    if (value === 0 || value === "0") {
        return "0";
    }
    if (!value) {
        return "";
    }
    if (typeof value !== "string") {
        value = value.toString();
    }
    return value.replace(/\B(?=(\d{3})+(?!\d))/g, "");
};


export const CurrencyParser = (value: string | number): string => {
    if (value === 0 || value === "0") {
        return "0";
    }
    if (!value) {
        return "";
    }
    if (typeof value !== "string") {
        value = value.toString();
    }
    return `$ ${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
};

export const NormFile = (e) => {
    if (Array.isArray(e)) {
        return e;
    }
    return e && e.fileList;
};

export const TwoColumnLayout = {
    xs: 24,
    sm: 24,
    md: 12,
    lg: 12,
    xl: 12,
};

export const ThreeColumnLayout = {
    xs: 24,
    sm: 24,
    md: 12,
    lg: 8,
    xl: 8,
    xxl: 8,
};

export const FourColumnLayout = {
    xs: 24,
    sm: 24,
    md: 8,
    lg: 6,
    xl: 6,
    xxl: 6,
};

export const formItemLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 8 },
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 16 },
    },
};

export const formSLADBLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 18 },
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 6 },
    },
};

export const formSLADBOutStandingLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 1 },
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 23 },
    },
};

export const tailFormItemLayout = {
    wrapperCol: {
        xs: {
            span: 24,
            offset: 0,
        },
        sm: {
            span: 16,
            offset: 8,
        },
    },
};
export const tailFormAddressItemLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 4 },
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 20 },
    },
};

export const GetFileID = (fileList: any[]): string[] => {
    if (!fileList || fileList.length === 0) {
        return [];
    }
    const list: any[] = [];
    for (const file of fileList) {
        if (!file.response) {
            if (isGuid(file.uid)) {
                list.push(file.uid);
            }
        } else {
            if (isGuid(file.response)) {
                list.push(file.response);
            }
        }
    }
    return list;
};

export const isGuid = (stringToTest: string): boolean => {
    if (stringToTest[0] === "{") {
        stringToTest = stringToTest.substring(1, stringToTest.length - 1);
    }
    const regexGuid = /^(\{){0,1}[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(\}){0,1}$/gi;
    return regexGuid.test(stringToTest);
};
