import { useState, useEffect } from "react";
import { useParams, Link } from "react-router";
import {
  getOrderById,
  shipOrder,
  deliverOrder,
  adminCancelOrder,
} from "../../services/orderService";
import {
  formatOrderStatus,
  getOrderStatusStyle,
} from "../../utils/orderStatus";

export default function AdminOrderDetail() {
  const { id } = useParams();

  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [actionError, setActionError] = useState("");
  const [isProcessing, setIsProcessing] = useState(false);

  useEffect(() => {
    const loadOrder = async () => {
      const result = await getOrderById(id);
      if (result.success) {
        setOrder(result.data);
      } else {
        setError(result.error);
      }
      setLoading(false);
    };
    loadOrder();
  }, [id]);

  // Recharge la commande après une action
  const reloadOrder = async () => {
    const result = await getOrderById(id);
    if (result.success) {
      setOrder(result.data);
    }
  };

  const handleShip = async () => {
    setActionError("");
    setIsProcessing(true);
    const result = await shipOrder(id);
    if (result.success) {
      await reloadOrder();
    } else {
      setActionError(result.error);
    }
    setIsProcessing(false);
  };

  const handleDeliver = async () => {
    setActionError("");
    setIsProcessing(true);
    const result = await deliverOrder(id);
    if (result.success) {
      await reloadOrder();
    } else {
      setActionError(result.error);
    }
    setIsProcessing(false);
  };

  const handleCancel = async () => {
    if (!confirm("Êtes-vous sûr de vouloir annuler cette commande ?")) {
      return;
    }
    setActionError("");
    setIsProcessing(true);
    const result = await adminCancelOrder(id);
    if (result.success) {
      await reloadOrder();
    } else {
      setActionError(result.error);
    }
    setIsProcessing(false);
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center py-20">
        <p className="text-brown/60 dark:text-rose-gold/60 tracking-widest uppercase text-sm">
          Chargement...
        </p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex flex-col items-center py-20 gap-4">
        <p className="text-red-500 text-sm">{error}</p>
        <Link
          to="/admin/orders"
          className="text-sm text-gold dark:text-rose-gold underline"
        >
          Retour aux commandes
        </Link>
      </div>
    );
  }

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
          <span
            className={`inline-block text-[11px] tracking-[1px] px-3 py-1 ${getOrderStatusStyle(order.status)}`}
          >
            {formatOrderStatus(order.status).toUpperCase()}
          </span>
        </div>
      </div>

      {/* Section CLIENT */}
      <div className="mb-12">
        <h2 className="font-serif text-[22px] mb-6 text-brown-dark dark:text-white">
          Client
        </h2>
        <div className="flex flex-col gap-2">
          <p className="text-[14px] text-brown-dark dark:text-white">
            {order.memberFirstName} {order.memberLastName}
          </p>
          <p className="text-[13px] text-brown-light dark:text-[#999]">
            {order.memberEmail}
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
              className="flex items-start gap-4 pb-4 border-b border-beige-dark dark:border-[#2A2A2A]"
            >
              <div className="w-20 h-20 shrink-0 bg-beige dark:bg-[#1A1A1A] flex items-center justify-center overflow-hidden">
                {item.productImageUrl ? (
                  <img
                    src={`${import.meta.env.VITE_API_URL.replace("/api", "")}${item.productImageUrl}`}
                    alt={item.productName}
                    className="w-full h-full object-cover"
                  />
                ) : (
                  <span className="text-brown/40 dark:text-rose-gold/40 text-[10px] tracking-[1px]">
                    PHOTO
                  </span>
                )}
              </div>
              <div className="flex-1">
                <p className="text-[14px] text-brown-dark dark:text-white">
                  {item.productName}
                </p>
                <p className="text-[12px] text-brown-light dark:text-[#999] mt-1">
                  Quantité : {item.quantity} × {item.unitPrice.toFixed(2)} €
                </p>
              </div>
              <p className="text-[14px] font-medium text-brown-dark dark:text-white shrink-0">
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

      {/* Actions selon le statut */}
      {(order.status === "Pending" ||
        order.status === "Confirmed" ||
        order.status === "Shipped") && (
        <div className="mb-12 px-6 py-6 bg-beige dark:bg-[#1A1A1A] border-l-2 border-gold dark:border-rose-gold">
          <h2 className="font-serif text-[18px] mb-4 text-brown-dark dark:text-white">
            Actions
          </h2>
          <div className="flex flex-wrap gap-3">
            {/* Bouton EXPÉDIER */}
            {(order.status === "Pending" || order.status === "Confirmed") && (
              <button
                onClick={handleShip}
                disabled={isProcessing}
                className="px-6 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium
                  text-beige bg-brown hover:bg-brown-dark
                  dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark
                  disabled:opacity-40 disabled:cursor-not-allowed"
              >
                {isProcessing ? "TRAITEMENT..." : "EXPÉDIER"}
              </button>
            )}

            {/* Bouton LIVRER */}
            {order.status === "Shipped" && (
              <button
                onClick={handleDeliver}
                disabled={isProcessing}
                className="px-6 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium
                  text-beige bg-brown hover:bg-brown-dark
                  dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark
                  disabled:opacity-40 disabled:cursor-not-allowed"
              >
                {isProcessing ? "TRAITEMENT..." : "LIVRER"}
              </button>
            )}

            {/* Bouton ANNULER */}
            {(order.status === "Pending" || order.status === "Confirmed") && (
              <button
                onClick={handleCancel}
                disabled={isProcessing}
                className="px-6 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium border
                  text-red-500 border-red-500 hover:bg-red-500 hover:text-white
                  disabled:opacity-40 disabled:cursor-not-allowed"
              >
                {isProcessing ? "TRAITEMENT..." : "ANNULER"}
              </button>
            )}
          </div>
        </div>
      )}

      {/* Erreur d'action */}
      {actionError && (
        <p className="text-[13px] text-center mb-6 text-red-500">
          {actionError}
        </p>
      )}

      {/* Bouton retour */}
      <div className="flex justify-center">
        <Link
          to="/admin/orders"
          className="px-8 py-3 text-[12px] tracking-[2px] text-center cursor-pointer font-medium border
            text-brown border-brown hover:bg-beige
            dark:text-rose-gold dark:border-rose-gold dark:hover:bg-[#1A1A1A]"
        >
          ← RETOUR AUX COMMANDES
        </Link>
      </div>
    </section>
  );
}
