import { connectRouter, routerMiddleware } from "connected-react-router";
import { History } from "history";
import { applyMiddleware, combineReducers, compose, createStore } from "redux";
import thunk from "redux-thunk";
import { reducers } from "./reducer";

export default function configureStore(history: History, initialState: any) {
    const middleware = [thunk, routerMiddleware(history)];

    const enhancers: any[] = [];
    const isDevelopment = process.env.NODE_ENV === "development";

    if (
        isDevelopment &&
        typeof window !== "undefined" &&
        (window as any).__REDUX_DEVTOOLS_EXTENSION__ &&
        (window as any).__REDUX_DEVTOOLS_EXTENSION__()
    ) {
        enhancers.push((window as any).__REDUX_DEVTOOLS_EXTENSION__());
    }

    const rootReducer = combineReducers({ ...reducers, router: connectRouter(history) });

    return createStore(rootReducer, initialState, compose(applyMiddleware(...middleware), ...enhancers));
}
