import { Button, Form, Icon, Input } from "antd";
import { FormComponentProps } from "antd/lib/form";
import React from "react";
import { connect } from "react-redux";
import { LoginActions } from "./store/action";
import { ILoginState } from "./store/model";
import WithUserContext from "../../controls/hoc";
import { HasErrors } from "../../helpers/base";
import { IApplicationState } from "../../store/state";
import { RouteComponentProps } from "react-router-dom";
import { IPermissionState } from "../../helpers/model";
import Img1 from "../../styles/images/login/1.jpg";

const images = [Img1];

type IProps = FormComponentProps & ILoginState & typeof LoginActions & RouteComponentProps<{}> & IPermissionState;
interface IState {
    imageIndex: number;
    reload: boolean;
}

class Login extends React.Component<IProps, IState> {
    constructor(props: IProps) {
        super(props);

        this.state = {
            imageIndex: Math.floor((Math.random() * 100) % images.length),
            reload: true,
        };
    }

    componentDidUpdate(prevProps: IProps) {
        if (this.props.failed && !prevProps.failed) {
            this.setState({ reload: true });
        }
    }

    public render() {
        const { getFieldDecorator, getFieldsError } = this.props.form;
        return (
            <div className="signin-wrapper" style={{ backgroundImage: `url(${images[this.state.imageIndex]})` }}>
                <div className="signin-content">
                    <h3 className="signin-brand">MyStore</h3>
                    <Form onSubmit={this.onFormSubmit} className="singup-form">
                        <Form.Item>
                            {getFieldDecorator("username", {
                                rules: [
                                    {
                                        message: "Enter the username",
                                        required: true,
                                    },
                                ],
                            })(<Input placeholder="UserName" prefix={<Icon type="user" />} />)}
                        </Form.Item>
                        <Form.Item>
                            {getFieldDecorator("password", {
                                rules: [
                                    {
                                        message: "Enter the password",
                                        required: true,
                                    },
                                ],
                            })(<Input placeholder="Password" type="password" prefix={<Icon type="key" />} />)}
                        </Form.Item>
                        {/* <Form.Item>
                            {getFieldDecorator("captcha", {
                                rules: [
                                    {
                                        required: true,
                                        message: "Enter the captcha code",
                                    },
                                ],
                            })(
                                <BICTCaptcah
                                    placeholder="Captcha code"
                                    reload={this.state.reload}
                                    afterReload={() => this.setState({ reload: false })}
                                />,
                            )}
                        </Form.Item> */}
                        <Form.Item>
                            <div className="signin-button-group">
                                <Button
                                    className="submit-button"
                                    type="primary"
                                    htmlType="submit"
                                    disabled={HasErrors(getFieldsError()) || this.props.loading}>
                                    Login
                                </Button>
                            </div>
                        </Form.Item>
                    </Form>
                </div>
            </div>
        );
    }

    private onFormSubmit = (event: any) => {
        event.preventDefault();
        this.props.form.validateFields((err, values) => {
            if (!err) {
                this.props.loginSend(values, (token: string) => {
                    this.props.permissions.setToken(token);
                });
            }
        });
    };
}

const LoginForm = Form.create<IProps>()(Login);

export default connect((state: IApplicationState) => state.login, LoginActions)(WithUserContext(LoginForm));
