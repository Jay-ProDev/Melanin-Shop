import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router";
import { Provider } from "jotai";
import { store } from "./store";
// import { AuthProvider } from "./context/AuthContext";
import App from "./App";
import "./index.css";

createRoot(document.getElementById("root")).render(
    <StrictMode>
        <BrowserRouter>
            {/* <AuthProvider> */}
            <Provider store={store}>
                <App />
            </Provider>
            {/* </AuthProvider> */}
        </BrowserRouter>
    </StrictMode>
);