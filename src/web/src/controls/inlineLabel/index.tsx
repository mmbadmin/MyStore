import React from "react";

interface IProps {
    title: string;
    value: string;
}

export const InlineLabel = ({ title, value }: IProps) => (
    <div className="inline-label">
        <span className="inline-label-title">{title}</span>
        <span>: </span>
        <span className="inline-label-value">{value}</span>
    </div>
);
