export const ORDER_STATUS_LABELS = {
  Pending: "En attente",
  Confirmed: "Confirmée",
  Shipped: "Expédiée",
  Delivered: "Livrée",
  Cancelled: "Annulée",
};

const ORDER_STATUS_STYLES = {
  Pending:
    "bg-amber-100 text-amber-800 dark:bg-amber-900/30 dark:text-amber-300",
  Confirmed: "bg-blue-100 text-blue-800 dark:bg-blue-900/30 dark:text-blue-300",
  Shipped:
    "bg-purple-100 text-purple-800 dark:bg-purple-900/30 dark:text-purple-300",
  Delivered:
    "bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-300",
  Cancelled: "bg-red-100 text-red-800 dark:bg-red-900/30 dark:text-red-300",
};

export function formatOrderStatus(status) {
  return ORDER_STATUS_LABELS[status] ?? status;
}

export function getOrderStatusStyle(status) {
  return ORDER_STATUS_STYLES[status] ?? "bg-gray-100 text-gray-800";
}
