import React from "react";
import { Layout } from "antd";
import { IPermissionState } from "../helpers/model";
import { RouteComponentProps } from "react-router-dom";
import WithUserContext from "../controls/hoc";

interface IProps extends RouteComponentProps<{}>, IPermissionState {
    children?: any;
    menu: any;
    header: any;
}

class DashboardLayout extends React.Component<IProps> {
    public render() {
        const { children, header, menu } = this.props;
        return (
            <Layout>
                <Layout.Header>{header()}</Layout.Header>
                <Layout>
                    <Layout.Sider width="200" breakpoint="xl" collapsedWidth="0">
                        {menu()}
                    </Layout.Sider>
                    <Layout.Content>
                        <div className="container">{children}</div>
                    </Layout.Content>
                </Layout>
            </Layout>
        );
    }
}

export default WithUserContext(DashboardLayout);
