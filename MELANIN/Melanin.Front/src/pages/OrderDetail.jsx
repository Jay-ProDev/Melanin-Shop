import { useState, useEffect } from "react";
import { useParams, Link } from "react-router";
import { useAtom } from "jotai";
import { tokenAtom } from "../store";
import { getMyOrder } from "../services/orderService";
import { formatOrderStatus } from "../utils/orderStatus";

export default function OrderDetail() {
  const { id } = useParams();
  const [token] = useAtom(tokenAtom);

  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadOrder = async () => {
      const result = await getMyOrder(id);
      if (result.success) {
        setOrder(result.data);
      } else {
        setError(result.error);
      }
      setLoading(false);
    };

    loadOrder();
  }, [id, token]);

  if (loading) {
    return (
      <div className="flex justify-center py-28">
        <p className="text-[13px] text-brown-light dark:text-[#777]">
          Chargement...
        </p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex flex-col items-center py-28 gap-6">
        <p className="text-[13px] text-red-500">{error}</p>
        <Link
          to="/profile"
          className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999] hover:opacity-70"
        >
          RETOUR AU PROFIL
        </Link>
      </div>
    );
  }

  // Format date JJ/MM/AAAA
  const orderDate = new Date(order.createdAt).toLocaleDateString("fr-FR");

  return (
    <section className="max-w-3xl mx-auto px-6 py-16">
      {/* En-tête */}
      <div className="text-center mb-12">
        <h1 className="text-3xl font-serif tracking-widest uppercase text-brown-dark dark:text-white">
          Commande #{order.id}
        </h1>
      </div>

      {/* Infos en-tête : numéro, date, statut */}
      <div className="flex justify-center gap-12 mb-12 pb-8 border-b border-beige-dark dark:border-[#2A2A2A]">
        <div className="text-center">
          <p className="text-[11px] tracking-[2px] mb-2 text-brown-light dark:text-[#999]">
            COMMANDE N°
          </p>
          <p className="font-serif text-[18px] text-brown-dark dark:text-white">
            #{order.id}
          </p>
        </div>
        <div className="text-center">
          <p className="text-[11px] tracking-[2px] mb-2 text-brown-light dark:text-[#999]">
            DATE
          </p>
          <p className="font-serif text-[18px] text-brown-dark dark:text-white">
            {orderDate}
          </p>
        </div>
        <div className="text-center">
          <p className="text-[11px] tracking-[2px] mb-2 text-brown-light dark:text-[#999]">
            STATUT
          </p>
          <p className="font-serif text-[18px] text-brown-dark dark:text-white">
            {formatOrderStatus(order.status)}
          </p>
        </div>
      </div>

      {/* Articles */}
      <div className="mb-12">
        <h2 className="font-serif text-[22px] mb-6 text-brown-dark dark:text-white">
          Articles
        </h2>
        <div className="flex flex-col gap-4">
          {order.orderItems.map((item) => (
            <div
              key={item.id}
              className="flex justify-between items-start gap-4 pb-4 border-b border-beige-dark dark:border-[#2A2A2A]"
            >
              <div className="flex-1">
                <p className="text-[14px] text-brown-dark dark:text-white">
                  {item.productName}
                </p>
                <p className="text-[12px] text-brown-light dark:text-[#999] mt-1">
                  Quantité : {item.quantity} × {item.unitPrice.toFixed(2)} €
                </p>
              </div>
              <p className="text-[14px] font-medium text-brown-dark dark:text-white">
                {item.totalPrice.toFixed(2)} €
              </p>
            </div>
          ))}
        </div>
      </div>

      {/* Adresse de livraison */}
      <div className="mb-12">
        <h2 className="font-serif text-[22px] mb-6 text-brown-dark dark:text-white">
          Adresse de livraison
        </h2>
        <div className="flex flex-col gap-2">
          {order.shippingAddress.fullName && (
            <p className="text-[14px] text-brown-dark dark:text-white">
              {order.shippingAddress.fullName}
            </p>
          )}
          <p className="text-[14px] text-brown-dark dark:text-white">
            {order.shippingAddress.street}
          </p>
          <p className="text-[14px] text-brown-dark dark:text-white">
            {order.shippingAddress.postalCode} {order.shippingAddress.city}
          </p>
          <p className="text-[14px] text-brown-dark dark:text-white">
            {order.shippingAddress.country}
          </p>
          <p className="text-[14px] text-brown-light dark:text-[#999]">
            📞 {order.shippingAddress.phone}
          </p>
        </div>
      </div>

      {/* Total */}
      <div className="flex justify-between items-center mb-12 pt-4 border-t border-beige-dark dark:border-[#2A2A2A]">
        <p className="text-[14px] tracking-[2px] uppercase text-brown-dark dark:text-white">
          Total
        </p>
        <p className="font-serif text-[26px] text-brown-dark dark:text-white">
          {order.totalPrice.toFixed(2)} €
        </p>
      </div>

      {/* Message d'aide pour annulation/modification */}
      <div className="mb-8 px-6 py-4 bg-beige dark:bg-[#1A1A1A] border-l-2 border-gold dark:border-rose-gold">
        <p className="text-[12px] text-brown-light dark:text-[#999]">
          Pour annuler ou modifier cette commande, contactez notre service
          client.
        </p>
      </div>

      {/* Bouton retour */}
      <div className="flex justify-center">
        <Link
          to="/profile"
          className="px-8 py-3 text-[12px] tracking-[2px] text-center cursor-pointer font-medium border
            text-brown border-brown hover:bg-beige
            dark:text-rose-gold dark:border-rose-gold dark:hover:bg-[#1A1A1A]"
        >
          RETOUR AU PROFIL
        </Link>
      </div>
    </section>
  );
}
