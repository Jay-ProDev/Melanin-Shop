import axios from "axios";
import { tokenAtom, store } from "../store";

export async function createPaymentSession(orderId) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.post(`/Payment/checkout-session/${orderId}`, null, {
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