import React from "react";
import { connect } from "react-redux";
import { IDashboardPageState } from "./store/model";
import { DashboardPageActions } from "./store/action";
import { IApplicationState } from "../../store/state";
import Auth from "../../controls/auth";
import { permission } from "../../helpers/access";
import WithUserContext from "../../controls/hoc";
import { IPermissionState } from "../../helpers/model";
import { Row, Collapse, Col, Icon } from "antd";
import "./index.less";
import { Link } from "react-router-dom";

type IProps = IDashboardPageState & typeof DashboardPageActions & IPermissionState;

class DashboardPage extends React.Component<IProps> {
    componentDidMount() {
        if (this.props.permissions.havePermission(permission.Access.sladbForm_SLADBFormGetPagedListQueryHandler)) {
            this.props.getCategoryDashboardItem();
        }
    }

    public render() {
        return (
            <React.Fragment>
                <Auth policy={permission.Access.sladbForm_SLADBFormGetPagedListQueryHandler}>
                    {this.renderFirstCard()}
                </Auth>

            </React.Fragment>
        );
    }



    private renderFirstCard = () => {
        const { categoryDashboardItem } = this.props;
        const { Panel } = Collapse;

        if (!categoryDashboardItem) {
            return null;
        }
        const listItems = categoryDashboardItem.sort((a, b) => a.title.localeCompare(b.title)).map((d) =>

            <Panel header={d.title} key={d.id}>
                {this.renderForm(d.id)}
            </Panel>

        );

        return (
            <React.Fragment>
                <Row>
                    <Collapse accordion onChange={(key) => this.renderForm(key)} >
                        {listItems}
                    </Collapse>
                </Row>
            </React.Fragment>
        );
    };


    private renderForm = (id) => {
        const { categoryDashboardItem } = this.props;
        if (!categoryDashboardItem) {
            return null;
        }
        const formlist= categoryDashboardItem?.find(x => x.id === id)?.details;
        const listItems = formlist?.sort((a, b) => a.title.localeCompare(b.title)).map((d) =>
         <Col key={d.id} >
            <Icon type="form"  className="ant-icon"/>
            <Link to={`SLADBForm/${d.id}/${d.title.replace(/ /g, '~~')}`} >
                {d.title}
            </Link>
        </Col>
        );

        return (
            <React.Fragment>
                <Row>
                    {listItems}
                </Row>
            </React.Fragment>
        );
    };


}

export default connect(
    (state: IApplicationState) => state.dashboardPage,
    DashboardPageActions,
)(WithUserContext(DashboardPage));
