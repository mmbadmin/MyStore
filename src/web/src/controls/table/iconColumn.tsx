import { Icon } from "antd";
import React from "react";
import { IBaseColumnProps } from "./base";

interface IIconColumnProps<T> extends IBaseColumnProps<T> {
    activeIcon?: string | React.Component;
    inActiveIcon?: string | React.Component;
    noState?: string | React.Component;
    activeValue?: boolean | string;
    inActiveValue?: boolean | string;
    activeText?: string;
    inActiveText?: string;
}

class IconColumn<T> extends React.Component<IIconColumnProps<T>, React.ComponentState> {
    public static defaultProps = {
        activeIcon: <Icon type="check" theme="outlined" />,
        hasFilter: false,
        inActiveIcon: <Icon type="close" theme="outlined" />,
        activeValue: true,
        inActiveValue: false,
        activeText: "Yes",
        inActiveText: "No",
    };
}

export default IconColumn;
