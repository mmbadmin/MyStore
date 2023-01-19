import React from "react";
import { IBaseColumnProps } from "./base";

interface IDateColumnProps<T> extends IBaseColumnProps<T> {
    format?: string;
}

class DateColumn<T> extends React.Component<IDateColumnProps<T>, React.ComponentState> {
    public static defaultProps = {
        format: "YYYY/MM/DD",
        hasFilter: false,
    };
}

export default DateColumn;
