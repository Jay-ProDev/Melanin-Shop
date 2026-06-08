import { Link } from "react-router";

export default function Footer() {
  return (
    <footer
      className="border-t mt-auto
            bg-white border-beige-dark
            dark:bg-[#050505] dark:border-[#1F1F1F]"
    >
      {/* Barre de réassurance */}
      <div
        className="grid grid-cols-4 border-b
                border-beige-dark dark:border-[#1F1F1F]"
      >
        <div
          className="flex flex-col items-center py-6 border-r
                    border-beige-dark dark:border-[#1F1F1F]"
        >
          <p
            className="text-[11px] tracking-[2px] font-medium
                        text-brown-dark dark:text-white"
          >
            LIVRAISON OFFERTE
          </p>
          <p
            className="text-[12px] mt-1
                        text-brown-light dark:text-[#666]"
          >
            En Belgique dès 50€
          </p>
        </div>
        <div
          className="flex flex-col items-center py-6 border-r
                    border-beige-dark dark:border-[#1F1F1F]"
        >
          <p
            className="text-[11px] tracking-[2px] font-medium
                        text-brown-dark dark:text-white"
          >
            PAIEMENT SÉCURISÉ
          </p>
          <p
            className="text-[12px] mt-1
                        text-brown-light dark:text-[#666]"
          >
            Visa, Mastercard, Stripe
          </p>
        </div>
        <div
          className="flex flex-col items-center py-6 border-r
                    border-beige-dark dark:border-[#1F1F1F]"
        >
          <p
            className="text-[11px] tracking-[2px] font-medium
                        text-brown-dark dark:text-white"
          >
            RETOURS GRATUITS
          </p>
          <p
            className="text-[12px] mt-1
                        text-brown-light dark:text-[#666]"
          >
            Sous 14 jours
          </p>
        </div>
        <div className="flex flex-col items-center py-6">
          <p
            className="text-[11px] tracking-[2px] font-medium
                        text-brown-dark dark:text-white"
          >
            SERVICE CLIENT
          </p>
          <p
            className="text-[12px] mt-1
                        text-brown-light dark:text-[#666]"
          >
            À votre écoute 7j/7
          </p>
        </div>
      </div>

      {/* Contenu principal */}
      <div
        className="max-w-[1100px] mx-auto py-10 px-10
                flex justify-between items-start"
      >
        <div>
          <Link
            to="/"
            className="font-serif text-[18px] tracking-[4px]
                            text-brown dark:text-rose-gold"
          >
            MELANIN
          </Link>
          <p
            className="text-[12px] mt-3 max-w-[220px] leading-relaxed
                        text-brown-light dark:text-[#555]"
          >
            Wigs, mèches et soins capillaires de qualité premium pour sublimer
            votre beauté.
          </p>

          {/* Réseaux sociaux */}
          <div className="flex gap-4 mt-4">
            <a
              href="https://instagram.com"
              target="_blank"
              rel="noopener noreferrer"
              className="text-brown-light dark:text-[#666] hover:text-gold dark:hover:text-rose-gold"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="18"
                height="18"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <rect width="20" height="20" x="2" y="2" rx="5" ry="5" />
                <path d="M16 11.37A4 4 0 1 1 12.63 8 4 4 0 0 1 16 11.37z" />
                <line x1="17.5" x2="17.51" y1="6.5" y2="6.5" />
              </svg>
            </a>
            <a
              href="https://tiktok.com"
              target="_blank"
              rel="noopener noreferrer"
              className="text-brown-light dark:text-[#666] hover:text-gold dark:hover:text-rose-gold"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="18"
                height="18"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <path d="M9 12a4 4 0 1 0 4 4V4a5 5 0 0 0 5 5" />
              </svg>
            </a>
            <a
              href="https://facebook.com"
              target="_blank"
              rel="noopener noreferrer"
              className="text-brown-light dark:text-[#666] hover:text-gold dark:hover:text-rose-gold"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="18"
                height="18"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <path d="M18 2h-3a5 5 0 0 0-5 5v3H7v4h3v8h4v-8h3l1-4h-4V7a1 1 0 0 1 1-1h3z" />
              </svg>
            </a>
          </div>
        </div>

        <div className="flex gap-14">
          <div className="flex flex-col gap-3">
            <p
              className="text-[11px] tracking-[2px]
                            text-gold dark:text-rose-gold"
            >
              BOUTIQUE
            </p>
            <Link
              to="/"
              className="text-[13px] hover:opacity-70
                            text-brown-light dark:text-[#777]"
            >
              Wigs
            </Link>
            <Link
              to="/"
              className="text-[13px] hover:opacity-70
                            text-brown-light dark:text-[#777]"
            >
              Mèches
            </Link>
            <Link
              to="/"
              className="text-[13px] hover:opacity-70
                            text-brown-light dark:text-[#777]"
            >
              Soins capillaires
            </Link>
          </div>

          <div className="flex flex-col gap-3">
            <p
              className="text-[11px] tracking-[2px]
                            text-gold dark:text-rose-gold"
            >
              INFORMATIONS
            </p>
            <Link
              to="/livraison"
              className="text-[13px] hover:opacity-70
                            text-brown-light dark:text-[#777]"
            >
              Livraison
            </Link>
            <Link
              to="/retours"
              className="text-[13px] hover:opacity-70
                            text-brown-light dark:text-[#777]"
            >
              Retours & échanges
            </Link>
            <Link
              to="/cgv"
              className="text-[13px] hover:opacity-70
                            text-brown-light dark:text-[#777]"
            >
              CGV
            </Link>
            <Link
              to="/confidentialite"
              className="text-[13px] hover:opacity-70
                            text-brown-light dark:text-[#777]"
            >
              Confidentialité
            </Link>
          </div>

          <div className="flex flex-col gap-3">
            <p
              className="text-[11px] tracking-[2px]
                            text-gold dark:text-rose-gold"
            >
              CONTACT
            </p>
            <p
              className="text-[13px]
                            text-brown-light dark:text-[#777]"
            >
              contact@melanin.com
            </p>
            <p
              className="text-[13px]
                            text-brown-light dark:text-[#777]"
            >
              Bruxelles, Belgique
            </p>
            <Link
              to="/login"
              className="text-[13px] hover:opacity-70
                            text-brown-light dark:text-[#777]"
            >
              Mon compte
            </Link>
          </div>
        </div>
      </div>

      {/* Paiement + Copyright */}
      <div
        className="border-t py-5 px-10
                border-beige-dark dark:border-[#1A1A1A]"
      >
        <div className="max-w-[1100px] mx-auto flex justify-between items-center">
          <p
            className="text-[11px] tracking-[1px]
                        text-brown-light dark:text-[#444]"
          >
            © 2026 MELANIN — Tous droits réservés
          </p>

          {/* Icônes de paiement */}
          <div className="flex gap-3 items-center">
            <span
              className="text-[10px] tracking-[1px] mr-2
                            text-brown-light dark:text-[#444]"
            >
              PAIEMENT SÉCURISÉ
            </span>
            {/* Visa */}
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="36"
              height="24"
              viewBox="0 0 36 24"
              className="text-brown-light dark:text-[#555]"
            >
              <rect
                width="36"
                height="24"
                rx="3"
                fill="currentColor"
                fillOpacity="0.1"
                stroke="currentColor"
                strokeOpacity="0.2"
                strokeWidth="0.5"
              />
              <text
                x="18"
                y="15"
                textAnchor="middle"
                fill="currentColor"
                fontSize="9"
                fontWeight="600"
                fontFamily="sans-serif"
              >
                VISA
              </text>
            </svg>
            {/* Mastercard */}
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="36"
              height="24"
              viewBox="0 0 36 24"
              className="text-brown-light dark:text-[#555]"
            >
              <rect
                width="36"
                height="24"
                rx="3"
                fill="currentColor"
                fillOpacity="0.1"
                stroke="currentColor"
                strokeOpacity="0.2"
                strokeWidth="0.5"
              />
              <circle
                cx="14"
                cy="12"
                r="5"
                fill="currentColor"
                fillOpacity="0.15"
              />
              <circle
                cx="22"
                cy="12"
                r="5"
                fill="currentColor"
                fillOpacity="0.15"
              />
              <text
                x="18"
                y="21"
                textAnchor="middle"
                fill="currentColor"
                fontSize="4"
                fontFamily="sans-serif"
              >
                MASTERCARD
              </text>
            </svg>
            {/* Stripe */}
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="36"
              height="24"
              viewBox="0 0 36 24"
              className="text-brown-light dark:text-[#555]"
            >
              <rect
                width="36"
                height="24"
                rx="3"
                fill="currentColor"
                fillOpacity="0.1"
                stroke="currentColor"
                strokeOpacity="0.2"
                strokeWidth="0.5"
              />
              <text
                x="18"
                y="15"
                textAnchor="middle"
                fill="currentColor"
                fontSize="7"
                fontWeight="600"
                fontFamily="sans-serif"
              >
                stripe
              </text>
            </svg>
          </div>
        </div>
      </div>
    </footer>
  );
}
