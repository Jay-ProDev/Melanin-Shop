import { Link } from "react-router";
import { useAtom } from "jotai";
import { tokenAtom, cartCountAtom, getMemberIdFromToken } from "../store";
import { addToCart, addToLocalCart } from "../services/cartItemService";
import { useState } from "react";

export default function ProductCard({ product }) {
  const [token] = useAtom(tokenAtom);
  const [, setCartCount] = useAtom(cartCountAtom);
  const [cartMessage, setCartMessage] = useState(null);

  async function handleAddToCart(e) {
    e.preventDefault();

    if (token) {
      const memberId = getMemberIdFromToken(token);
      const result = await addToCart(memberId, product.id, 1);
      if (result.success) {
        setCartMessage("✓ ajouté");
        setCartCount((prev) => prev + 1);
      } else {
        setCartMessage("! Erreur, Réessayez");
      }
    } else {
      addToLocalCart(product, 1);
      setCartMessage("✓");
      setCartCount((prev) => prev + 1);
    }

    setTimeout(() => setCartMessage(null), 2000);
  }

  return (
    <Link
      to={`/product/${product.id}`}
      className="group block bg-white dark:bg-zinc-900 border border-beige-dark dark:border-zinc-800 rounded-lg overflow-hidden hover:shadow-lg transition-shadow"
    >
      {/* Image */}
      <div className="aspect-square bg-beige dark:bg-zinc-800 flex items-center justify-center overflow-hidden">
        {product.imageUrl ? (
          <img
            src={`${import.meta.env.VITE_API_URL.replace("/api", "")}${product.imageUrl}`}
            alt={product.name}
            className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
          />
        ) : (
          <span className="text-brown/40 dark:text-rose-gold/40 text-sm tracking-widest uppercase">
            Photo
          </span>
        )}
      </div>

      {/* Infos */}
      <div className="p-4 space-y-2">
        <h3 className="text-brown dark:text-rose-gold font-serif text-sm tracking-wide uppercase truncate">
          {product.name}
        </h3>

        <p className="text-gold dark:text-gold-light font-semibold">
          {product.unitPrice.toFixed(2)} €
        </p>

        <button
          onClick={handleAddToCart}
          disabled={product.stockQuantity === 0}
          className="w-full py-2 text-xs tracking-widest uppercase bg-brown dark:bg-rose-gold text-white dark:text-black rounded hover:opacity-90 transition-opacity disabled:opacity-40 disabled:cursor-not-allowed"
        >
          {cartMessage === "✓"
            ? "✓ Ajouté !"
            : cartMessage === "!"
              ? "Erreur"
              : product.stockQuantity === 0
                ? "Rupture de stock"
                : "Ajouter au panier"}
        </button>
      </div>
    </Link>
  );
}
