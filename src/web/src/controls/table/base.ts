import { ColumnProps as AntColumnProps } from "antd/lib/table";

export interface IBaseColumnProps<T> extends AntColumnProps<T> {
    hasFilter?: boolean;
}
