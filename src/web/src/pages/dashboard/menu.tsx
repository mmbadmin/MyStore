import React from "react";
import { Menu } from "antd";
import { RouteComponentProps, Link } from "react-router-dom";
import WithUserContext from "../../controls/hoc";
import { IPermissionState } from "../../helpers/model";
import { permission } from "../../helpers/access";
import { SelectParam } from "antd/lib/menu";

interface IMenuItem {
    key: string;
    title: string;
    url?: string;
    permissions?: string;
    children: IMenuItem[];
    show?: boolean;
}

interface IStates {
    selectedKeys: string[];
    openKeys: string[];
    menuItems: IMenuItem[];
}

const flat = (menuItems: IMenuItem[]): IMenuItem[] => {
    const items: IMenuItem[] = [];
    for (const menuItem of menuItems) {
        items.push(menuItem);
        if (menuItem.children.length > 0) {
            items.push(...flat(menuItem.children));
        }
    }
    return items;
};

interface IProps extends RouteComponentProps<any>, IPermissionState {}

class DashboardMenu extends React.Component<IProps, IStates> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            selectedKeys: [],
            openKeys: [],
            menuItems: [
                {
                    key: "1",
                    url: "/",
                    permissions: "",
                    title: "Home",
                    children: [],
                    show: true,
                },
                {
                    key: "2",
                    title: "Users&Roles",
                    permissions: permission.Access.user_UserGetPagedListQueryHandler,
                    children: [
                        {
                            key: "2-1",
                            title: "Users",
                            url: "/User",
                            permissions: permission.Access.user_UserGetPagedListQueryHandler,
                            children: [],
                        },
                        {
                            key: "2-2",
                            title: "Role",
                            url: "/Role",
                            permissions: permission.Access.role_RoleGetPagedListQueryHandler,
                            children: [],
                        },
                    ],
                },
               
            ],
        };
    }

    private showParent = (menuItems: IMenuItem[]) => {
        const { havePermission } = this.props.permissions;

        if (menuItems && menuItems.length) {
            for (let i = 0; i < menuItems.length; i++) {
                const menuItem = menuItems[i];
                if (menuItem.children && menuItem.children.length > 0) {
                    var showParent = this.showParent(menuItem.children);
                    if (showParent === true) {
                        return showParent;
                    } else {
                        continue;
                    }
                }
                if (menuItem.show) {
                    return true;
                }
                const show = havePermission(menuItem.permissions);
                if (show) {
                    return true;
                }
            }
        }
        return false;
    };

    private renderMenuItem = (menuItem: IMenuItem) => {
        const { havePermission } = this.props.permissions;
        if (menuItem.children.length === 0 && menuItem.url) {
            if (menuItem.show || havePermission(menuItem.permissions)) {
                return (
                    <Menu.Item key={menuItem.key}>
                        <Link to={menuItem.url}>{menuItem.title}</Link>
                    </Menu.Item>
                );
            }
        } else {
            const show = this.showParent(menuItem.children);
            if (show) {
                return (
                    <Menu.SubMenu key={menuItem.key} className="submenu" title={menuItem.title}>
                        {menuItem.children.map(this.renderMenuItem)}
                    </Menu.SubMenu>
                );
            }
        }
        return null;
    };

    private renderMenu = () => {
        return this.state.menuItems.map(this.renderMenuItem);
    };

    public render() {
        const { selectedKeys, openKeys } = this.state;
        return (
            <Menu
                onClick={(_) => window.scrollTo(0, 0)}
                onSelect={this.onSelect}
                onOpenChange={this.onOpenChange}
                theme="light"
                mode="inline"
                openKeys={openKeys}
                selectedKeys={selectedKeys}>
                {this.renderMenu()}
            </Menu>
        );
    }

    public componentDidMount() {
        this.setMenu();
    }

    public componentDidUpdate(prevProps: Readonly<IProps>) {
        if (this.props.location.pathname !== prevProps.location.pathname) {
            this.setMenu();
        }
    }

    private onSelect = (param: SelectParam) => {
        this.setState({ selectedKeys: param.selectedKeys });
    };

    private onOpenChange = (openKeys: string[]) => {
        this.setState({ openKeys: openKeys });
    };

    private setMenu = () => {
        const list = flat(this.state.menuItems).map((o) => {
            return {
                key: o.key,
                url: o.url,
            };
        });
        const selected = list.filter((x) => x.url === this.props.location.pathname).map((x) => x.key)[0];
        if (selected) {
            const key: string[] = [];
            if (selected.indexOf("-") > -1) {
                const selectedSplit = selected.split("-");
                if (selectedSplit.length > 0) {
                    let t: string = "";
                    for (let i = 0; i < selectedSplit.length; i++) {
                        if (i === 0) {
                            t = selectedSplit[i];
                        } else {
                            t += "-" + selectedSplit[i];
                        }
                        key.push(t);
                    }
                }
            }

            this.setState({ selectedKeys: [selected], openKeys: key.filter((x) => x !== selected) });
        } else {
            this.setState({ selectedKeys: [], openKeys: ["1"] });
        }
    };
}

export default WithUserContext(DashboardMenu);
