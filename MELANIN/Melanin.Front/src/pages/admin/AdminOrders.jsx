import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router";
import { getAllOrders } from "../../services/orderService";
import {
  ORDER_STATUS_LABELS,
  formatOrderStatus,
  getOrderStatusStyle,
} from "../../utils/orderStatus";

export default function AdminOrders() {
  const [orders, setOrders] = useState([]);
  const [activeFilter, setActiveFilter] = useState("all");
  const [visibleCount, setVisibleCount] = useState(20);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  // Liste des statuts (extraite de notre dictionnaire de labels)
  const statusOptions = Object.keys(ORDER_STATUS_LABELS);

  useEffect(() => {
    const loadOrders = async () => {
      const result = await getAllOrders();
      if (result.success) {
        setOrders(result.data);
      } else {
        setError(result.error);
      }
      setLoading(false);
    };
    loadOrders();
  }, []);

  // Filtre selon le statut actif
  let filteredOrders;
  if (activeFilter === "all") {
    filteredOrders = orders;
  } else {
    filteredOrders = orders.filter((order) => order.status === activeFilter);
  }

  // Compte combien de commandes pour chaque statut
  const countByStatus = {};
  for (const order of orders) {
    const status = order.status;
    if (countByStatus[status]) {
      countByStatus[status] = countByStatus[status] + 1;
    } else {
      countByStatus[status] = 1;
    }
  }

  // Reset visibleCount quand on change de filtre
  const handleFilterChange = (filter) => {
    setActiveFilter(filter);
    setVisibleCount(20);
  };

  if (loading) {
    return (
      <div className="flex justify-center py-28">
        <p className="text-[13px] text-brown-light dark:text-[#777]">
          Chargement...
        </p>
      </div>
    );
  }

  return (
    <section className="max-w-6xl mx-auto px-6 py-16">
      {/* En-tête */}
      <div className="mb-8">
        <h1 className="text-3xl font-serif tracking-widest uppercase text-brown-dark dark:text-white mb-2">
          Commandes
        </h1>
        <p className="text-[13px] text-brown-light dark:text-[#777]">
          {orders.length} commande{orders.length > 1 ? "s" : ""} au total
        </p>
      </div>

      {error && <p className="text-[13px] text-red-500 mb-6">{error}</p>}

      {/* Filtres */}
      <div className="flex flex-wrap gap-2 mb-8">
        <button
          onClick={() => handleFilterChange("all")}
          className={`text-[11px] tracking-[2px] px-4 py-2 cursor-pointer border transition-colors ${
            activeFilter === "all"
              ? "bg-brown text-beige border-brown dark:bg-rose-gold dark:text-[#0A0A0A] dark:border-rose-gold"
              : "border-beige-dark text-brown-light hover:border-brown dark:border-[#2A2A2A] dark:text-[#999] dark:hover:border-rose-gold"
          }`}
        >
          TOUTES ({orders.length})
        </button>

        {statusOptions.map((status) => (
          <button
            key={status}
            onClick={() => handleFilterChange(status)}
            className={`text-[11px] tracking-[2px] px-4 py-2 cursor-pointer border transition-colors ${
              activeFilter === status
                ? "bg-brown text-beige border-brown dark:bg-rose-gold dark:text-[#0A0A0A] dark:border-rose-gold"
                : "border-beige-dark text-brown-light hover:border-brown dark:border-[#2A2A2A] dark:text-[#999] dark:hover:border-rose-gold"
            }`}
          >
            {formatOrderStatus(status).toUpperCase()} (
            {countByStatus[status] || 0})
          </button>
        ))}
      </div>

      {/* Tableau */}
      {filteredOrders.length === 0 ? (
        <p className="text-[13px] text-brown-light dark:text-[#777] py-12 text-center">
          Aucune commande dans cette catégorie.
        </p>
      ) : (
        <>
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-beige-dark dark:border-[#2A2A2A]">
                  <th className="text-left text-[11px] tracking-[2px] py-3 px-2 text-brown-light dark:text-[#999]">
                    N°
                  </th>
                  <th className="text-left text-[11px] tracking-[2px] py-3 px-2 text-brown-light dark:text-[#999]">
                    DATE
                  </th>
                  <th className="text-left text-[11px] tracking-[2px] py-3 px-2 text-brown-light dark:text-[#999]">
                    CLIENT
                  </th>
                  <th className="text-left text-[11px] tracking-[2px] py-3 px-2 text-brown-light dark:text-[#999]">
                    STATUT
                  </th>
                  <th className="text-right text-[11px] tracking-[2px] py-3 px-2 text-brown-light dark:text-[#999]">
                    TOTAL
                  </th>
                </tr>
              </thead>
              <tbody>
                {filteredOrders.slice(0, visibleCount).map((order) => (
                  <tr
                    key={order.id}
                    onClick={() => navigate(`/admin/orders/${order.id}`)}
                    className="border-b border-beige-dark dark:border-[#2A2A2A] hover:bg-beige dark:hover:bg-[#1A1A1A] cursor-pointer transition-colors"
                  >
                    <td className="py-4 px-2 text-[14px] font-serif text-brown-dark dark:text-white">
                      #{order.id}
                    </td>
                    <td className="py-4 px-2 text-[13px] text-brown-light dark:text-[#999]">
                      {new Date(order.createdAt).toLocaleDateString("fr-FR")}
                    </td>
                    <td className="py-4 px-2 text-[13px]">
                      <p className="text-brown-dark dark:text-white">
                        {order.memberFirstName} {order.memberLastName}
                      </p>
                      <p className="text-[11px] text-brown-light dark:text-[#999] mt-1">
                        {order.memberEmail}
                      </p>
                    </td>
                    <td className="py-4 px-2">
                      <span
                        className={`text-[10px] tracking-[1px] px-2 py-1 ${getOrderStatusStyle(order.status)}`}
                      >
                        {formatOrderStatus(order.status).toUpperCase()}
                      </span>
                    </td>
                    <td className="py-4 px-2 text-right text-[14px] font-serif text-brown-dark dark:text-white">
                      {order.totalPrice.toFixed(2)} €
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {visibleCount < filteredOrders.length && (
            <div className="flex justify-center mt-8">
              <button
                onClick={() => setVisibleCount((current) => current + 20)}
                className="px-8 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium border
                  text-brown border-brown hover:bg-beige
                  dark:text-rose-gold dark:border-rose-gold dark:hover:bg-[#1A1A1A]"
              >
                VOIR PLUS ({filteredOrders.length - visibleCount} restante
                {filteredOrders.length - visibleCount > 1 ? "s" : ""})
              </button>
            </div>
          )}
        </>
      )}

      <div className="mt-12 flex justify-center">
        <Link
          to="/admin"
          className="text-[11px] tracking-[2px] hover:opacity-70 text-brown-light dark:text-[#999]"
        >
          ← RETOUR DASHBOARD
        </Link>
      </div>
    </section>
  );
}
