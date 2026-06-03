import { useEffect } from "react";
import { useParams, useSearchParams, useNavigate, Link } from "react-router";

export default function PaymentCancel() {
  const { id } = useParams();
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  useEffect(() => {
    const isFromStripe = searchParams.get("cancelled") === "true";
    if (!isFromStripe) {
      navigate(`/orders/${id}`, { replace: true });
    }
  }, [id, searchParams, navigate]);

  return (
    <section className="max-w-2xl mx-auto px-6 py-28 text-center">
      <h1 className="text-3xl font-serif tracking-widest uppercase text-brown-dark dark:text-white mb-6">
        Paiement annulé
      </h1>

      <p className="text-[13px] text-brown-light dark:text-[#777] mb-2">
        COMMANDE N° #{id}
      </p>

      <p className="text-[14px] text-brown-light dark:text-[#999] mb-12 leading-relaxed mt-8">
        Vous avez annulé le paiement. Aucun montant n'a été débité.
        <br />
        Votre commande reste en attente — vous pouvez la reprendre à tout
        moment depuis vos commandes.
      </p>

      <div className="flex flex-col sm:flex-row gap-4 justify-center">
        <Link
          to={`/orders/${id}`}
          className="px-8 py-3 text-[12px] tracking-[2px] text-center cursor-pointer font-medium border
            text-brown border-brown hover:bg-beige
            dark:text-rose-gold dark:border-rose-gold dark:hover:bg-[#1A1A1A]"
        >
          VOIR MA COMMANDE
        </Link>
        <Link
          to="/shop"
          className="px-8 py-3 text-[12px] tracking-[2px] text-center cursor-pointer font-medium
            text-beige bg-brown hover:bg-brown-dark
            dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark"
        >
          CONTINUER MES ACHATS
        </Link>
      </div>
    </section>
  );
}