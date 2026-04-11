import { useState, useEffect } from "react";
import { Link } from "react-router";
import { useAtom } from "jotai";
import { tokenAtom, cartCountAtom, getMemberIdFromToken } from "../store";
import {
  getCart,
  updateQuantity,
  removeFromCart,
  clearCart,
  getLocalCart,
  updateLocalCartQuantity,
  removeFromLocalCart,
  clearLocalCart,
} from "../services/cartItemService";

export default function Cart() {
  const [token] = useAtom(tokenAtom);
  const [cartItems, setCartItems] = useState([]);
  const [, setCartCount] = useAtom(cartCountAtom);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function loadCart() {
      setLoading(true);
      setError(null);

      if (token) {
        const memberId = getMemberIdFromToken(token);
        const result = await getCart(memberId);
        if (result.success) {
          setCartItems(result.data);
          // Initialiser le compteur depuis l'API
          const total = result.data.reduce(
            (acc, item) => acc + item.quantity,
            0,
          );
          setCartCount(total);
        } else {
          setError(result.error);
        }
      } else {
        const localCart = getLocalCart();
        setCartItems(localCart);
        // Initialiser le compteur depuis localStorage
        const total = localCart.reduce((acc, item) => acc + item.quantity, 0);
        setCartCount(total);
      }

      setLoading(false);
    }

    loadCart();
  }, [token]);

  async function handleUpdateQuantity(cartItemId, quantity) {
    if (quantity <= 0) return;

    if (token) {
      const result = await updateQuantity(cartItemId, quantity);
      if (!result.success) {
        setError(result.error);
        return;
      }
      setCartItems((prev) =>
        prev.map((item) =>
          item.id === cartItemId ? { ...item, quantity } : item,
        ),
      );
    } else {
      const updated = updateLocalCartQuantity(cartItemId, quantity);
      setCartItems(updated);
    }

    // Mettre à jour le compteur
    setCartCount(
      cartItems
        .map((item) => (item.id === cartItemId ? { ...item, quantity } : item))
        .reduce((acc, item) => acc + item.quantity, 0),
    );
  }

  async function handleRemove(cartItemId) {
    const itemToRemove = cartItems.find((item) => item.id === cartItemId);

    if (token) {
      const result = await removeFromCart(cartItemId);
      if (!result.success) {
        setError(result.error);
        return;
      }
    } else {
      removeFromLocalCart(cartItemId);
    }

    setCartItems((prev) => prev.filter((item) => item.id !== cartItemId));
    setCartCount((prev) => prev - itemToRemove.quantity);
  }

  async function handleClear() {
    if (token) {
      const memberId = getMemberIdFromToken(token);
      const result = await clearCart(memberId);
      if (!result.success) {
        setError(result.error);
        return;
      }
    } else {
      clearLocalCart();
    }

    setCartItems([]);
    setCartCount(0);
  }

  const total = cartItems.reduce(
    (total, item) => total + item.unitPrice * item.quantity,
    0,
  );

  if (loading) {
    return (
      <div className="flex justify-center items-center py-20">
        <p className="text-brown/60 dark:text-rose-gold/60 tracking-widest uppercase text-sm">
          Chargement...
        </p>
      </div>
    );
  }

  return (
    <section className="max-w-4xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-serif tracking-widest uppercase text-brown dark:text-rose-gold text-center mb-10">
        Mon Panier
      </h1>

      {error && (
        <p className="text-center text-red-500 text-sm mb-6">{error}</p>
      )}

      {cartItems.length === 0 ? (
        <div className="flex flex-col items-center gap-6 py-20">
          <p className="text-brown/60 dark:text-rose-gold/60 text-sm tracking-widest uppercase">
            Votre panier est vide
          </p>
          <Link
            to="/shop"
            className="text-sm tracking-widest uppercase px-6 py-3 bg-brown dark:bg-rose-gold text-white dark:text-black hover:opacity-90 transition-opacity"
          >
            Découvrir la boutique
          </Link>
        </div>
      ) : (
        <div className="space-y-6">
          {cartItems.map((item) => (
            <div
              key={item.id}
              className="flex gap-6 border-b border-beige-dark dark:border-zinc-700 pb-6"
            >
              {/* Image */}
              <div className="w-24 h-24 shrink-0 bg-beige dark:bg-zinc-800 rounded flex items-center justify-center overflow-hidden">
                {item.productImageUrl ? (
                  <img
                    src={`${import.meta.env.VITE_API_URL.replace("/api", "")}${item.productImageUrl}`}
                    alt={item.productName}
                    className="w-full h-full object-cover"
                  />
                ) : (
                  <span className="text-brown/40 dark:text-rose-gold/40 text-xs">
                    Photo
                  </span>
                )}
              </div>

              {/* Infos */}
              <div className="flex-1 space-y-2">
                <p className="font-serif text-brown dark:text-rose-gold tracking-wide">
                  {item.productName}
                </p>

                {/* Caractéristiques */}
                <div className="flex flex-wrap gap-3 text-xs text-brown/50 dark:text-rose-gold/50">
                  {item.hairColor && <span>Couleur : {item.hairColor}</span>}
                  {item.hairLength && <span>Longueur : {item.hairLength}</span>}
                  {item.hairTexture && (
                    <span>Texture : {item.hairTexture}</span>
                  )}
                  {item.capSize && <span>Bonnet : {item.capSize}</span>}
                </div>

                {/* Prix unitaire */}
                <p className="text-sm text-gold dark:text-gold-light">
                  {item.unitPrice.toFixed(2)} € / unité
                </p>

                {/* Quantité + supprimer */}
                <div className="flex items-center gap-4">
                  <div className="flex items-center border border-beige-dark dark:border-zinc-700">
                    <button
                      onClick={() =>
                        handleUpdateQuantity(item.id, item.quantity - 1)
                      }
                      className="px-3 py-1 text-brown dark:text-rose-gold hover:opacity-70"
                    >
                      −
                    </button>
                    <span className="px-3 py-1 text-sm text-brown dark:text-rose-gold">
                      {item.quantity}
                    </span>
                    <button
                      onClick={() =>
                        handleUpdateQuantity(item.id, item.quantity + 1)
                      }
                      className="px-3 py-1 text-brown dark:text-rose-gold hover:opacity-70"
                    >
                      +
                    </button>
                  </div>
                  <button
                    onClick={() => handleRemove(item.id)}
                    className="text-xs text-red-400 hover:text-red-600 transition-colors"
                  >
                    Supprimer
                  </button>
                </div>
              </div>

              {/* Sous-total */}
              <div className="shrink-0 text-right">
                <p className="font-semibold text-brown dark:text-rose-gold">
                  {(item.unitPrice * item.quantity).toFixed(2)} €
                </p>
              </div>
            </div>
          ))}

          {/* Total + actions */}
          <div className="flex flex-col items-end gap-4 pt-4">
            <button
              onClick={handleClear}
              className="text-xs text-red-400 hover:text-red-600 transition-colors"
            >
              Vider le panier
            </button>
            <div className="flex items-center gap-4">
              <p className="text-lg font-serif tracking-wide text-brown dark:text-rose-gold">
                Total : {total.toFixed(2)} €
              </p>
              <button
                disabled
                className="px-6 py-3 text-xs tracking-widest uppercase bg-brown dark:bg-rose-gold text-white dark:text-black hover:opacity-90 transition-opacity disabled:opacity-40 disabled:cursor-not-allowed"
              >
                Commander
              </button>
            </div>
          </div>
        </div>
      )}
    </section>
  );
}
