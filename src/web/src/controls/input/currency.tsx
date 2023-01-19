import * as React from "react";
import { Input } from "antd";
import { InputProps } from "antd/lib/input";
import { CurrencyParser } from "../../helpers/base";
import { onKeyPress } from "./base";

const formatter = (value) => {
    if (value === undefined || value === null || value === "") {
        return "";
    }
    return CurrencyParser(value);
};

const parser = (value) => {
    if (value === undefined || value === null || value === "") {
        return "";
    }
    if (!(typeof value === "string")) {
        value = value.toString();
    }
    return value.replace(/\$\s?|(,*)/g, "");
};

export interface IProps extends Omit<InputProps, "onChange"> {
    onChange?: (value: string) => void;
}

interface IStates {
    value?: string | string[] | number;
}

class CurrencyInput extends React.Component<IProps, IStates> {
    constructor(props) {
        super(props);
        this.state = {
            value: this.props.value,
        };
    }

    public render() {
        const { onChange, value, ...rest } = this.props;
        const stateValue = this.state.value;
        return (
            <Input
                {...rest}
                onChange={this.onChange}
                onKeyPress={onKeyPress}
                value={formatter(parser(stateValue))}
                maxLength={19}
                dir="auto"
            />
        );
    }

    private onChange = (event) => {
        let { value } = event.target;
        value = value.replace(/[^0-9.-]/g, "");
        this.setState({ value });
        if (this.props.onChange) {
            if (value) {
                this.props.onChange(parser(value));
            } else {
                this.props.onChange("");
            }
        }
    };
}

export default CurrencyInput;
