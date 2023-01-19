import React from "react";
import Modal from "../../controls/model";
import { IUserState } from "./store/model";
import { UserActions } from "./store/action";
import { RouteComponentProps } from "react-router-dom";
import Form, { FormComponentProps } from "antd/lib/form";
import { Row, Col, Input } from "antd";
import { formItemLayout } from "../../helpers/base";

type IProps = IUserState & typeof UserActions & RouteComponentProps<{}> & FormComponentProps;

interface IStates {
    confirmDirty: boolean;
}

class UserPassword extends React.Component<IProps, IStates> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            confirmDirty: false,
        };
    }

    public render() {
        return (
            <Modal
                title="Change Password"
                visible={this.props.userPassword.open}
                onCancel={this.onCancel}
                onOk={this.onOk}
                okText="Confirm"
                cancelText="Cancel"
                okButtonProps={{
                    loading: this.props.userPassword.loading,
                }}>
                {this.renderForm()}
            </Modal>
        );
    }

    private renderForm = () => {
        const { getFieldDecorator } = this.props.form;
        return (
            <Form layout="horizontal" {...formItemLayout}>
                <Row>
                    <Col>
                        <Form.Item label="Password ">
                            {getFieldDecorator("password", {
                                rules: [
                                    {
                                        message: "Enter the password",
                                        required: true,
                                    },
                                    {
                                        validator: this.validateToNextPassword,
                                    },
                                    { min: 6, message: "Password must be at least 6 characters long" },
                                ],
                            })(<Input placeholder="Password" type="password" />)}
                        </Form.Item>
                    </Col>
                    <Col>
                    <Form.Item label="repeat the password">
                            {getFieldDecorator("confirmPassword", {
                                rules: [
                                    {
                                        message: "Enter the password again",
                                        required: true,
                                    },
                                    {
                                        validator: this.compareToFirstPassword,
                                    },
                                ],
                            })(<Input placeholder="repeat the password" type="password" onBlur={this.handleConfirmBlur} />)}
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
                this.props.sendUserPassword({ id: this.props.userPassword.itemId, ...values });
            }
        });
    };

    private onCancel = (event) => {
        this.props.toggleUserPasswordModal(false);
    };

    private validateToNextPassword = (_, value, callback) => {
        const form = this.props.form;
        if (value && this.state.confirmDirty) {
            form.validateFields(["confirmPassword"], { force: true });
        }
        callback();
    };

    private compareToFirstPassword = (_, value, callback) => {
        const form = this.props.form;
        if (value && value !== form.getFieldValue("password")) {
            callback("A password is not the same as repeating a password");
        } else {
            callback();
        }
    };

    private handleConfirmBlur = (e) => {
        const value = e.target.value;
        this.setState({ confirmDirty: this.state.confirmDirty || !!value });
    };
}

const UserPasswordForm = Form.create<IProps>()(UserPassword);

export default UserPasswordForm;
