const ORDER_STATUS_LABELS = {
  Pending: "En attente",
  Confirmed: "Confirmée",
  Shipped: "Expédiée",
  Delivered: "Livrée",
  Cancelled: "Annulée",
};

export function formatOrderStatus(status) {
  return ORDER_STATUS_LABELS[status] ?? status;
}
