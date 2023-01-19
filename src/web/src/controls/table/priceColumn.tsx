import React from "react";
import { IBaseColumnProps } from "./base";

class WrapColumn<T> extends React.Component<IBaseColumnProps<T>, React.ComponentState> {}

export default WrapColumn;
