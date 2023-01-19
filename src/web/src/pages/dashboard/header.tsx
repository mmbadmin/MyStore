import React from "react";
import { Const } from "../../helpers/const";
import { Link, withRouter, RouteComponentProps } from "react-router-dom";
import { IPermissionState } from "../../helpers/model";
import BStorage from "../../helpers/storage";
import WithUserContext from "../../controls/hoc";
import { Icon, Dropdown, Menu } from "antd";

const menu = (onClick) => (
    <Menu onClick={onClick}>
        <Menu.Item key="changepass">
            <Icon type="lock" /> Change Password
        </Menu.Item>
        <Menu.Item key="logout">
            <Icon type="logout" /> LogOut
        </Menu.Item>
    </Menu>
);

interface IProps extends RouteComponentProps<any>, IPermissionState {}

class DashboardHeader extends React.Component<IProps> {
    private logout = () => {
        BStorage.removeItem(Const.AuthName);
        window.location.href = Const.PublicURI;
    };

    private onNoopClick = (event) => {
        event.preventDefault();
    };

    onMenuClick = (e) => {
        const key = e.key;
        switch (key) {
            case "changepass": {
                this.props.history.push("/ChangePassword");
                break;
            }
            case "logout": {
                this.logout();
                break;
            }
            default: {
                break;
            }
        }
    };

    public render() {
        const { user } = this.props.permissions;
        return (
            <div className="dashboard-header-wrapper">
                <div className="dashboard-header-logo">
                    <Link to="/">
                    </Link>
                    <h3>MyStore</h3>
                </div>
                <div className="dashboard-header-actions">
                    <p>
                        {user.firstName} {user.lastName}
                    </p>
                    <Dropdown overlay={menu(this.onMenuClick)} placement="bottomCenter">
                        <a className="ant-dropdown-link" href="#user" onClick={this.onNoopClick}>
                            <Icon type="user" theme="outlined" className="center" />
                        </a>
                    </Dropdown>
                    <a href="#notif" onClick={this.onNoopClick}>
                        <Icon type="bell" theme="outlined" className="center" />
                    </a>
                </div>
            </div>
        );
    }
}

export default withRouter(WithUserContext(DashboardHeader));
