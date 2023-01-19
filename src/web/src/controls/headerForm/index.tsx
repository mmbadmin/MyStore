import React from "react";
import { Breadcrumb, Icon, Divider, Tooltip } from "antd";
import { Link } from "react-router-dom";

interface IProps {
    title: string;
    formid: number;
    formName: string;
}

const FormHeader = (props: IProps) => {
    return (
        <Divider orientation="left" className="bheader">
            <Breadcrumb separator=">">
                <Breadcrumb.Item>
                    <Tooltip title={`Back To ${props.formName}`}>
                    <Link to={`/SLADBForm/${props.formid}/${props.formName}`} >
                        <Icon type="rollback" />
                    </Link>
                    </Tooltip>
                </Breadcrumb.Item>
                <Breadcrumb.Item>{props.title}</Breadcrumb.Item>
            </Breadcrumb>
        </Divider>
    );
};

export default FormHeader;
