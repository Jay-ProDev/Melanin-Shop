import axios from "axios";
import { tokenAtom, store } from "../store";

const apiClient = axios.create();

apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            store.set(tokenAtom, null);
            window.location.href = "/login";
        }
        return Promise.reject(error);
    }
);

export default apiClient;