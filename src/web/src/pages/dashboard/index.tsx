import React from "react";
import { RouteComponentProps } from "react-router-dom";
import DashboardLayout from "../../layouts/dashboard";
import DashboardHeader from "./header";
import DashboardMenu from "./menu";
import DashboardRouter from "./router";

class Dashboard extends React.Component<RouteComponentProps<{}>> {
    public render() {
        const { ...props } = this.props;
        return (
            <DashboardLayout menu={this.getMenu} header={this.getHeader} {...props}>
                <DashboardRouter />
            </DashboardLayout>
        );
    }

    private getMenu = () => {
        return <DashboardMenu {...this.props} />;
    };

    private getHeader = () => {
        return <DashboardHeader />;
    };
}

export default Dashboard;
