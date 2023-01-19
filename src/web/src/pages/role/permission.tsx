import React from "react";
import Modal from "../../controls/model";
import { IRoleState } from "./store/model";
import { RoleActions } from "./store/action";
import Form, { FormComponentProps } from "antd/lib/form";
import { Tree } from "antd";
import { formItemLayout } from "../../helpers/base";

type IProps = IRoleState & typeof RoleActions & FormComponentProps;

interface IStates {
    perID: string[];
}

class RolePermission extends React.Component<IProps, IStates> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            perID: [],
        };
    }

    public componentDidUpdate(prevProps: IProps) {
        if (!prevProps.rolePermission.open && this.props.rolePermission.open) {
            this.setState({ perID: this.props.rolePermission.roleData });
        }
    }

    public render() {
        return (
            <Modal
                title="Roles"
                visible={this.props.rolePermission.open}
                okText="Confirm"
                cancelText="Cancel"
                onCancel={this.onCancel}
                onOk={this.onOk}
                okButtonProps={{
                    loading: this.props.rolePermission.loading,
                }}>
                {this.renderForm()}
            </Modal>
        );
    }

    private renderForm = () => {
        return (
            <Form layout="horizontal" {...formItemLayout}>
                <Form.Item>
                    <Tree
                        showLine={true}
                        checkable={true}
                        defaultCheckedKeys={this.props.rolePermission.roleData}
                        checkedKeys={this.state.perID}
                        onCheck={this.onCheck}>
                        {this.getTreeNodes()}
                    </Tree>
                </Form.Item>
            </Form>
        );
    };

    private onOk = (event) => {
        event.preventDefault();
        this.props.form.validateFields((err, values) => {
            if (!err) {
                let list: string[] = [];
                if (this.state.perID.length > 0) {
                    list = this.state.perID.filter((x) => !x.startsWith("C"));
                }
                this.props.sendPermission({ roleId: this.props.rolePermission.roleId, permissionIds: list });
            }
        });
    };

    private onCancel = (event) => {
        this.props.toggleRolePermissionModal(false);
    };

    private getTreeNodes = () => {
        const { permissionData } = this.props.rolePermission;
        return permissionData.map((x) => {
            if (x.permissions && x.permissions.length > 0) {
                return (
                    <Tree.TreeNode title={`${x.title}`} key={`${x.id}`}>
                        {x.permissions.map((z) => (
                            <Tree.TreeNode title={`${z.title}`} key={`${z.id}`} />
                        ))}
                    </Tree.TreeNode>
                );
            }
            return <Tree.TreeNode title={`${x.title}`} key={`${x.id}`} />;
        });
    };

    private onCheck = (checkedKeys, _) => {
        const perID = checkedKeys;
        this.setState({ perID });
    };
}

const RolePermissionForm = Form.create<IProps>()(RolePermission);

export default RolePermissionForm;
