import React from "react";
import { Spin, Input } from "antd";
import Fetch from "../../helpers/fetch";
import URL from "../../helpers/url";

class BICTCaptcah extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            id: "",
            image: {},
            loading: false,
            expire: false,
        };
    }

    UNSAFE_componentWillMount() {
        this.loadCaptcha();
    }

    UNSAFE_componentWillReceiveProps(nextProps) {
        if (nextProps.reload === true) {
            this.loadCaptcha();
            if (this.props.afterReload) {
                this.props.afterReload();
            }
        }
    }

    setExpire = () => {
        setTimeout(() => {
            this.setState({ expire: true });
        }, 1000 * 60 * 10);
    };

    loadCaptcha = async () => {
        this.setState({ loading: true });

        var result = await Fetch().url(URL.User.GetCaptcha).get().json();
        const state = {
            loading: false,
            image: result.file,
            id: result.id,
            expire: false,
        };
        this.setState(state);
        this.setExpire();
    };

    onClick = (event) => {
        event.preventDefault();
        this.loadCaptcha();
    };

    onChange = (event) => {
        const value = event.target.value;
        if (this.props.onChange) {
            this.props.onChange(this.state.id + "|" + value);
        }
    };

    render() {
        const { loading, image, expire } = this.state;
        const { placeholder } = this.props;

        const captchaImage = (
            <img
                className="captcha"
                src={`data:image/png;base64,${image}`}
                alt=""
                style={{ border: "1px solid #9a9a9a;" }}
            />
        );
        return (
            <Spin spinning={loading} style={{ textAlign: "center" }}>
                {expire ? (
                    <div className="captcha-error" onClick={this.onClick}>
                        <div className="captcha-message">
                        Your Captcha code has expired
                            <br />
                            Click to get the new Captcha code
                        </div>
                        {captchaImage}
                    </div>
                ) : (
                    captchaImage
                )}
                <Input placeholder={placeholder} onChange={this.onChange} disabled={expire} maxLength={4} />
            </Spin>
        );
    }
}

export default BICTCaptcah;
