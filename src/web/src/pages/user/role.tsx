import React from "react";
import Modal from "../../controls/model";
import { IUserState } from "./store/model";
import { UserActions } from "./store/action";
import { RouteComponentProps } from "react-router-dom";
import Form, { FormComponentProps } from "antd/lib/form";
import { Row, Col, Tree } from "antd";
import { formItemLayout } from "../../helpers/base";

type IProps = IUserState & typeof UserActions & RouteComponentProps<{}> & FormComponentProps;

interface IStates {
    perID: string[];
}

class UserRole extends React.Component<IProps, IStates> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            perID: [],
        };
    }

    public componentDidUpdate(prevProps: IProps) {
        if (!prevProps.userRole.open && this.props.userRole.open) {
            this.setState({ perID: this.props.userRole.data });
        }
    }

    public render() {
        return (
            <Modal
                title="User roles"
                visible={this.props.userRole.open}
                okText="Confirm"
                cancelText="Cancel"
                onCancel={this.onCancel}
                onOk={this.onOk}
                okButtonProps={{
                    loading: this.props.userRole.loading,
                }}>
                {this.renderForm()}
            </Modal>
        );
    }

    private renderForm = () => {
        return (
            <Form layout="horizontal" {...formItemLayout}>
                <Row>
                    <Col>
                        <Col>
                            <Form.Item label="Roles">
                                <Tree
                                    checkable={true}
                                    defaultCheckedKeys={this.props.userRole.data}
                                    checkedKeys={this.state.perID}
                                    onCheck={this.onCheck}>
                                    {this.getTreeNodes()}
                                </Tree>
                            </Form.Item>
                        </Col>
                    </Col>
                </Row>
            </Form>
        );
    };

    private getTreeNodes = () => {
        const { roles } = this.props;
        return roles.map((x) => {
            return <Tree.TreeNode title={`${x.title}`} key={`${x.id}`} />;
        });
    };

    private onOk = (event) => {
        event.preventDefault();
        this.props.form.validateFields((err, values) => {
            if (!err) {
                this.props.sendUserRoles({ userId: this.props.userRole.itemId, roleIds: this.state.perID });
            }
        });
    };

    private onCancel = (event) => {
        this.props.toggleUserRoleModal(false);
    };

    private onCheck = (checkedKeys, _) => {
        const perID = checkedKeys;
        this.setState({ perID });
    };
}

const UserRoleForm = Form.create<IProps>()(UserRole);

export default UserRoleForm;
