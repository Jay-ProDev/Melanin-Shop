import apiClient from "./apiClient";
import { tokenAtom, store } from "../store";

// ========== CLIENT ==========

export async function createOrder(
  shippingAddressId,
  billingAddressId = null,
  couponCode = null,
) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.post(
      "/Order",
      { shippingAddressId, billingAddressId, couponCode },
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

export async function getMyOrders() {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.get("/Order/my-orders", {
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

export async function getMyOrder(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.get(`/Order/my-orders/${id}`, {
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

export async function cancelMyOrder(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.put(`/Order/my-orders/${id}/cancel`, null, {
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

// ========== ADMIN ==========

export async function getAllOrders() {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.get("/Order/all", {
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

export async function getOrdersByStatus(status) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.get(`/Order/status/${status}`, {
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

export async function getOrderById(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.get(`/Order/${id}`, {
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

export async function shipOrder(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.put(`/Order/${id}/ship`, null, {
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

export async function deliverOrder(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.put(`/Order/${id}/deliver`, null, {
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

export async function adminCancelOrder(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.put(`/Order/${id}/admin-cancel`, null, {
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
