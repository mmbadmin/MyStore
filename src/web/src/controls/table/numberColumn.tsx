import React from "react";
import { IBaseColumnProps } from "./base";

interface INumberColumnProps<T> extends IBaseColumnProps<T> {}

class NumberColumn<T> extends React.Component<INumberColumnProps<T>, React.ComponentState> {
    public static defaultProps = {
        hasFilter: false,
    };
}

export default NumberColumn;
