import { Modal } from "antd";
import React from "react";

export class MBox {
    public static success(
        content: string | React.ReactNode,
        title: string | React.ReactNode = "Success",
        onOk?: (...args: any[]) => any | PromiseLike<any>,
    ) {
        Modal.success({
            content: MBox.getElement(content),
            onOk,
            title: MBox.getElement(title),
        });
    }

    public static error(
        content: string | React.ReactNode,
        title: string | React.ReactNode = "Error",
        onOk?: (...args: any[]) => any | PromiseLike<any>,
    ) {
        Modal.error({
            content: MBox.getElement(content),
            onOk,
            title: MBox.getElement(title),
        });
    }

    public static info(
        content: string | React.ReactNode,
        title: string | React.ReactNode = "Informatiom",
        onOk?: (...args: any[]) => any | PromiseLike<any>,
    ) {
        Modal.info({
            content: MBox.getElement(content),
            onOk,
            title: MBox.getElement(title),
        });
    }

    public static confirm(
        content: string | React.ReactNode,
        title: string | React.ReactNode = "Confirm",
        onOk?: (...args: any[]) => any | PromiseLike<any>,
        iconType?: string,
    ) {
        Modal.confirm({
            content: MBox.getElement(content),
            onOk,
            title: MBox.getElement(title),
            iconType,
        });
    }

    private static getElement = (item: string | React.ReactNode) => {
        if (typeof item === "string") {
            return MBox.getMessage(item);
        } else {
            return item;
        }
    };

    private static getMessage = (item: string) => {
        const messages = item.split("\n");
        if (messages.length === 0) {
            return null;
        }
        if (messages.length === 1) {
            return <div>{messages[0]}</div>;
        } else {
            return (
                <div>
                    {messages.map((x, i) => (
                        <p key={i}>{x}</p>
                    ))}
                </div>
            );
        }
    };
}
