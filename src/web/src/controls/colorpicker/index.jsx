import React from "react";
import RcColorPicker from "rc-color-picker";
import "rc-color-picker/assets/index.css";

class ColorPicker extends React.Component {
    onChange = (colors) => {
        if (this.props.onChange) {
            this.props.onChange(colors.color);
        }
    };

    render() {
        return (
            <RcColorPicker
                animation="slide-up"
                onChange={this.onChange}
                defaultColor={"#fff"}
                color={this.props.value}
            />
        );
    }
}

export default ColorPicker;
