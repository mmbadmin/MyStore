import React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../store/state";
import { IRoleState } from "./store/model";
import { RoleActions } from "./store/action";
import Table from "../../controls/table";
import Header from "../../controls/header";
import Auth from "../../controls/auth";
import { CreateButton, ConfirmButton } from "../../controls/button";
import { permission } from "../../helpers/access";
import { Button, Tooltip, Icon } from "antd";
import RoleCreate from "./create";
import RoleUpdate from "./update";
import RolePermission from "./permission";
import { IPermissionState } from "../../helpers/model";
import WithUserContext from "../../controls/hoc";

type IProps = IRoleState & typeof RoleActions & IPermissionState;

class RoleList extends React.Component<IProps> {
    public componentDidMount() {
        this.props.getPagedList(1);
        if (this.props.permissions.havePermission(permission.Access.role_RoleCreateCommandHandler)) {
            this.props.getPermissionList();
        }
    }

    public render() {
        const { ...props } = this.props;
        return (
            <React.Fragment>
                <Header title="Roles" />
                <Auth policy={permission.Access.role_RoleCreateCommandHandler}>
                    <CreateButton onClick={() => this.props.toggleRoleCreateModal(true)} />
                </Auth>
                {this.renderTable()}
                <RoleCreate {...props} />
                <RoleUpdate {...props} />
                <RolePermission {...props} />
            </React.Fragment>
        );
    }

    private onTableChange = (pagination, filters, sorter) => {
        this.props.getPagedList(pagination.current, pagination.pageSize, sorter.columnKey, sorter.order, filters);
    };

    private renderTable = () => {
        const { data, loading } = this.props.roleList;
        return (
            <Table rowKey="id" dataSet={data} loading={loading} onChange={this.onTableChange}>
                <Table.Column key="title" dataIndex="title" title="Title" sorter={true} hasFilter={true} />
                <Table.Column key="description" dataIndex="description" title="Description" sorter={true} />
                <Table.Column key="button" title="" render={this.renderColumnButton} />
            </Table>
        );
    };

    private renderColumnButton = (_, record) => {
        return (
            <Button.Group>
                <Auth policy={permission.Access.role_RoleUpdateCommandHandler}>
                    <Tooltip placement="top" title="Edit">
                        <Button onClick={() => this.props.getItem(record.id)}>
                            <Icon type="edit" theme="outlined" />
                        </Button>
                    </Tooltip>
                </Auth>
                <Auth policy={permission.Access.role_RoleGetItemQueryHandler}>
                    <Tooltip placement="top" title="Roles">
                        <Button onClick={() => this.props.getRolePermissionList(record.id)}>
                            <Icon type="lock" theme="outlined" />
                        </Button>
                    </Tooltip>
                </Auth>
                <Auth policy={permission.Access.role_RoleDeleteCommandHandler}>
                    <Tooltip placement="top" title="Delete">
                        <ConfirmButton
                            confirmText="Delete Selected role?"
                            confirmTitle="Delete"
                            onConfirm={() => this.props.sendDelete(record.id)}>
                            <Icon type="delete" theme="outlined" />
                        </ConfirmButton>
                    </Tooltip>
                </Auth>
            </Button.Group>
        );
    };
}

export default connect((state: IApplicationState) => state.role, RoleActions)(WithUserContext(RoleList));
