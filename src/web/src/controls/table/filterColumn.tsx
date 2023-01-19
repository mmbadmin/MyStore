import React from "react";
import { IBaseColumnProps } from "./base";

interface IFilter {
    key: any;
    value: string;
}

interface IFilterColumnProps<T> extends IBaseColumnProps<T> {
    filter: IFilter[];
}

class FilterColumn<T> extends React.Component<IFilterColumnProps<T>, React.ComponentState> {
    public static defaultProps = {
        hasFilter: false,
    };
}

export default FilterColumn;
