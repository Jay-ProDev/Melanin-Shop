import axios from "axios";
import { tokenAtom, store } from "../store";
import apiClient from "./apiClient";

export async function authRegister(firstName, lastName, email, password) {
  let response;
  try {
    response = await axios.post(
      "/Member/register",
      { firstName, lastName, email, password },
      { baseURL: import.meta.env.VITE_API_URL },
    );
  } catch (error) {
    return {
      success: false,
      error: error.response?.data?.message ?? error.message,
    };
  }
  return {
    success: true,
    data: response.data,
  };
}

export async function authLogin(email, password) {
  let response;
  try {
    response = await axios.post(
      "/Member/login",
      { email, password },
      { baseURL: import.meta.env.VITE_API_URL },
    );
  } catch (error) {
    return {
      success: false,
      error: error.response?.data?.message ?? error.message,
    };
  }
  return {
    success: true,
    data: response.data,
  };
}

export async function getMe() {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await apiClient.get("/Member/me", {
      baseURL: import.meta.env.VITE_API_URL,
      headers: { Authorization: `Bearer ${token}` },
    });
  } catch (error) {
    return {
      success: false,
      error: error.response?.data?.message ?? error.message,
    };
  }
  return {
    success: true,
    data: response.data,
  };
}

export async function getMemberById(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.get(`/Member/${id}`, {
      baseURL: import.meta.env.VITE_API_URL,
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  } catch (error) {
    return {
      success: false,
      error: error.response?.data?.message ?? error.message,
    };
  }
  return {
    success: true,
    data: response.data,
  };
}

export async function updateMember(id, firstName, lastName, email) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.put(
      `/Member/${id}`,
      { firstName, lastName, email },
      {
        baseURL: import.meta.env.VITE_API_URL,
        headers: { Authorization: `Bearer ${token}` },
      },
    );
  } catch (error) {
    return {
      success: false,
      error: error.response?.data?.message ?? error.message,
    };
  }
  return {
    success: true,
    data: response.data,
  };
}
