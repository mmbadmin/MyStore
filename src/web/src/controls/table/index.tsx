import { Button, Input, Table as AntTable, Icon, DatePicker } from "antd";
import { TableProps, PaginationConfig, SorterResult } from "antd/lib/table";
import AntColumn from "antd/lib/table/Column";
import React from "react";
import Column from "./column";
import DateColumn from "./dateColumn";
import IconColumn from "./iconColumn";
import FilterColumn from "./filterColumn";
import PriceColumn from "./priceColumn";
import IndexColumn from "./indexColumns";
import NumberColumn from "./numberColumn";
import SelectColumn from "./selectColumn";
import { IPagedList } from "../../helpers/model";
import Select from "../select";
import { PriceParser } from "../../helpers/base";
import NDate from "@nepo/ndate";
import isEmpty from "lodash.isempty";
import NumberInput from "../input/numberInput";

interface IProps<T> extends TableProps<T> {
    dataSet?: T[] | IPagedList;
}

interface IStates<T> {
    sorter?: SorterResult<T>;
    columns: any[];
    currentPage?: number;
    search: any;
}

const stringFilterList = [
    { value: "contains", title: "contains" },
    { value: "eq", title: "Equal" },
    { value: "gte", title: "Larger equals" },
    { value: "lte", title: "Less than equal" },
    { value: "gt", title: "bigger than" },
    { value: "neq", title: "Unequal" },
    { value: "lt", title: "Smaller than" },
    { value: "startswith", title: "Starts with" },
    { value: "endswith", title: "Ends with" },
    { value: "doesnotcontain", title: "Do not include this" },
];

const filterList = [
    { value: "eq", title: "Equal" },
    { value: "gte", title: "Larger equals" },
    { value: "lte", title: "Less than equal" },
    { value: "gt", title: "bigger than" },
    { value: "neq", title: "Unequal" },
    { value: "lt", title: "Smaller than" },
];

class Table<T> extends React.Component<IProps<T>, IStates<T>> {
    public static Column: typeof Column = Column;
    public static IconColumn: typeof IconColumn = IconColumn;
    public static DateColumn: typeof DateColumn = DateColumn;
    public static FilterColumn: typeof FilterColumn = FilterColumn;
    public static PriceColumn: typeof PriceColumn = PriceColumn;
    public static IndexColumn: typeof IndexColumn = IndexColumn;
    public static NumberColumn: typeof NumberColumn = NumberColumn;
    public static SelectColumn: typeof SelectColumn = SelectColumn;

    constructor(props: IProps<T>) {
        super(props);
        this.state = {
            sorter: undefined,
            columns: Array.isArray(this.props.children) ? this.props.children : [this.props.children],
            currentPage: 1,
            search: {},
        };
    }

    public render() {
        const { children, dataSet, onChange, ...rest } = this.props;
        const { columns } = this.state;
        rest.dataSource = this.getDataSource(dataSet);
        rest.pagination = this.getPagination(dataSet);
        rest.bordered = true;
        return (
            <AntTable {...rest} onChange={this.onTableChange} scroll={{ x: 400 }}>
                {this.GetColumn(columns)}
            </AntTable>
        );
    }

    onShowSizeChange = (current, pageSize) => {};

    private getPagination = (dataSet?: T[] | IPagedList): PaginationConfig | false => {
        if (!dataSet) {
            return false;
        }
        if (Array.isArray(dataSet)) {
            return false;
        }
        return {
            onShowSizeChange: this.onShowSizeChange,
            showSizeChanger: true,
            pageSizeOptions: ["5", "10", "25", "50"],
            total: dataSet.total,
            itemRender: this.getPaginationItemRender,
            showQuickJumper: true,
            showTotal: (total) => ` total : ${total} `,
        };
    };

    private getPaginationItemRender = (page, type) => {
        if (type === "prev") {
            return (
                <a className="ant-pagination-item-link">
                    <Icon type="left" theme="outlined" />
                </a>
            );
        }
        if (type === "next") {
            return (
                <a className="ant-pagination-item-link">
                    <Icon type="right" theme="outlined" />
                </a>
            );
        }
        return page;
    };

