import { useState, useEffect } from "react";
import { useNavigate } from "react-router";
import { createProduct } from "../../services/productService";
import { getAllCategories } from "../../services/categoryService";

const HAIR_LENGTHS = [
  "Inches8",
  "Inches10",
  "Inches12",
  "Inches14",
  "Inches16",
  "Inches18",
  "Inches20",
  "Inches22",
  "Inches24",
  "Inches26",
  "Inches28",
  "Inches30",
];

const HAIR_TEXTURES = [
  "Straight",
  "Wavy",
  "Curly",
  "Kinky",
  "DeepWave",
  "BodyWave",
  "LooseWave",
];

const CAP_SIZES = ["Small", "Medium", "Large", "XLarge"];

export default function CreateProduct() {
  const navigate = useNavigate();
  const [categories, setCategories] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    async function fetchCategories() {
      const result = await getAllCategories();
      if (result.success) setCategories(result.data);
    }
    fetchCategories();
  }, []);

  async function handleSubmit(formData) {
    setError(null);
    setLoading(true);

    const productData = {
      name: formData.get("name"),
      description: formData.get("description"),
      unitPrice: parseFloat(formData.get("unitPrice")),
      stockQuantity: parseInt(formData.get("stockQuantity")),
      categoryId: parseInt(formData.get("categoryId")),
      hairColor: formData.get("hairColor") || null,
      hairLength: formData.get("hairLength") || null,
      hairTexture: formData.get("hairTexture") || null,
      capSize: formData.get("capSize") || null,
    };

    const result = await createProduct(productData);
    setLoading(false);

    if (result.success) {
      navigate("/admin");
    } else {
      setError(result.error);
    }
  }

  return (
    <section className="max-w-2xl mx-auto px-6 py-10">
      <h1 className="text-2xl font-serif tracking-widest uppercase text-brown dark:text-rose-gold mb-8">
        Nouveau produit
      </h1>

      {error && (
        <p className="text-red-500 text-sm mb-4">
          {typeof error === "string" ? error : JSON.stringify(error)}
        </p>
      )}

      <form action={handleSubmit} className="space-y-5">
        {/* Nom */}
        <div>
          <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
            Nom
          </label>
          <input
            type="text"
            name="name"
            required
            className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
          />
        </div>

        {/* Description */}
        <div>
          <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
            Description
          </label>
          <textarea
            name="description"
            required
            rows={3}
            className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none focus:border-gold dark:focus:border-rose-gold transition-colors resize-none"
          />
        </div>

        {/* Prix + Stock sur la même ligne */}
        <div className="flex gap-4">
          <div className="flex-1">
            <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
              Prix (€)
            </label>
            <input
              type="number"
              name="unitPrice"
              required
              min="0.01"
              step="0.01"
              className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
            />
          </div>
          <div className="flex-1">
            <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
              Stock
            </label>
            <input
              type="number"
              name="stockQuantity"
              required
              min="0"
              className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
            />
          </div>
        </div>

        {/* Catégorie */}
        <div>
          <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
            Catégorie
          </label>
          <select
            name="categoryId"
            required
            className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
          >
            <option value="">-- Choisir --</option>
            {categories.map((cat) => (
              <option key={cat.id} value={cat.id}>
                {cat.name}
              </option>
            ))}
          </select>
        </div>

        {/* Attributs cheveux — optionnels */}
        <p className="text-xs tracking-widest uppercase text-brown/40 dark:text-rose-gold/40 pt-2">
          Attributs cheveux (optionnel)
        </p>

        {/* Couleur */}
        <div>
          <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
            Couleur
          </label>
          <input
            type="text"
            name="hairColor"
            placeholder="Ex : Noir Naturel, Brun Foncé..."
            className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold placeholder:text-brown/30 dark:placeholder:text-rose-gold/30 outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
          />
        </div>

        {/* Longueur + Texture sur la même ligne */}
        <div className="flex gap-4">
          <div className="flex-1">
            <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
              Longueur
            </label>
            <select
              name="hairLength"
              className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
            >
              <option value="">-- Aucune --</option>
              {HAIR_LENGTHS.map((l) => (
                <option key={l} value={l}>
                  {l}
                </option>
              ))}
            </select>
          </div>
          <div className="flex-1">
            <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
              Texture
            </label>
            <select
              name="hairTexture"
              className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
            >
              <option value="">-- Aucune --</option>
              {HAIR_TEXTURES.map((t) => (
                <option key={t} value={t}>
                  {t}
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Taille bonnet */}
        <div>
          <label className="block text-xs tracking-widest uppercase text-brown/60 dark:text-rose-gold/60 mb-1">
            Taille bonnet
          </label>
          <select
            name="capSize"
            className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
          >
            <option value="">-- Aucune --</option>
            {CAP_SIZES.map((s) => (
              <option key={s} value={s}>
                {s}
              </option>
            ))}
          </select>
        </div>

        {/* Boutons */}
        <div className="flex gap-4 pt-4">
          <button
            type="submit"
            disabled={loading}
            className="flex-1 py-3 text-xs tracking-widest uppercase bg-brown dark:bg-rose-gold text-white dark:text-black rounded hover:opacity-90 transition-opacity disabled:opacity-50"
          >
            {loading ? "Création..." : "Créer le produit"}
          </button>
          <button
            type="button"
            onClick={() => navigate("/admin")}
            className="px-6 py-3 text-xs tracking-widest uppercase border border-brown dark:border-rose-gold text-brown dark:text-rose-gold rounded hover:opacity-80 transition-opacity"
          >
            Annuler
          </button>
        </div>
      </form>
    </section>
  );
}
