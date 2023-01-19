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

class UserCreate extends React.Component<IProps, IStates> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            confirmDirty: false,
        };
    }

    public render() {
        return (
            <Modal
                title="Create New User"
                visible={this.props.userCreate.open}
                onCancel={this.onCancel}
                onOk={this.onOk}
                okText="Confirm"
                cancelText="Cancel"
                okButtonProps={{
                    loading: this.props.userCreate.loading,
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
                    <Col >
                        <Form.Item label="First Name">
                            {getFieldDecorator("firstName", {
                                rules: [
                                    {
                                        message: "Enter the name",
                                        required: true,
                                    },
                                ],
                            })(<Input placeholder="First Name" />)}
                        </Form.Item>
                    </Col>
                    <Col >
                        <Form.Item label="Last Name">
                            {getFieldDecorator("lastName", {
                                rules: [
                                    {
                                        message: "Enter the last name",
                                        required: true,
                                    },
                                ],
                            })(<Input placeholder="Last Name" />)}
                        </Form.Item>
                    </Col>
                    <Col >
                        <Form.Item label="UserName">
                            {getFieldDecorator("userName", {
                                rules: [
                                    {
                                        message: "Enter the username",
                                        required: true,
                                    },
                                ],
                            })(<Input placeholder="UserName" />)}
                        </Form.Item>
                    </Col>
                    <Col >
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
                    <Col >
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
                this.props.sendCreate(values);
            }
        });
    };

    private onCancel = (event) => {
        this.props.toggleUserCreateModal(false);
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

const UserCreateForm = Form.create<IProps>()(UserCreate);

export default UserCreateForm;
