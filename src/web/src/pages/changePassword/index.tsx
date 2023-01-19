import React from "react";
import { connect } from "react-redux";
import { LoginActions } from "../login/store/action";
import { ILoginState } from "../login/store/model";
import { RouteComponentProps } from "react-router-dom";
import Form, { FormComponentProps } from "antd/lib/form";
import { Row, Col, Input, Divider } from "antd";
import { formItemLayout } from "../../helpers/base";
import { IApplicationState } from "../../store/state";
import WithUserContext from "../../controls/hoc";
import { CancelButton, CreateButton } from "../../controls/button";
import { Const } from "../../helpers/const";

type IProps = ILoginState & typeof LoginActions & RouteComponentProps<{}> & FormComponentProps;

interface IStates {
    confirmDirty: boolean;
}




class ChangePasswordPage extends React.Component<IProps, IStates> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            confirmDirty: false,
        };
    }


    public render() {
        return (
            <React.Fragment>
               
                {this.renderForm()}
                <CreateButton createTitle="Save" onClick={this.onOk} />
                <Divider type="vertical"></Divider>
                <CancelButton onClick={() => this.backtoIndex()} />
                </React.Fragment>
        );
    }

    private renderForm = () => {
        const { getFieldDecorator } = this.props.form;
        return (
            <Form layout="horizontal" {...formItemLayout} onSubmit={this.onOk}>
                <Row>
                <Col >
                        <Form.Item label="Cuurent Password ">
                            {getFieldDecorator("oldPassword", {
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
                            })(<Input placeholder="Cuurent Password" type="password" />)}
                        </Form.Item>
                    </Col>
                    <Col >
                        <Form.Item label="Password ">
                            {getFieldDecorator("newpassword", {
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
                        <Form.Item label="Confirm Password">
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
                            })(<Input placeholder="Confirm Password" type="password" onBlur={this.handleConfirmBlur} />)}
                        </Form.Item>
                    </Col>
                </Row>
            </Form>
        );
    };

    private backtoIndex = () => {
        this.props.history.push("/");
    };

    private backtoLogin = () => {
        window.location.href = Const.PublicURI;
    };

   

    public onOk = (event) => {
        event.preventDefault();
        this.props.form.validateFields((err, values) => {
            if (!err) {
                this.props.changePassword(values, () => {
                    this.backtoLogin();
                });
            }
        });
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
        if (value && value !== form.getFieldValue("newpassword")) {
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

const ChangePasswordPageForm = Form.create<IProps>()(ChangePasswordPage);

export default connect(
    (state: IApplicationState) => state.login,
    LoginActions,
)(WithUserContext(ChangePasswordPageForm));
