import { useAtom } from "jotai";
import { tokenAtom, roleAtom, cartCountAtom } from "../store";
import { Link } from "react-router";
import { useState, useEffect } from "react";

export default function Navbar() {
  const [token, setToken] = useAtom(tokenAtom);
  const [role] = useAtom(roleAtom);
  const [theme, setTheme] = useState(localStorage.getItem("theme") || "light");
  const [cartCount] = useAtom(cartCountAtom);

  useEffect(() => {
    if (theme === "dark") {
      document.documentElement.classList.add("dark");
    } else {
      document.documentElement.classList.remove("dark");
    }
    localStorage.setItem("theme", theme);
  }, [theme]);

  const toggleTheme = () => {
    setTheme(theme === "dark" ? "light" : "dark");
  };

  const handleLogout = () => {
    setToken(null);
  };

  return (
    <nav
      className="flex items-center justify-between px-10 py-5 border-b
            bg-white dark:bg-[#0A0A0A]
            border-beige-dark dark:border-[#1F1F1F]"
    >
      <Link
        to="/"
        className="font-serif text-[22px] tracking-[4px]
                    text-brown dark:text-rose-gold"
      >
        MELANIN
      </Link>
      <div className="flex items-center gap-8">
        <Link
          to="/"
          className="text-[13px] tracking-[1px] hover:opacity-70
                        text-brown-light dark:text-[#999]"
        >
          ACCUEIL
        </Link>
        <Link
          to="/shop"
          className="text-[13px] tracking-[1px] hover:opacity-70
                        text-brown-light dark:text-[#999]"
        >
          BOUTIQUE
        </Link>

        {/* Icône panier */}
        <Link to="/cart" className="relative hover:opacity-70">
          <span className="text-[20px] text-brown-light dark:text-[#999]">
            🛍
          </span>
          {cartCount > 0 && (
            <span
              className="absolute -top-2 -right-2 text-[10px] w-4 h-4 flex items-center justify-center rounded-full
                            bg-brown text-beige dark:bg-rose-gold dark:text-[#0A0A0A]"
            >
              {cartCount}
            </span>
          )}
        </Link>

        <button
          onClick={toggleTheme}
          className="text-[13px] cursor-pointer hover:opacity-70
                        text-brown-light dark:text-[#999]"
        >
          {theme === "dark" ? "☀" : "☾"}
        </button>
        {token ? (
          <>
            {role === "Admin" && (
              <Link
                to="/admin"
                className="text-[13px] tracking-[1px] hover:opacity-70
                                    text-brown-light dark:text-[#999]"
              >
                DASHBOARD
              </Link>
            )}
            <button
              onClick={handleLogout}
              className="text-[13px] tracking-[1px] cursor-pointer hover:opacity-70
                                text-brown-light dark:text-[#999]"
            >
              DÉCONNEXION
            </button>
          </>
        ) : (
          <>
            <Link
              to="/login"
              className="text-[13px] tracking-[1px] hover:opacity-70
                                text-brown-light dark:text-[#999]"
            >
              CONNEXION
            </Link>
            <Link
              to="/register"
              className="text-[13px] tracking-[1px] px-5 py-2 border
                                text-beige bg-brown border-brown hover:bg-brown-dark
                                dark:text-rose-gold dark:bg-transparent dark:border-rose-gold dark:hover:bg-rose-gold dark:hover:text-[#0A0A0A]"
            >
              INSCRIPTION
            </Link>
          </>
        )}
      </div>
    </nav>
  );
}
