import axios from "axios";
import { tokenAtom, store } from "../store";

export async function getAddress() {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.get("/Address", {
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

export async function createAddress(addressData) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.post("/Address", addressData, {
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

export async function updateAddress(id, addressData) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.put(`/Address/${id}`, addressData, {
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

export async function deleteAddress(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.delete(`/Address/${id}`, {
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
