import { useState, useEffect } from "react";
import { Link } from "react-router";
import {
  getAllProducts,
  activateProduct,
  deactivateProduct,
  increaseStock,
  decreaseStock,
  deleteProduct,
} from "../../services/productService";

export default function Dashboard() {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [stockInputs, setStockInputs] = useState({});

  async function fetchProducts() {
    setLoading(true);
    setError(null);
    const result = await getAllProducts();
    if (result.success) {
      setProducts(result.data);
    } else {
      setError(result.error);
    }
    setLoading(false);
  }

  useEffect(() => {
    fetchProducts();
  }, []);

  useEffect(() => {
    if (!error) return;
    const timer = setTimeout(() => setError(null), 3000);
    return () => clearTimeout(timer);
  }, [error]);

  async function handleToggleActive(p) {
    const fn = p.isActive ? deactivateProduct : activateProduct;
    const result = await fn(p.id);
    if (result.success) fetchProducts();
    else setError(result.error);
  }

  async function handleIncrease(id) {
    const qty = parseInt(stockInputs[id]) || 1;
    const result = await increaseStock(id, qty);
    if (result.success) {
      setStockInputs((prev) => ({ ...prev, [id]: "" }));
      fetchProducts();
    } else {
      setError(result.error);
    }
  }

  async function handleDecrease(id) {
    const qty = parseInt(stockInputs[id]) || 1;
    const result = await decreaseStock(id, qty);
    if (result.success) {
      setStockInputs((prev) => ({ ...prev, [id]: "" }));
      fetchProducts();
    } else {
      setError(result.error);
    }
  }

  async function handleDelete(id, name) {
    if (!confirm(`Supprimer "${name}" ?`)) return;
    const result = await deleteProduct(id);
    if (result.success) fetchProducts();
    else setError(result.error);
  }

  return (
    <section className="max-w-7xl mx-auto px-6 py-10">
      <div className="flex justify-between items-center mb-10">
        <div>
          <h1 className="text-3xl font-serif tracking-widest uppercase text-brown dark:text-rose-gold">
            Dashboard
          </h1>
          <p className="text-xs text-brown/50 dark:text-rose-gold/50 mt-1">
            {products.length} produit(s)
          </p>
        </div>
        <div className="flex gap-3">
          <Link
            to="/admin/products/create"
            className="px-5 py-2 text-xs tracking-widest uppercase bg-brown dark:bg-rose-gold text-white dark:text-black rounded hover:opacity-90 transition-opacity"
          >
            + Produit
          </Link>
          <Link
            to="/admin/categories"
            className="px-5 py-2 text-xs tracking-widest uppercase border border-brown dark:border-rose-gold text-brown dark:text-rose-gold rounded hover:opacity-80 transition-opacity"
          >
            Catégories
          </Link>
        </div>
      </div>

      {error && <p className="text-red-500 text-sm mb-4">{error}</p>}

      {loading ? (
        <p className="text-center text-brown/60 dark:text-rose-gold/60 text-sm tracking-widest uppercase py-20">
          Chargement...
        </p>
      ) : products.length === 0 ? (
        <p className="text-center text-brown/60 dark:text-rose-gold/60 text-sm tracking-widest uppercase py-20">
          Aucun produit
        </p>
      ) : (
        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="border-b border-beige-dark dark:border-zinc-700 text-left text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60">
                <th className="py-3 pr-4">Produit</th>
                <th className="py-3 pr-4">Catégorie</th>
                <th className="py-3 pr-4">Prix</th>
                <th className="py-3 pr-4">Stock</th>
                <th className="py-3 pr-4">Statut</th>
                <th className="py-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {products.map((p) => (
                <tr
                  key={p.id}
                  className={`border-b border-beige-dark/50 dark:border-zinc-800 ${!p.isActive ? "opacity-50" : ""}`}
                >
                  {/* Miniature + Nom */}
                  <td className="py-4 pr-4">
                    <div className="flex items-center gap-3">
                      <div className="w-10 h-10 rounded bg-beige dark:bg-zinc-800 flex items-center justify-center shrink-0 overflow-hidden">
                        {p.imageUrl ? (
                          <img
                            src={`${import.meta.env.VITE_API_URL.replace("/api", "")}${p.imageUrl}`}
                            alt={p.name}
                            className="w-full h-full object-cover"
                          />
                        ) : (
                          <span className="text-brown/30 dark:text-rose-gold/30 text-[8px]">
                            IMG
                          </span>
                        )}
                      </div>
                      <span className="text-brown dark:text-rose-gold font-medium">
                        {p.name}
                      </span>
                    </div>
                  </td>

                  <td className="py-4 pr-4 text-brown/70 dark:text-rose-gold/70">
                    {p.categoryName}
                  </td>

                  <td className="py-4 pr-4 text-gold dark:text-gold-light">
                    {p.unitPrice.toFixed(2)} €
                  </td>

                  {/* Stock avec input quantité */}
                  <td className="py-4 pr-4">
                    <div className="flex items-center gap-1">
                      <button
                        onClick={() => handleDecrease(p.id)}
                        disabled={p.stockQuantity === 0}
                        className="w-6 h-6 rounded bg-beige-dark dark:bg-zinc-700 text-brown dark:text-rose-gold text-xs hover:opacity-80 disabled:opacity-30 disabled:cursor-not-allowed"
                      >
                        −
                      </button>
                      <span className="text-brown dark:text-rose-gold kw-8 text-center text-xs">
                        {p.stockQuantity}
                      </span>
                      <button
                        onClick={() => handleIncrease(p.id)}
                        className="w-6 h-6 rounded bg-beige-dark dark:bg-zinc-700 text-brown dark:text-rose-gold text-xs hover:opacity-80"
                      >
                        +
                      </button>
                      <input
                        type="number"
                        min="1"
                        placeholder="qté"
                        value={stockInputs[p.id] || ""}
                        onChange={(e) =>
                          setStockInputs((prev) => ({
                            ...prev,
                            [p.id]: e.target.value,
                          }))
                        }
                        style={{ width: "60px" }}
                        className="ml-2 px-1 py-0.5 text-xs text-center border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none"
                      />
                    </div>
                  </td>

                  {/* Toggle Switch */}
                  <td className="py-4 pr-4">
                    <button
                      onClick={() => handleToggleActive(p)}
                      style={{
                        width: "32px",
                        height: "18px",
                        backgroundColor: p.isActive ? "#22c55e" : "#ef4444",
                        borderRadius: "9px",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: p.isActive ? "flex-end" : "flex-start",
                        padding: "2px",
                        cursor: "pointer",
                        transition: "background-color 0.3s",
                      }}
                    >
                      <span
                        style={{
                          width: "14px",
                          height: "14px",
                          backgroundColor: "white",
                          borderRadius: "50%",
                          display: "block",
                          boxShadow: "0 1px 3px rgba(0,0,0,0.2)",
                        }}
                      />
                    </button>
                  </td>

                  {/* Actions */}
                  <td className="py-4 text-right">
                    <div className="flex justify-end items-center gap-3">
                      <Link
                        to={`/admin/products/edit/${p.id}`}
                        className="text-xs tracking-wide px-3 py-1 border border-brown dark:border-rose-gold text-brown dark:text-rose-gold rounded hover:opacity-80"
                      >
                        Modifier
                      </Link>
                      <button
                        onClick={() => handleDelete(p.id, p.name)}
                        className="text-base hover:scale-110 transition-transform"
                        title="Supprimer"
                      >
                        ❌
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
}