    private getDataSource = (dataSet?: T[] | IPagedList): T[] => {
        if (!dataSet) {
            return [];
        }
        if (Array.isArray(dataSet)) {
            return dataSet;
        } else {
            return dataSet.data;
        }
    };

    private onTableChange = (pagination: PaginationConfig, filters: any, sorter: SorterResult<T>) => {
        this.setState({ sorter });
        const filter: any = {};
        for (const key in filters) {
            if (filters.hasOwnProperty(key)) {
                const element = filters[key];
                if (isEmpty(element)) {
                    continue;
                }
                filter[key] = [];
                filter[key][0] = JSON.parse(element);
            }
        }
        this.setState({ currentPage: pagination.current });
        if (this.props.onChange) {
            this.props.onChange(pagination, filter, sorter, {
                currentDataSource: this.getDataSource(this.props.dataSet),
            });
        }
    };

    private onSearch = (values, setSelectedKeys, confirm) => () => {
        if (!values.operator) {
            values.operator = "eq";
        }
        const json = JSON.stringify(values);
        setSelectedKeys(json);
        confirm();
    };

    private onReset = (index, clearFilters) => () => {
        const { search } = this.state;
        delete search[index];
        this.setState({ ...this.state, search });
        clearFilters();
    };

    private RenderIconColumnFilterDropdown = (
        key,
        { activeText, inActiveText },
        { setSelectedKeys, selectedKeys, confirm, clearFilters },
    ) => {
        let { search } = this.state;
        return (
            <div style={{ padding: 8 }}>
                <Select
                    getPopupContainer={this.getSelectPopupContainer}
                    style={{ width: "380px" }}
                    value={search && search[key] && search[key].value}
                    allowClear={false}
                    onChange={(value) => {
                        if (!search) {
                            search = {};
                        }
                        if (!search[key]) {
                            search[key] = {};
                        }
                        search[key].operator = "eq";
                        search[key].value = String(value);
                        this.setState({ ...this.state, search });
                    }}>
                    <Select.Option key="true" value="true">
                        {activeText}
                    </Select.Option>
                    <Select.Option key="false" value="false">
                        {inActiveText}
                    </Select.Option>
                </Select>
                <div className="btngroup">
                    <Button type="primary" onClick={this.onSearch(search[key], setSelectedKeys, confirm)}>
                        Search
                    </Button>
                    <Button onClick={this.onReset(key, clearFilters)}>clean</Button>
                </div>
            </div>
        );
    };

    private RenderSelectColumnFilterDropdown = (
        key,
        items: string | any[],
        { setSelectedKeys, selectedKeys, confirm, clearFilters },
    ) => {
        let { search } = this.state;
        if (typeof items === "string") {
            return (
                <div style={{ padding: 8 }}>
                    <Select
                        getPopupContainer={this.getSelectPopupContainer}
                        style={{ width: "380px" }}
                        value={search && search[key] && search[key].value}
                        allowClear={false}
                        onChange={(value) => {
                            if (!search) {
                                search = {};
                            }
                            if (!search[key]) {
                                search[key] = {};
                            }
                            search[key].operator = "eq";
                            search[key].value = String(value);
                            this.setState({ ...this.state, search });
                        }}
                        url={items}
                    />
                    <div className="btngroup">
                        <Button type="primary" onClick={this.onSearch(search[key], setSelectedKeys, confirm)}>
                            Search
                        </Button>
                        <Button onClick={this.onReset(key, clearFilters)}>clean</Button>
                    </div>
                </div>
            );
        } else {
            return (
                <div style={{ padding: 8 }}>
                    <Select
                        getPopupContainer={this.getSelectPopupContainer}
                        style={{ width: "380px" }}
                        value={search && search[key] && search[key].value}
                        allowClear={false}
                        onChange={(value) => {
                            if (!search) {
                                search = {};
                            }
                            if (!search[key]) {
                                search[key] = {};
                            }
                            search[key].operator = "eq";
                            search[key].value = String(value);
                            this.setState({ ...this.state, search });
                        }}>
                        {items.map((x) => (
                            <Select.Option key={x.id} value={x.id}>
                                {x.title}
                            </Select.Option>
                        ))}
                    </Select>
                    <div className="btngroup">
                        <Button type="primary" onClick={this.onSearch(search[key], setSelectedKeys, confirm)}>
                            Search
                        </Button>
                        <Button onClick={this.onReset(key, clearFilters)}>clean</Button>
                    </div>
                </div>
            );
        }
    };

