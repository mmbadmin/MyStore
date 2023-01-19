import React from "react";
import Modal from "../../controls/model";
import { IRoleState } from "./store/model";
import { RoleActions } from "./store/action";
import Form, { FormComponentProps } from "antd/lib/form";
import { Row, Col, Input } from "antd";
import { formItemLayout, TwoColumnLayout } from "../../helpers/base";

type IProps = IRoleState & typeof RoleActions & FormComponentProps;

class RoleUpdate extends React.Component<IProps> {
    public render() {
        return (
            <Modal
                title="Edit"
                visible={this.props.roleUpdate.open}
                okText="Confirm"
                cancelText="Cancel"
                onCancel={this.onCancel}
                onOk={this.onOk}
                okButtonProps={{
                    loading: this.props.roleUpdate.loading,
                }}>
                {this.renderForm()}
            </Modal>
        );
    }

    private renderForm = () => {
        const { getFieldDecorator } = this.props.form;
        const { roleItem } = this.props;
        return (
            <Form layout="horizontal" {...formItemLayout}>
                <Row>
                    <Col {...TwoColumnLayout}>
                        <Form.Item label="Title">
                            {getFieldDecorator("title", {
                                initialValue: roleItem.title,
                                rules: [
                                    {
                                        message: "User Enter the title",
                                        required: true,
                                    },
                                ],
                            })(<Input placeholder="Title" />)}
                        </Form.Item>
                    </Col>
                    <Col {...TwoColumnLayout}>
                        <Form.Item label="Description">
                            {getFieldDecorator("description", {
                                initialValue: roleItem.description,
                            })(<Input placeholder="Description" />)}
                        </Form.Item>
                    </Col>
                </Row>
            </Form>
        );
    };

    private onOk = (event) => {
        event.preventDefault();
        this.props.form.validateFields((err, values) => {
            if (!err) {
                this.props.sendUpdate({ id: this.props.roleUpdate.itemId, ...values });
            }
        });
    };

    private onCancel = (event) => {
        this.props.toggleRoleUpdateModal(false);
    };
}

const RoleUpdateForm = Form.create<IProps>()(RoleUpdate);

export default RoleUpdateForm;
