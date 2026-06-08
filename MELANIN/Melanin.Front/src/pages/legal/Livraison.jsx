export default function Livraison() {
  return (
    <div className="max-w-[800px] mx-auto px-10 py-16">
      <h1 className="font-serif text-[28px] tracking-[3px] mb-8 text-brown-dark dark:text-white">
        LIVRAISON
      </h1>
      <div className="flex flex-col gap-6 text-[14px] leading-relaxed text-brown-light dark:text-[#999]">
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">DÉLAIS</h2>
          <p>Livraison en Belgique sous 3 à 5 jours ouvrables. Livraison en France et Luxembourg sous 5 à 7 jours ouvrables.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">FRAIS</h2>
          <p>Livraison offerte en Belgique dès 50€ d'achat. En dessous de ce montant, les frais de livraison sont de 4,95€.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">SUIVI</h2>
          <p>Un email de confirmation avec numéro de suivi vous sera envoyé dès l'expédition de votre commande.</p>
        </section>
      </div>
    </div>
  );
}