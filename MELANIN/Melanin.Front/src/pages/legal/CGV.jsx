export default function CGV() {
  return (
    <div className="max-w-[800px] mx-auto px-10 py-16">
      <h1 className="font-serif text-[28px] tracking-[3px] mb-8 text-brown-dark dark:text-white">
        CONDITIONS GÉNÉRALES DE VENTE
      </h1>
      <div className="flex flex-col gap-6 text-[14px] leading-relaxed text-brown-light dark:text-[#999]">
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">OBJET</h2>
          <p>Les présentes CGV régissent les ventes effectuées sur le site Melanin. Toute commande implique l'acceptation sans réserve des présentes conditions.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">PRIX</h2>
          <p>Les prix sont indiqués en euros TTC. Melanin se réserve le droit de modifier ses prix à tout moment, les commandes étant facturées au tarif en vigueur lors de la validation.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">PAIEMENT</h2>
          <p>Le paiement s'effectue en ligne par carte bancaire via Stripe. La transaction est sécurisée et les données bancaires ne sont pas conservées.</p>
        </section>
        <section>
          <h2 className="text-[13px] tracking-[2px] font-medium mb-2 text-brown-dark dark:text-white">PROPRIÉTÉ</h2>
          <p>Les produits restent la propriété de Melanin jusqu'au complet paiement du prix.</p>
        </section>
      </div>
    </div>
  );
}