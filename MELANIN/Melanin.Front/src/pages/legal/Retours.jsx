export default function Retours() {
  return (
    <div className="max-w-[800px] mx-auto px-10 py-16">
      <h1 className="font-serif text-[28px] tracking-[3px] mb-8 text-brown-dark dark:text-white">
        RETOURS & ÉCHANGES
      </h1>
      <div className="flex flex-col gap-6 text-[14px] leading-relaxed text-brown-light dark:text-[#999]">
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">CONDITIONS</h2>
          <p>Vous disposez de 14 jours après réception pour retourner un article. Les produits doivent être retournés dans leur état d'origine, non utilisés et dans leur emballage d'origine.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">PROCÉDURE</h2>
          <p>Contactez notre service client à contact@melanin.com en indiquant votre numéro de commande. Nous vous enverrons une étiquette de retour gratuite.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">REMBOURSEMENT</h2>
          <p>Le remboursement sera effectué sous 5 à 7 jours ouvrables après réception et vérification du retour.</p>
        </section>
      </div>
    </div>
  );
}