    private RenderDateColumnFilterDropdown = (key, { setSelectedKeys, selectedKeys, confirm, clearFilters }) => {
        let { search } = this.state;
        return (
            <div style={{ padding: 8 }}>
                <DatePicker
                    placeholder="Search value"
                    style={{ width: "380px" }}
                    value={search && search[key] && search[key].value}
                    onChange={(val) => {
                        if (!search) {
                            search = {};
                        }
                        if (!search[key]) {
                            search[key] = {};
                        }
                        search[key].value = val ? val : "";
                        this.setState({ ...this.state, search });
                    }}
                />
                <Select
                    getPopupContainer={this.getSelectPopupContainer}
                    style={{ width: "380px" }}
                    value={(search && search[key] && search[key].operator) ?? "eq"}
                    allowClear={false}
                    onChange={(value) => {
                        if (!search) {
                            search = {};
                        }
                        if (!search[key]) {
                            search[key] = {};
                        }
                        search[key].operator = String(value);
                        this.setState({ ...this.state, search });
                    }}>
                    {filterList.map((x) => (
                        <Select.Option key={x.value} value={x.value}>
                            {x.title}
                        </Select.Option>
                    ))}
                </Select>
                <div className="btngroup">
                    <Button type="primary" onClick={this.onSearch(search[key], setSelectedKeys, confirm)}>
                        Search
                    </Button>
                    <Button onClick={this.onReset(key, clearFilters)}>clean</Button>
                </div>
            </div>
        );
    };

    private RenderNumberColumnFilterDropdown = (key, { setSelectedKeys, selectedKeys, confirm, clearFilters }) => {
        let { search } = this.state;
        return (
            <div style={{ padding: 8 }}>
                <NumberInput
                    placeholder="Search value"
                    style={{ width: "380px" }}
                    value={search && search[key] && search[key].value}
                    onChange={(e) => {
                        if (!search) {
                            search = {};
                        }
                        if (!search[key]) {
                            search[key] = {};
                        }
                        search[key].value = e ?? "";
                        this.setState({ ...this.state, search });
                    }}
                />
                <Select
                    getPopupContainer={this.getSelectPopupContainer}
                    style={{ width: "380px" }}
                    value={(search && search[key] && search[key].operator) || "eq"}
                    allowClear={false}
                    onChange={(value) => {
                        if (!search) {
                            search = {};
                        }
                        if (!search[key]) {
                            search[key] = {};
                        }
                        search[key].operator = String(value);
                        this.setState({ ...this.state, search });
                    }}>
                    {filterList.map((x, i) => (
                        <Select.Option key={x.value} value={x.value}>
                            {x.title}
                        </Select.Option>
                    ))}
                </Select>
                <div className="btngroup">
                    <Button type="primary" onClick={this.onSearch(search[key], setSelectedKeys, confirm)}>
                        Search
                    </Button>
                    <Button onClick={this.onReset(key, clearFilters)}>clean</Button>
                </div>
            </div>
        );
    };

