import React from "react";
import Modal from "../../controls/model";
import { IUserState } from "./store/model";
import { UserActions } from "./store/action";
import Form, { FormComponentProps } from "antd/lib/form";
import { Row, Col, Input } from "antd";
import { TwoColumnLayout, formItemLayout } from "../../helpers/base";

type IProps = IUserState & typeof UserActions & FormComponentProps;

class UserUpdate extends React.Component<IProps> {
    public render() {
        return (
            <Modal
                title="Edit"
                visible={this.props.userUpdate.open}
                okText="Confirm"
                cancelText="Cancel"
                onCancel={this.onCancel}
                onOk={this.onOk}
                okButtonProps={{
                    loading: this.props.userUpdate.loading,
                }}>
                {this.renderForm()}
            </Modal>
        );
    }

    private renderForm = () => {
        const { getFieldDecorator } = this.props.form;
        const { userItem } = this.props;
        return (
            <Form layout="horizontal" {...formItemLayout}>
                <Row>
                    <Col {...TwoColumnLayout}>
                    <Form.Item label="First Name">
                            {getFieldDecorator("firstName", {
                                initialValue: userItem.firstName,
                                rules: [
                                    {
                                        message: "Enter the name",
                                        required: true,
                                    },
                                ],
                            })(<Input placeholder="First Name" />)}
                        </Form.Item>
                    </Col>
                    <Col {...TwoColumnLayout}>
                    <Form.Item label="Last Name">
                            {getFieldDecorator("lastName", {
                                initialValue: userItem.lastName,
                                rules: [
                                    {
                                        message: "Enter the last name",
                                        required: true,
                                    },
                                ],
                            })(<Input placeholder="Last Name" />)}
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
                this.props.sendUpdate({ id: this.props.userUpdate.itemId, ...values });
            }
        });
    };

    private onCancel = (event) => {
        this.props.toggleUserUpdateModal(false);
    };
}

const UserUpdateForm = Form.create<IProps>()(UserUpdate);

export default UserUpdateForm;
