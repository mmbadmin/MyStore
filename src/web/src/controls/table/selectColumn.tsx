import React from "react";
import { IBaseColumnProps } from "./base";

interface ISelectColumnProps<T> extends IBaseColumnProps<T> {
    items: string | any[];
}

class SelectColumn<T> extends React.Component<ISelectColumnProps<T>, React.ComponentState> {
    public static defaultProps: Partial<ISelectColumnProps<any>> = {
        hasFilter: false,
    };
}

export default SelectColumn;
