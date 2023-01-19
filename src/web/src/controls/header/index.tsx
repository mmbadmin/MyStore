import React from "react";
import { Breadcrumb, Icon, Divider, Tooltip } from "antd";
import { Link } from "react-router-dom";

interface IProps {
    title: string;
}

const Header = (props: IProps) => {
    return (
        <Divider orientation="left" className="bheader">
            <Breadcrumb separator=">">
                <Breadcrumb.Item>
                    <Tooltip title={`Back To Home`}>
                        <Link to="/">
                            <Icon type="home" />
                        </Link>
                    </Tooltip>

                </Breadcrumb.Item>
                <Breadcrumb.Item>{props.title}</Breadcrumb.Item>
            </Breadcrumb>
        </Divider>
    );
};

export default Header;
