import { Link } from "react-router";

export default function NotFound() {
  return (
    <div className="flex flex-col items-center justify-center text-center py-40 px-8">
      <p
        className="text-[11px] tracking-[6px] mb-5
                text-gold dark:text-rose-gold"
      >
        ERREUR 404
      </p>
      <h1
        className="font-serif text-[42px] font-normal
                text-brown-dark dark:text-white"
      >
        Page introuvable
      </h1>
      <p
        className="text-[14px] mt-4 max-w-[380px] leading-relaxed
                text-brown-light dark:text-[#777]"
      >
        La page que vous cherchez n'existe pas ou a été déplacée.
      </p>
      <Link
        to="/"
        className="mt-7 text-[12px] tracking-[2px] font-medium px-8 py-3
                    text-beige bg-brown hover:bg-brown-dark
                    dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark"
      >
        RETOUR À L'ACCUEIL
      </Link>
    </div>
  );
}
