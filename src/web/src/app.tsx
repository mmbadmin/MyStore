import React from "react";
import AuthRoute from "./controls/authRoute";
import EmptyLayout from "./layouts/empty";
import Dashboard from "./pages/dashboard";
import { ConfigProvider } from "antd";
import enUS from "antd/lib/locale-provider/en_US";

import { Switch } from "react-router-dom";



export default () => (
    <EmptyLayout>
        <ConfigProvider locale={enUS}>
            <Switch>
                <AuthRoute path="/" component={Dashboard} />
            </Switch>
        </ConfigProvider>
    </EmptyLayout>
);