    private RenderColumnFilterDropdown = (key, { setSelectedKeys, selectedKeys, confirm, clearFilters }) => {
        let { search } = this.state;
        return (
            <div style={{ padding: 8 }}>
                <Input
                    placeholder="Search value"
                    style={{ width: "380px" }}
                    value={search && search[key] && search[key].value}
                    onChange={(e) => {
                        if (!search) {
                            search = {};
                        }
                        if (!search[key]) {
                            search[key] = {};
                        }
                        search[key].value = e.target.value ? e.target.value : "";
                        this.setState({ ...this.state, search });
                    }}
                />
                <Select
                    getPopupContainer={this.getSelectPopupContainer}
                    style={{ width: "380px" }}
                    value={(search && search[key] && search[key].operator) || "contains"}
                    allowClear={false}
                    onChange={(value) => {
                        if (!search) {
                            search = {};
                        }
                        if (!search[key]) {
                            search[key] = {};
                        }
                        search[key].operator = String(value);
                        this.setState({ ...this.state, search });
                    }}>
                    {stringFilterList.map((x, i) => (
                        <Select.Option key={x.value} value={x.value}>
                            {x.title}
                        </Select.Option>
                    ))}
                </Select>
                <div className="btngroup">
                    <Button type="primary" onClick={this.onSearch(search[key], setSelectedKeys, confirm)}>
                        Search
                    </Button>
                    <Button onClick={this.onReset(key, clearFilters)}>clean</Button>
                </div>
            </div>
        );
    };

    private getSelectPopupContainer = (target) => {
        return target.parentElement ? target.parentElement : document.body;
    };

    private renderColumn = (column) => {
        const { key } = column;
        const { hasFilter, ...rest } = column.props;
        if (column.type === IconColumn) {
            if (hasFilter) {
                rest.filterDropdown = this.RenderIconColumnFilterDropdown.bind(this, key, rest);
            }
            rest.render = (text: boolean | string, _) => {
                let value: boolean | string;
                if (typeof text === "boolean") {
                    value = text;
                } else {
                    value = text.toString();
                }
                if (value === column.props.activeValue) {
                    return column.props.activeIcon;
                } else if (value === column.props.inActiveValue) {
                    return column.props.inActiveIcon;
                }
                return column.props.noState;
            };
        } else if (column.type === IndexColumn) {
            rest.render = (text: string, record: any, index: number) => {
                const res = this.getPagination(this.props.dataSet);
                if (!res) {
                    return index + 1;
                } else {
                    const { currentPage } = this.state;

                    return ((currentPage || 1) - 1) * 10 + (index + 1);
                }
            };
        } else if (column.type === DateColumn) {
            if (hasFilter) {
                rest.filterDropdown = this.RenderDateColumnFilterDropdown.bind(this, key);
            }
            rest.render = (text, _) => {
                if (!text || text === "0001-01-01T00:00:00") {
                    return null;
                }
                const format = column.props.format;
                const r = new NDate(text);
                return <bdo dir="ltr">{r.format(format)}</bdo>;
            };
        } else if (column.type === FilterColumn) {
            rest.render = (text, _) => {
                if (!text) {
                    return null;
                }
                const filter = column.props.filter;
                for (const item of filter) {
                    if (item.key.toString().toLowerCase().trim() === text.toString().toLowerCase().trim()) {
                        return item.value;
                    }
                }
                return null;
            };
        } else if (column.type === PriceColumn) {
            rest.render = (text, _) => {
                if (text === undefined || text === null || text === "") {
                    return null;
                }
                if (!(typeof text === "string")) {
                    text = text.toString();
                }
                return PriceParser(text);
            };
        } else if (column.type === NumberColumn) {
            if (hasFilter) {
                rest.filterDropdown = this.RenderNumberColumnFilterDropdown.bind(this, key);
            }
        } else if (column.type === SelectColumn) {
            if (hasFilter) {
                rest.filterDropdown = this.RenderSelectColumnFilterDropdown.bind(this, key, rest.items);
            }
        } else {
            if (hasFilter) {
                rest.filterDropdown = this.RenderColumnFilterDropdown.bind(this, key);
            }
        }
        const { sorter } = this.state;
        if (sorter && sorter.columnKey === column.key) {
            rest.sortOrder = sorter.order;
        } else {
            rest.sortOrder = false;
        }
        return <AntColumn key={key} {...rest} />;
    };

    private GetColumn = (columns: any[]) => {
        return columns.map((x) => this.renderColumn(x));
    };
}

export default Table;
