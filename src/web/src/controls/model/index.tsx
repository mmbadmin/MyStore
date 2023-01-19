import React from "react";
import { Modal as AntModal, Spin } from "antd";
import { ModalProps } from "antd/lib/modal/Modal";

interface IProps extends ModalProps {
    spinHide?: boolean;
    spinText?: string;
}

class Modal extends React.Component<IProps> {
    public static defaultProps: Partial<IProps> = {
        width: "50%",
        spinText: "Sending information ...",
    };

    public render() {
        const { ...props } = this.props;
        let loading: boolean = false;
        if (props.okButtonProps !== undefined) {
            if (props.okButtonProps.loading) {
                loading = true;
            }
        }
        if (loading) {
            props.closable = false;
            props.cancelButtonProps = {
                disabled: true,
            };
        } else {
            props.closable = true;
            props.cancelButtonProps = {
                disabled: false,
            };
        }
        const { spinHide, spinText } = this.props;
        if (spinHide && loading) {
            return (
                <AntModal destroyOnClose={true} maskClosable={false} {...props}>
                    <div style={{ minHeight: "200px", padding: "50px 0", textAlign: "center" }}>
                        <Spin tip={spinText} />
                    </div>
                </AntModal>
            );
        } else {
            return (
                <AntModal destroyOnClose={true} maskClosable={false} {...props}>
                    <Spin spinning={loading} tip={spinText}>
                        {this.props.children}
                    </Spin>
                </AntModal>
            );
        }
    }
}

export default Modal;
