import React from "react";
import * as echarts from "echarts";
import elementResizeEvent from "element-resize-event";

export const Colors = [
    "#37A2DA",
    "#32C5E9",
    "#67E0E3",
    "#9FE6B8",
    "#FFDB5C",
    "#ff9f7f",
    "#fb7293",
    "#E062AE",
    "#E690D1",
    "#e7bcf3",
    "#9d96f5",
    "#8378EA",
    "#96BFFF",
];

const defaultOption = {
    textStyle: { fontFamily: "OpenSans" },
    color: Colors,
};

interface IProps {
    height: number;
    option: any;
    showLoading?: boolean;
    onClick?: any;
    onReady?: any;
}

class Chart extends React.Component<IProps> {
    private chartRef: any;

    constructor(props) {
        super(props);
        this.chartRef = React.createRef();
    }

    public componentDidMount() {
        const chart = this.renderChart();
        elementResizeEvent(this.chartRef.current, () => {
            chart.resize();
        });
        const { onReady } = this.props;
        if (typeof onReady === "function") {
            onReady(chart);
        }
    }

    public componentDidUpdate() {
        const chart = this.renderChart();
        chart.resize();
    }

    public componentWillUnmount() {
        echarts.dispose(this.chartRef.current);
    }

    public render() {
        const { height } = this.props;
        var title = this.props.option.title.text;
        return (
            <div className="echart-wrapper">
                {title ? (
                    <div>
                        <h3>{title}</h3>
                    </div>
                ) : null}
                <div ref={this.chartRef} style={{ height }} />
            </div>
        );
    }

    private renderChart() {
        const chartDom = this.chartRef.current;
        const chart = echarts.init(chartDom);
        const { onClick, option, showLoading } = this.props;
        if (onClick) {
            chart.off("click");
            chart.on("click", onClick);
        }
        const options = Object.assign({}, option, defaultOption);
        delete options.title;
        chart.setOption(options);
        if (showLoading) {
            chart.showLoading();
        } else {
            chart.hideLoading();
        }
        return chart;
    }
}

export default Chart;
