import React from "react";
import ReactDOM from "react-dom";
import { createBrowserHistory, History } from "history";
import { Provider } from "react-redux";
import { ConnectedRouter } from "connected-react-router";
import configureStore from "./store/configureStore";
import App from "./app";
import "./styles/index.less";
import * as serviceWorker from "./serviceWorker";

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
const history: History = createBrowserHistory({ basename: baseUrl || "" });

const initialState = (window as any).initialReduxState;
const store = configureStore(history, initialState);

const rootElement = document.getElementById("approot");

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <App />
        </ConnectedRouter>
    </Provider>,
    rootElement,
);

serviceWorker.unregister();
