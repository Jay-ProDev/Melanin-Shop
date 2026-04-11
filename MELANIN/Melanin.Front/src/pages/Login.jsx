import { useAtom } from "jotai";
import { tokenAtom, getMemberIdFromToken } from "../store";
import { authLogin } from "../services/memberService";
import { Navigate } from "react-router";
import { useState } from "react";
import {
  addToCart,
  clearLocalCart,
  getLocalCart,
} from "../services/cartItemService";

export default function Login() {
  const [token, setToken] = useAtom(tokenAtom);
  const [error, setError] = useState("");

  // Remplacer le loginAction
  const loginAction = async (formData) => {
    setError("");
    const result = await authLogin(
      formData.get("email"),
      formData.get("password"),
    );
    if (result.success) {
      // 1. Récupérer le panier localStorage avant de sauvegarder le token
      const localCart = getLocalCart();

      // 2. Sauvegarder le token
      setToken(result.data.token);

      // 3. Fusionner le localStorage avec l'API si articles présents
      if (localCart.length > 0) {
        const memberId = getMemberIdFromToken(result.data.token);

        for (const item of localCart) {
          await addToCart(memberId, item.productId, item.quantity);
        }

        // 4. Vider le lcalStorage
        clearLocalCart();
      }
    } else {
      setError(result.error);
    }
  };

  if (token) {
    return <Navigate to="/" replace />;
  }

  return (
    <div className="flex flex-col items-center justify-center py-28 px-8">
      <h1
        className="font-serif text-[32px] mb-2
                text-brown-dark dark:text-white"
      >
        Connexion
      </h1>
      <p
        className="text-[13px] mb-8
                text-brown-light dark:text-[#777]"
      >
        Accédez à votre espace Melanin
      </p>

      {error && <p className="text-[13px] mb-4 text-red-500">{error}</p>}

      <form
        action={loginAction}
        className="flex flex-col gap-4 w-full max-w-[360px]"
      >
        <div className="flex flex-col gap-1">
          <label
            htmlFor="auth-email"
            className="text-[11px] tracking-[2px]
                            text-brown-light dark:text-[#999]"
          >
            EMAIL
          </label>
          <input
            id="auth-email"
            type="email"
            name="email"
            className="px-4 py-3 text-[14px] border outline-none
                            bg-white border-beige-dark text-brown-dark
                            focus:border-gold
                            dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white
                            dark:focus:border-rose-gold"
          />
        </div>
        <div className="flex flex-col gap-1">
          <label
            htmlFor="auth-pwd"
            className="text-[11px] tracking-[2px]
                            text-brown-light dark:text-[#999]"
          >
            MOT DE PASSE
          </label>
          <input
            id="auth-pwd"
            type="password"
            name="password"
            className="px-4 py-3 text-[14px] border outline-none
                            bg-white border-beige-dark text-brown-dark
                            focus:border-gold
                            dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white
                            dark:focus:border-rose-gold"
          />
        </div>
        <button
          type="submit"
          className="mt-2 py-3 text-[12px] tracking-[2px]  cursor-pointer font-medium
                        text-beige bg-brown hover:bg-brown-dark
                        dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark"
        >
          SE CONNECTER
        </button>
      </form>
    </div>
  );
}
