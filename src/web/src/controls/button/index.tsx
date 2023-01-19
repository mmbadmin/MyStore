import React from "react";
import { Button } from "antd";
import { ButtonProps } from "antd/lib/button";
import { MBox } from "../mbox";

interface ICreateButtonProps {
    createTitle?: string;
}

interface ICancelButtonProps {
    cancelitle?: string;
}


export const CreateButton = (props: ICreateButtonProps & ButtonProps) => {
    const { createTitle, ...rest } = props;
    return (
        <Button type="primary" {...rest}>
            {createTitle || "Add"}
        </Button>
    );
};

export const CancelButton = (props: ICancelButtonProps & ButtonProps) => {
    const { cancelitle, ...rest } = props;
    return (
        <Button type="default" {...rest}>
            {cancelitle || "Cancel"}
        </Button>
    );
};

interface IConfirmButtonProps {
    confirmText: string;
    confirmTitle: string;
    iconType?: string;
    onConfirm: () => void;
}

export class ConfirmButton extends React.Component<IConfirmButtonProps & ButtonProps> {
    public static defaultProps: Partial<ButtonProps> = {
        type: "danger",
    };

    public render() {
        const { confirmText, confirmTitle, icon, onConfirm, ...rest } = this.props;
        return <Button {...rest} onClick={this.onClick} />;
    }

    private onClick = (event) => {
        event.preventDefault();
        MBox.confirm(
            this.props.confirmText,
            this.props.confirmTitle,
            () => {
                this.props.onConfirm();
            },
            this.props.icon,
        );
    };
}
