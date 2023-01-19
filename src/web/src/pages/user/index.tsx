import React from "react";
import { RouteComponentProps } from "react-router-dom";
import { connect } from "react-redux";
import { IApplicationState } from "../../store/state";
import { IUserState } from "./store/model";
import { UserActions } from "./store/action";
import Table from "../../controls/table";
import Header from "../../controls/header";
import Auth from "../../controls/auth";
import { CreateButton, ConfirmButton } from "../../controls/button";
import { permission } from "../../helpers/access";
import { Button, Tooltip, Icon } from "antd";
import UserCreate from "./create";
import UserUpdate from "./update";
import UserRole from "./role";
import UserPassword from "./password";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLock, faUnlock, faKey, faUserLock } from "@fortawesome/free-solid-svg-icons";

type IProps = IUserState & typeof UserActions & RouteComponentProps<{}>;

class UserList extends React.Component<IProps> {
    public componentDidMount() {
        this.props.getRoles();
        this.props.getPagedList(1);
    }

    public render() {
        const { ...props } = this.props;
        return (
            <React.Fragment>
                <Header title={`Users`} />
                <Auth policy={permission.Access.user_UserCreateCommandHandler}>
                    <CreateButton onClick={() => this.props.toggleUserCreateModal(true)} />
                </Auth>
                {this.renderTable()}
                <UserCreate {...props} />
                <UserUpdate {...props} />
                <UserRole {...props} />
                <UserPassword {...props} />
            </React.Fragment>
        );
    }

    public componentDidUpdate(prevProps: IProps) {
        if (prevProps.match.url !== this.props.match.url) {
            this.props.getPagedList(1);
        }
    }

    private onTableChange = (pagination, filters, sorter) => {
        this.props.getPagedList(pagination.current, pagination.pageSize, sorter.columnKey, sorter.order, filters);
    };

    private renderTable = () => {
        const { data, loading } = this.props.userList;
        return (
            <Table rowKey="id" dataSet={data} loading={loading} onChange={this.onTableChange}>
                <Table.Column key="userName" dataIndex="userName" title="UserName" sorter={true} hasFilter={true} />
                <Table.Column key="firstName" dataIndex="firstName" title="FirstName" sorter={true} hasFilter={true} />
                <Table.Column key="lastName" dataIndex="lastName" title="LastName" sorter={true} hasFilter={true} />
                <Table.IconColumn
                    key="isActive"
                    dataIndex="isActive"
                    title="Active Status"
                    sorter={true}
                    hasFilter={true}
                />
                <Table.Column key="button" title="" render={this.renderColumnButton} />
            </Table>
        );
    };

    private renderColumnButton = (_, record) => {
        return (
            <Button.Group>
                <Auth policy={permission.Access.user_UserUpdateCommandHandler}>
                    <Tooltip placement="top" title="Edit">
                        <Button onClick={() => this.props.getItem(record.id)}>
                            <Icon type="edit" theme="outlined" />
                        </Button>
                    </Tooltip>
                </Auth>
                <Auth policy={permission.Access.user_UserResetPasswordCommandHandler}>
                    <Tooltip placement="top" title="Change Password">
                        <Button onClick={() => this.props.setPasswordItemId(record.id)}>
                            <FontAwesomeIcon icon={faKey} />
                        </Button>
                    </Tooltip>
                </Auth>
                <Auth policy={permission.Access.user_UserRoleUpdateCommandHandler}>
                    <Tooltip placement="top" title="Role User">
                        <Button onClick={() => this.props.getUserRoles(record.id)}>
                            <Icon type="usergroup-add" />
                        </Button>
                    </Tooltip>
                </Auth>
                {record.isActive ? (
                    <Auth policy={permission.Access.user_UserDeactiveCommandHandler}>
                        <Tooltip placement="top" title="Deactivate User">
                            <Button onClick={() => this.props.sendDeactive(record.id)}>
                                <FontAwesomeIcon icon={faLock} />
                            </Button>
                        </Tooltip>
                    </Auth>
                ) : (
                    <Auth policy={permission.Access.user_UserActiveCommandHandler}>
                        <Tooltip placement="top" title="Activate User">
                            <Button onClick={() => this.props.sendActive(record.id)}>
                                <FontAwesomeIcon icon={faUnlock} />
                            </Button>
                        </Tooltip>
                    </Auth>
                )}
                {record.isLocked ? (
                    <Auth policy={permission.Access.user_UserUnlockCommandHandler}>
                        <Tooltip placement="top" title="Set Unlock">
                            <Button onClick={() => this.props.sendUnlock(record.id)}>
                                <FontAwesomeIcon icon={faUserLock} />
                            </Button>
                        </Tooltip>
                    </Auth>
                ) : null}
                <Auth policy={permission.Access.user_UserUpdateCommandHandler}>
                    <Tooltip placement="top" title="Delete">
                        <ConfirmButton
                            confirmText="Delete selected user ?"
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

export default connect((state: IApplicationState) => state.user, UserActions)(UserList);
