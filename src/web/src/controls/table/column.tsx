import React from "react";
import { IBaseColumnProps } from "./base";

class Column<T> extends React.Component<IBaseColumnProps<T>, React.ComponentState> {
    public static defaultProps = {
        hasFilter: false,
    };
}

export default Column;
