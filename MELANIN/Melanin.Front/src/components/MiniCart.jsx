import { useEffect, useState } from "react";
import { Link } from "react-router";
import { useAtom } from "jotai";
import { tokenAtom, cartCountAtom, getMemberIdFromToken } from "../store";
import { getCart, getLocalCart } from "../services/cartItemService";

// Pourquoi : résumé du panier affiché à côté des produits,
// pour voir ce qu'on a choisi sans quitter la boutique.
export default function MiniCart() {
  const [token] = useAtom(tokenAtom);
  const [cartCount] = useAtom(cartCountAtom);
  const [items, setItems] = useState([]);

  // Recharge le panier à chaque changement du compteur (ajout, suppression, connexion).
  // Connecté : panier en base (API). Non connecté : panier dans le navigateur (localStorage).
  useEffect(() => {
    let ignore = false;

    async function loadCart() {
      const result = token
        ? await getCart(getMemberIdFromToken(token))
        : { success: true, data: getLocalCart() };
      if (!ignore && result.success) setItems(result.data);
    }

    loadCart();
    return () => {
      ignore = true;
    };
  }, [token, cartCount]);

  const total = items.reduce(
    (sum, item) => sum + item.unitPrice * item.quantity,
    0,
  );

  return (
    <div className="border border-beige-dark dark:border-zinc-700 rounded-lg p-3">
      <h2 className="font-serif text-sm tracking-widest uppercase text-brown dark:text-rose-gold mb-4">
        Mon panier
      </h2>

      {items.length === 0 ? (
        <p className="text-xs text-brown/50 dark:text-rose-gold/50">
          Votre panier est vide
        </p>
      ) : (
        <>
          <ul className="space-y-3 max-h-80 overflow-y-auto pr-1">
            {items.map((item) => (
              <li key={item.id} className="text-xs">
                <p className="text-brown dark:text-rose-gold truncate">
                  {item.productName}
                </p>
                <div className="flex justify-between text-brown/60 dark:text-rose-gold/60">
                  <span>× {item.quantity}</span>
                  <span>{(item.unitPrice * item.quantity).toFixed(2)} €</span>
                </div>
              </li>
            ))}
          </ul>

          <div className="flex justify-between mt-4 pt-4 border-t border-beige-dark dark:border-zinc-700 text-sm font-semibold text-brown dark:text-rose-gold">
            <span>Total</span>
            <span>{total.toFixed(2)} €</span>
          </div>

          <Link
            to="/cart"
            className="block mt-2 text-center px-4 py-2 text-xs tracking-widest uppercase rounded
                       bg-brown text-white dark:bg-rose-gold dark:text-black
                       hover:opacity-90 transition-opacity"
          >
            Voir le panier
          </Link>
        </>
      )}
    </div>
  );
}