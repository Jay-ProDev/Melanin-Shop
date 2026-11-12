import { Link } from "react-router";

export default function Home() {
  return (
    <>
      {/* ===== Accueil ===== */}
      <div className="flex flex-col items-center justify-center text-center py-40 px-8">
        <p className="text-[11px] tracking-[6px] mb-5 text-gold dark:text-rose-gold animate-fade-up">
          COLLECTION 2027
        </p>

        <h1 className="font-serif text-[42px] font-normal leading-tight text-brown-dark dark:text-white animate-fade-up [animation-delay:150ms]">
          Sublimez votre
          <br />
          beauté naturelle
        </h1>

        <p className="text-[14px] mt-4 max-w-[380px] leading-relaxed text-brown-light dark:text-[#777] animate-fade-up [animation-delay:300ms]">
          Wigs, mèches et soins capillaires de qualité premium pour révéler votre
          éclat.
        </p>

        <div className="mt-7 flex gap-4 animate-fade-up [animation-delay:450ms]">
          <Link
            to="/shop"
            className="text-[12px] tracking-[2px] font-medium px-8 py-3
              text-beige bg-brown hover:bg-brown-dark
              dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark"
          >
            DÉCOUVRIR
          </Link>
          <a
            href="#notre-histoire"
            className="text-[12px] tracking-[2px] px-8 py-3 border
              text-brown-dark border-brown-light hover:bg-beige-dark
              dark:text-[#AAA] dark:border-[#555] dark:hover:bg-[#1A1A1A] dark:hover:text-rose-gold dark:hover:border-rose-gold-dark"
          >
            NOTRE HISTOIRE
          </a>
        </div>
      </div>

      {/* ===== Notre histoire ===== */}
      <section
        id="notre-histoire"
        className="scroll-mt-24 border-t border-beige-dark dark:border-[#1F1F1F] py-24 px-8"
      >
        <div className="max-w-[720px] mx-auto text-center">
          <div className="animate-fade-up [animation-delay:600ms]">
            <p className="text-[11px] tracking-[6px] mb-5 text-gold dark:text-rose-gold">
              NOTRE HISTOIRE
            </p>
            <h2 className="font-serif text-[32px] leading-tight mb-8 text-brown-dark dark:text-white">
              Une chevelure
              <br />
              pour chaque histoire
            </h2>
          </div>

          <div className="space-y-5 text-[14px] leading-relaxed text-brown-light dark:text-[#888]">
            <p className="animate-fade-up [animation-delay:1000ms]">
              Chez Melanin, nous croyons que les cheveux racontent quelque chose
              de nous, et que chacun devrait pouvoir choisir ce qu'ils racontent.
            </p>
            <p className="animate-fade-up [animation-delay:1400ms]">
              Changer de style le temps d'une soirée, gagner du temps au
              quotidien, protéger ses cheveux naturels, retrouver confiance
              après une perte de cheveux, ou suivre ses convictions : les
              raisons de porter une perruque ou des mèches sont aussi variées
              que les personnes qui les portent. Des artistes sur scène aux
              femmes de tous les jours, chacune y trouve sa liberté.
            </p>
            <p className="animate-fade-up [animation-delay:1800ms]">
              Née au cœur de la capitale de l'Europe, Melanin sélectionne des
              perruques, des mèches et des soins de qualité pour tous les types
              de cheveux, tous les styles et toutes les envies. Pas de profil
              type, pas de règle : juste l'envie de vous sentir bien, comme vous
              êtes ou comme vous voulez être.
            </p>
          </div>
        </div>

        {/* Engagements : arrivent un par un, chaque titre scintille dans sa couleur */}
        <div className="max-w-[1000px] mx-auto mt-16 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-10 text-center">
          <div className="animate-fade-up [animation-delay:2200ms]">
            <p className="font-serif text-[22px] mb-2 bg-linear-to-r from-red-500 via-orange-400 to-red-500 bg-size-[200%_auto] bg-clip-text text-transparent animate-shimmer">
              Qualité
            </p>
            <p className="text-[13px] text-brown-light dark:text-[#777]">
              Des produits choisis pour durer.
            </p>
          </div>

          <div className="animate-fade-up [animation-delay:2400ms]">
            <p className="font-serif text-[22px] mb-2 bg-linear-to-r from-amber-500 via-yellow-400 to-amber-500 bg-size-[200%_auto] bg-clip-text text-transparent animate-shimmer">
              Pour tous
            </p>
            <p className="text-[13px] text-brown-light dark:text-[#777]">
              Tous les cheveux, tous les styles, toutes les envies.
            </p>
          </div>

          <div className="animate-fade-up [animation-delay:2600ms]">
            <p className="font-serif text-[22px] mb-2 bg-linear-to-r from-emerald-500 via-lime-400 to-emerald-500 bg-size-[200%_auto] bg-clip-text text-transparent animate-shimmer">
              Liberté
            </p>
            <p className="text-[13px] text-brown-light dark:text-[#777]">
              Changer de look quand vous le voulez.
            </p>
          </div>

          <div className="animate-fade-up [animation-delay:2800ms]">
            <p className="font-serif text-[22px] mb-2 bg-linear-to-r from-sky-500 via-violet-500 to-sky-500 bg-size-[200%_auto] bg-clip-text text-transparent animate-shimmer">
              Proximité
            </p>
            <p className="text-[13px] text-brown-light dark:text-[#777]">
              Une marque à votre écoute, au cœur de l'Europe.
            </p>
          </div>
        </div>
      </section>
    </>
  );
}