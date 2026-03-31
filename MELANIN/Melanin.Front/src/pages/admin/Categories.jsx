import { useState, useEffect } from "react";
import { useNavigate } from "react-router";
import {
  getAllCategories,
  createCategory,
  updateCategory,
  deleteCategory,
} from "../../services/categoryService";

export default function Categories() {
  const navigate = useNavigate();
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editingId, setEditingId] = useState(null);

  async function fetchCategories() {
    setLoading(true);
    setError(null);
    const result = await getAllCategories();
    if (result.success) {
      setCategories(result.data);
    } else {
      setError(result.error);
    }
    setLoading(false);
  }

  useEffect(() => {
    fetchCategories();
  }, []);

  useEffect(() => {
    if (!error) return;
    const timer = setTimeout(() => setError(null), 3000);
    return () => clearTimeout(timer);
  }, [error]);

  async function handleCreate(formData) {
    setError(null);
    const name = formData.get("name");
    const slug = formData.get("slug");

    const result = await createCategory(name, slug);
    if (result.success) {
      fetchCategories();
    } else {
      setError(result.error);
    }
  }

  async function handleUpdate(formData) {
    setError(null);
    const name = formData.get("name");
    const slug = formData.get("slug");

    const result = await updateCategory(editingId, name, slug);
    if (result.success) {
      setEditingId(null);
      fetchCategories();
    } else {
      setError(result.error);
    }
  }

  async function handleDelete(id, name) {
    if (!confirm(`Supprimer la catégorie "${name}" ?`)) return;
    const result = await deleteCategory(id);
    if (result.success) {
      fetchCategories();
    } else {
      setError(result.error);
    }
  }

  return (
    <section className="max-w-2xl mx-auto px-6 py-10">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-2xl font-serif tracking-widest uppercase text-brown dark:text-rose-gold">
          Catégories
        </h1>
        <button
          onClick={() => navigate("/admin")}
          className="text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 hover:text-brown dark:hover:text-rose-gold transition-colors"
        >
          ← Retour au dashboard
        </button>
      </div>

      {error && (
        <p className="text-red-500 text-sm mb-4">
          {typeof error === "string" ? error : JSON.stringify(error)}
        </p>
      )}

      {/* Formulaire création */}
      <form action={handleCreate} className="flex gap-3 mb-8">
        <input
          type="text"
          name="name"
          required
          placeholder="Nom"
          className="flex-1 px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold placeholder:text-brown/30 dark:placeholder:text-rose-gold/30 outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
        />
        <input
          type="text"
          name="slug"
          required
          placeholder="Slug"
          className="flex-1 px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold placeholder:text-brown/30 dark:placeholder:text-rose-gold/30 outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
        />
        <button
          type="submit"
          className="px-5 py-2 text-xs tracking-widest uppercase bg-brown dark:bg-rose-gold text-white dark:text-black rounded hover:opacity-90 transition-opacity"
        >
          Ajouter
        </button>
      </form>

      {/* Liste */}
      {loading ? (
        <p className="text-center text-brown/60 dark:text-rose-gold/60 text-sm tracking-widest uppercase py-10">
          Chargement...
        </p>
      ) : categories.length === 0 ? (
        <p className="text-center text-brown/60 dark:text-rose-gold/60 text-sm tracking-widest uppercase py-10">
          Aucune catégorie
        </p>
      ) : (
        <div className="space-y-3">
          {categories.map((cat) => (
            <div key={cat.id}>
              {editingId === cat.id ? (
                <form action={handleUpdate} className="flex gap-3 items-center">
                  <input
                    type="text"
                    name="name"
                    required
                    defaultValue={cat.name}
                    className="flex-1 px-4 py-2 border border-gold dark:border-rose-gold rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none"
                  />
                  <input
                    type="text"
                    name="slug"
                    required
                    defaultValue={cat.slug}
                    className="flex-1 px-4 py-2 border border-gold dark:border-rose-gold rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none"
                  />
                  <button
                    type="submit"
                    className="text-xs tracking-wide text-green-600 dark:text-green-400 hover:opacity-80"
                  >
                    ✓
                  </button>
                  <button
                    type="button"
                    onClick={() => setEditingId(null)}
                    className="text-xs tracking-wide text-brown/50 dark:text-rose-gold/50 hover:opacity-80"
                  >
                    ✕
                  </button>
                </form>
              ) : (
                <div className="flex items-center justify-between px-4 py-3 border border-beige-dark dark:border-zinc-700 rounded">
                  <div>
                    <span className="text-brown dark:text-rose-gold font-medium">
                      {cat.name}
                    </span>
                    <span className="text-brown/40 dark:text-rose-gold/40 text-xs ml-3">
                      /{cat.slug}
                    </span>
                  </div>
                  <div className="flex items-center gap-3">
                    <button
                      onClick={() => setEditingId(cat.id)}
                      className="text-xs tracking-wide text-gold dark:text-rose-gold hover:opacity-80"
                    >
                      Modifier
                    </button>
                    <button
                      onClick={() => handleDelete(cat.id, cat.name)}
                      className="text-base hover:scale-110 transition-transform"
                      title="Supprimer"
                    >
                      ❌
                    </button>
                  </div>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </section>
  );
}
