export default function Confidentialite() {
  return (
    <div className="max-w-[800px] mx-auto px-10 py-16">
      <h1 className="font-serif text-[28px] tracking-[3px] mb-8 text-brown-dark dark:text-white">
        POLITIQUE DE CONFIDENTIALITÉ
      </h1>
      <div className="flex flex-col gap-6 text-[14px] leading-relaxed text-brown-light dark:text-[#999]">
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">DONNÉES COLLECTÉES</h2>
          <p>Nous collectons les données nécessaires au traitement de vos commandes : nom, prénom, adresse email, adresse de livraison.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">UTILISATION</h2>
          <p>Vos données sont utilisées uniquement pour le traitement de vos commandes et la communication relative à celles-ci. Elles ne sont jamais vendues à des tiers.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">VOS DROITS</h2>
          <p>Conformément au RGPD, vous disposez d'un droit d'accès, de rectification et de suppression de vos données. Contactez-nous à contact@melanin.com.</p>
        </section>
      </div>
    </div>
  );
}