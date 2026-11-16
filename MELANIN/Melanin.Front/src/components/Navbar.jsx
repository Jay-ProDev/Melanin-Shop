import { useAtom } from "jotai";
import { tokenAtom, roleAtom, cartCountAtom } from "../store";
import { Link } from "react-router";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router";

export default function Navbar() {
  const [token, setToken] = useAtom(tokenAtom);
  const [role] = useAtom(roleAtom);
  const [theme, setTheme] = useState(localStorage.getItem("theme") || "light");
  const [cartCount] = useAtom(cartCountAtom);
  const [menuOpen, setMenuOpen] = useState(false);
  const navigate = useNavigate();

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

  // Ferme le menu téléphone après un clic sur un lien
  const closeMenu = () => setMenuOpen(false);

  const handleLogout = () => {
    closeMenu();
    navigate("/");
    // Délai d'1 frame pour laisser React-router finir la navigation
    // avant que RequireAuth ne réagisse au token null
    setTimeout(() => setToken(null), 16);
  };

  // Classes communes aux liens texte
  const linkClass =
    "text-[13px] tracking-[1px] hover:opacity-70 text-brown-light dark:text-[#999]";

  // Liens affichés à 2 endroits : la barre (ordinateur) et le menu (téléphone)
  const mainLinks = (
    <>
      <Link to="/" onClick={closeMenu} className={linkClass}>
        ACCUEIL
      </Link>
      <Link to="/shop" onClick={closeMenu} className={linkClass}>
        BOUTIQUE
      </Link>
    </>
  );

  const accountLinks = token ? (
    <>
      {role === "Admin" && (
        <Link to="/admin" onClick={closeMenu} className={linkClass}>
          DASHBOARD
        </Link>
      )}

      <Link to="/profile" onClick={closeMenu} className={linkClass}>
        PROFIL
      </Link>

      <button onClick={handleLogout} className={linkClass}>
        DÉCONNEXION 
      </button>
    </>
  ) : (
    <>
      <Link to="/login" onClick={closeMenu} className={linkClass}>
        CONNEXION
      </Link>

      <Link
        to="/register"
        onClick={closeMenu}
        className="text-[13px] tracking-[1px] px-5 py-2 border
                  text-beige bg-brown border-brown hover:bg-brown-dark
                  dark:text-rose-gold dark:bg-transparent dark:border-rose-gold dark:hover:bg-rose-gold dark:hover:text-[#0A0A0A]"
      >
        INSCRIPTION
      </Link>
    </>
  );

  return (
    <nav
      className="sticky top-0 z-50 border-b
            bg-white dark:bg-[#0A0A0A]
            border-beige-dark dark:border-[#1F1F1F]"
    >
      {/* Barre du haut : logo à gauche, liens et icônes à droite */}
      <div className="flex items-center justify-between px-4 lg:px-10 py-3">
        <Link
          to="/"
          onClick={closeMenu}
          className="flex items-center gap-3
                    font-serif text-[22px] tracking-[4px]
                    text-brown dark:text-rose-gold"
        >
          <img
            src="/melanin-favicon.svg"
            alt=""
            className="w-8 h-8 object-cover rounded-lg"
          />
          MELANIN
        </Link>

        <div className="flex items-center gap-5 lg:gap-8">
          {/* Cachés sur téléphone, affichés à partir de lg */}
          <div className="hidden lg:flex items-center gap-8">{mainLinks}</div>

          {/* Icône panier */}
          <Link to="/cart" onClick={closeMenu} className="relative hover:opacity-70">
            <span className="text-[20px]">🛒</span>
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
            aria-label={theme === "dark" ? "Passer en mode clair" : "Passer en mode sombre"}
            title={theme === "dark" ? "Mode clair" : "Mode sombre"}
            className="text-[16px] hover:opacity-70"
          >
            {theme === "dark" ? "☀️" : "🌚"}
          </button>

          <div className="hidden lg:flex items-center gap-8">{accountLinks}</div>

          {/* Bouton burger : seulement sous lg */}
          <button
            onClick={() => setMenuOpen(!menuOpen)}
            aria-label={menuOpen ? "Fermer le menu" : "Ouvrir le menu"}
            aria-expanded={menuOpen}
            className="lg:hidden text-[22px] leading-none hover:opacity-70
                      text-brown dark:text-rose-gold"
          >
            {menuOpen ? "✕" : "☰"}
          </button>
        </div>
      </div>

      {/* Menu téléphone : les liens les uns sous les autres */}
      {menuOpen && (
        <div className="lg:hidden flex flex-col items-start gap-5 px-4 pb-6">
          {mainLinks}
          {accountLinks}
        </div>
      )}
    </nav>
  );
}