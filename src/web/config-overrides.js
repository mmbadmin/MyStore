const { override, fixBabelImports, addLessLoader, addWebpackAlias } = require("customize-cra");

module.exports = override(
    fixBabelImports("import", {
        libraryName: "antd",
        libraryDirectory: "es",
        style: true,
    }),
    addWebpackAlias({
     
    }),
    addLessLoader({
        javascriptEnabled: true,
        modifyVars: {
            "@font-family": "OpenSans",
            "@border-radius-base": "0px",
            "@border-radius-sm": "0px",
            // "@layout-header-background": "#3c8dbc",
            "@input-border-color": "#797979",
            "@select-border-color": "#797979",
        },
    }),
);
