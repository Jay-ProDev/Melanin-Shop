import axios from "axios";
import { tokenAtom, store } from "../store";

// ========== PUBLIC ==========

export async function getAllCategories() {
  let response;
  try {
    response = await axios.get("/Category", {
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

export async function getCategoryById(id) {
  let response;
  try {
    response = await axios.get(`/Category/${id}`, {
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

export async function getCategoryBySlug(slug) {
  let response;
  try {
    response = await axios.get(`/Category/slug/${slug}`, {
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

// ========== ADMIN ==========

export async function createCategory(name, slug) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.post(
      "/Category",
      { name, slug },
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

export async function updateCategory(id, name, slug) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.put(
      `/Category/${id}`,
      { name, slug },
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

export async function deleteCategory(id) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response = await axios.delete(`/Category/${id}`, {
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
