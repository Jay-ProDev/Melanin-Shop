import { Link } from "react-router";

export default function Home() {
  return (
    <div className="flex flex-col items-center justify-center text-center py-40 px-8">
      <p
        className="text-[11px] tracking-[6px] mb-5
                text-gold dark:text-rose-gold"
      >
        COLLECTION 2026
      </p>
      <h1
        className="font-serif text-[42px] font-normal leading-tight
                text-brown-dark dark:text-white"
      >
        Sublimez votre
        <br />
        beauté naturelle
      </h1>
      <p
        className="text-[14px] mt-4 max-w-[380px] leading-relaxed
                text-brown-light dark:text-[#777]"
      >
        Wigs, mèches et soins capillaires de qualité premium pour révéler votre
        éclat.
      </p>
      <div className="mt-7 flex gap-4">
        <Link
          to="/shop"
          className="text-[12px] tracking-[2px] font-medium px-8 py-3
        text-beige bg-brown hover:bg-brown-dark
        dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark"
        >
          DÉCOUVRIR
        </Link>
        <Link
          to="/shop"
          className="text-[12px] tracking-[2px] px-8 py-3 border
    text-brown-dark border-brown-light hover:bg-beige-dark
    dark:text-[#AAA] dark:border-[#555] dark:hover:bg-[#1A1A1A] dark:hover:text-rose-gold dark:hover:border-rose-gold-dark"
        >
          NOS SOINS
        </Link>
      </div>
    </div>
  );
}
