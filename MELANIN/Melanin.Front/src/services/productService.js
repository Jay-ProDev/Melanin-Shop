import axios from "axios";
import { tokenAtom, store } from "../store";

// ========== PUBLIC ==========

export async function getAllActiveProducts() {
  let response;
  try {
    response = await axios.get("/Product/active", {
      baseURL: import.meta.env.VITE_API_URL,
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

export async function getProductById(id) {
  let response;
  try {
    response = await axios.get(`/Product/${id}`, {
      baseURL: import.meta.env.VITE_API_URL,
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

export async function getProductsByCategory(categoryId) {
  let response;
  try {
    response = await axios.get(`/Product/category/${categoryId}`, {
      baseURL: import.meta.env.VITE_API_URL,
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

export async function searchProducts(keyword) {
  let response;
  try {
    response = await axios.get("/Product/search", {
      baseURL: import.meta.env.VITE_API_URL,
      params: { keyword },
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

export async function getAllProducts() {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.get("/Product", {
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

export async function createProduct(productData) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.post("/Product", productData, {
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

export async function updateProduct(id, productData) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.put(`/Product/${id}`, productData, {
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

export async function activateProduct(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.put(`/Product/${id}/activate`, null, {
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

export async function deactivateProduct(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.put(`/Product/${id}/deactivate`, null, {
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

export async function increaseStock(id, quantity) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.put(
      `/Product/${id}/stock/increase`,
      { quantity },
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

export async function decreaseStock(id, quantity) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.put(
      `/Product/${id}/stock/decrease`,
      { quantity },
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

export async function deleteProduct(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.delete(`/Product/${id}`, {
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
