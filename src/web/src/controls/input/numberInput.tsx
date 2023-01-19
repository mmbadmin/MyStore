import React from "react";
import { Input } from "antd";
import { InputProps } from "antd/lib/input";
import { onKeyPress } from "./base";

export interface IProps extends Omit<InputProps, "onChange"> {
    onChange?: (value: string) => void;
}

interface IStates {
    value?: string | string[] | number;
}

class NumberInput extends React.Component<IProps, IStates> {
    constructor(props) {
        super(props);
        this.state = {
            value: this.props.value,
        };
    }

    public render() {
        const { onChange, value, ...rest } = this.props;
        return <Input {...rest} onChange={this.onChange} onKeyPress={onKeyPress} value={value} />;
    }

    private onChange = (event) => {
        let { value } = event.target;
        value = value.replace(/[^0-9.-]/g, "");
        this.setState({ value });
        if (this.props.onChange) {
            if (value) {
                this.props.onChange(value);
            } else {
                this.props.onChange("");
            }
        }
    };
}

export default NumberInput;
