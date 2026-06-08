import apiClient from "./apiClient";
import { tokenAtom, store } from "../store";

// ========== CONNECTÉ ==========

export async function getCart(memberId) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.get(`/CartItem/${memberId}`, {
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

export async function addToCart(memberId, productId, quantity) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.post(
      `/CartItem/${memberId}`,
      { productId, quantity },
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

export async function updateQuantity(cartItemId, quantity) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.put(
      `/CartItem/${cartItemId}`,
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

export async function removeFromCart(cartItemId) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.delete(`/CartItem/${cartItemId}`, {
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

export async function clearCart(memberId) {
  let response;
  try {
    const token = store.get(tokenAtom);
    response =  await apiClient.delete(`/CartItem/clear/${memberId}`, {
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

// ========== INVITÉ (localStorage) ==========

export function getLocalCart() {
  return JSON.parse(localStorage.getItem("cart") || "[]");
}

export function addToLocalCart(product, quantity) {
  const localCart = getLocalCart();
  const existingItem = localCart.find((item) => item.productId === product.id);
  if (existingItem) {
    existingItem.quantity += quantity;
  } else {
    localCart.push({
      id: product.id,
      productId: product.id,
      productName: product.name,
      productImageUrl: product.imageUrl,
      unitPrice: product.unitPrice,
      quantity,
      hairColor: product.hairColor,
      hairLength: product.hairLength,
      hairTexture: product.hairTexture,
      capSize: product.capSize,
    });
  }
  localStorage.setItem("cart", JSON.stringify(localCart));
}

export function updateLocalCartQuantity(itemId, quantity) {
  const localCart = getLocalCart();
  const updated = localCart.map((item) =>
    item.id === itemId ? { ...item, quantity } : item,
  );
  localStorage.setItem("cart", JSON.stringify(updated));
  return updated;
}

export function removeFromLocalCart(itemId) {
  const localCart = getLocalCart();
  const updated = localCart.filter((item) => item.id !== itemId);
  localStorage.setItem("cart", JSON.stringify(updated));
  return updated;
}

export function clearLocalCart() {
  localStorage.removeItem("cart");
}
