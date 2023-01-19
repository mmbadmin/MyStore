import React from "react";
import { Select as AntSelect, Spin } from "antd";
import {
    SelectProps as AntSelectProps,
    OptionProps as AntOptionProps,
    OptGroupProps as AntOptGroupProps,
} from "antd/lib/select";
import { BPromise, FindParentBySelector } from "../../helpers/base";
import Fetch from "../../helpers/fetch";

interface IProps extends AntSelectProps {
    url?: string;
    valueProp: string;
    displayProp: string | ((value: any) => string);
    onChange?: (value, item) => void;
    filter?: (value: any) => boolean;
    callOnChangeOnMount?: boolean;
}

interface IStates {
    data: any[];
    loading: boolean;
    reload: false;
    open: boolean;
}

class Select extends React.Component<IProps, IStates> {
    public static Option: React.ClassicComponentClass<AntOptionProps> = AntSelect.Option;
    public static OptGroup: React.ClassicComponentClass<AntOptGroupProps> = AntSelect.OptGroup;

    public static defaultProps: Partial<IProps> = {
        placeholder: "انتخاب نمایید",
        valueProp: "id",
        displayProp: "title",
        showArrow: true,
    };

    constructor(props) {
        super(props);
        this.state = {
            data: [],
            loading: false,
            reload: false,
            open: false,
        };
    }

    public componentDidMount() {
        if (!this.props.children && this.props.url) {
            this.loadData(this.props.url, this.props.callOnChangeOnMount);
        }
    }

    public componentDidUpdate(prevProps) {
        if (prevProps.url !== this.props.url) {
            if (this.props.value && this.props.onChange) {
                this.props.onChange("", undefined as any);
            }
            this.loadData(this.props.url);
        }
    }

    public render() {
        const { url, value, ...props } = this.props;
        const { loading, open } = this.state;

        const selectProps = {
            allowClear: true,
            ...props,
            getPopupContainer: this.getParents,
            showSearch: true,
            showArrow: true,
            optionFilterProp: "children",
            value: this.getValue(value),
        };
        if (this.props.mode === "multiple") {
            selectProps.onFocus = this.onFocus;
            selectProps.onBlur = this.onBlur;
            selectProps.open = open;
        }
        if (!this.props.children) {
            selectProps.onChange = this.onChange;
            selectProps.notFoundContent = loading ? <Spin size="small" /> : "گزینه ای وجود ندارد";
        }

        if (this.props.children) {
            return (
                <AntSelect {...selectProps} value={this.getValue(value)}>
                    {this.getOptionsFromChildren(this.props.children)}
                </AntSelect>
            );
        } else {
            return (
                <AntSelect {...selectProps} value={this.getValue(value)}>
                    {this.getOptionsFromData()}
                </AntSelect>
            );
        }
    }

    private onFocus = () => {
        this.setState({ open: true });
    };

    private onBlur = () => {
        this.setState({ open: false });
    };

    private onChange = (value) => {
        if (this.props.onChange) {
            const { data } = this.state;
            const item = data.filter((x) => x[this.props.valueProp].toString() === value);
            this.props.onChange(value, item[0]);
        }
    };

    private getOptionsFromChildren = (child) => {
        if (Array.isArray(child)) {
            const c: any[] = [];
            child.forEach((x) => {
                if (!x) {
                    return;
                }
                const { props, key } = x;
                c.push(
                    <AntSelect.Option key={key} value={props.value.toString()}>
                        {props.children}
                    </AntSelect.Option>,
                );
            });
            return c;
        } else {
            const { props, key } = child;
            return (
                <AntSelect.Option key={key} value={props.value.toString()}>
                    {props.children}
                </AntSelect.Option>
            );
        }
    };

    private getOptionsFromData = () => {
        const { data } = this.state;
        if (this.props.filter) {
            return data.filter(this.props.filter).map((item, index) => {
                const id = this.getProp(item, this.props.valueProp);
                const title = this.checkDisplayProp(item, this.props.displayProp);
                const key = (id || index).toString();
                return (
                    <AntSelect.Option key={key} value={id.toString()}>
                        {title}
                    </AntSelect.Option>
                );
            });
        } else {
            return data.map((item, index) => {
                const id = this.getProp(item, this.props.valueProp);
                const title = this.checkDisplayProp(item, this.props.displayProp);
                const key = (id || index).toString();
                return (
                    <AntSelect.Option key={key} value={id.toString()}>
                        {title}
                    </AntSelect.Option>
                );
            });
        }
    };

    private getParents = (target) => {
        const parent = FindParentBySelector(target, ".ant-modal-body");
        if (parent) {
            return parent;
        }
        return document.body;
    };

    private checkDisplayProp = (record, displayProp) => {
        if (typeof displayProp === "string") {
            return this.getProp(record, displayProp);
        } else {
            return displayProp(record);
        }
    };
    private getProp = (o, s) => {
        s = s.replace(/\[(\w+)\]/g, ".$1"); // convert indexes to properties
        s = s.replace(/^\./, ""); // strip a leading dot
        var a = s.split(".");
        for (var i = 0, n = a.length; i < n; ++i) {
            var k = a[i];
            if (k in o) {
                o = o[k];
            } else {
                return;
            }
        }
        return o;
    };

    private loadData = async (url, callOnChange = false) => {
        this.setState({ loading: true, reload: false, data: [] });
        const res = await BPromise(Fetch().url(url).get().json());
        if (res.ok) {
            this.setState({ data: res.data, loading: false });
            if (callOnChange) {
                if (this.props.value && this.props.onChange) {
                    this.props.onChange(
                        this.props.value,
                        res.data.filter((x) => x[this.props.valueProp] === this.props.value)[0],
                    );
                }
            }
        } else {
            this.setState({ data: [], loading: false });
        }
    };

    private getValue = (value: any): string | string[] | undefined => {
        if (this.props.mode === "multiple" || this.props.mode === "tags") {
            if (!value) {
                return [];
            }
            if (!Array.isArray(value)) {
                return [`${value}`];
            } else {
                return value.map((x) => `${x}`);
            }
        } else {
            if (!value) {
                return undefined;
            }
            if (typeof value === "object") {
                return value;
            } else if (!(typeof value === "string")) {
                return value.toString();
            }
        }
        return value;
    };
}

export default Select;
