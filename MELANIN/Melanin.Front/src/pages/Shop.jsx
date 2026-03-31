import { useState, useEffect } from "react";
import { getAllActiveProducts } from "../services/productService";
import { getAllCategories } from "../services/categoryService";
import ProductCard from "../components/ProductCard";

export default function Shop() {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedCategories, setSelectedCategories] = useState([]);
  const [searchKeyword, setSearchKeyword] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchData() {
      setLoading(true);
      setError(null);

      const [catResult, prodResult] = await Promise.all([
        getAllCategories(),
        getAllActiveProducts(),
      ]);

      if (!catResult.success) {
        setError(catResult.error);
        setLoading(false);
        return;
      }
      if (!prodResult.success) {
        setError(prodResult.error);
        setLoading(false);
        return;
      }

      setCategories(catResult.data);
      setProducts(prodResult.data);
      setLoading(false);
    }

    fetchData();
  }, []);

  function toggleCategory(categoryId) {
    setSelectedCategories((prev) =>
      prev.includes(categoryId)
        ? prev.filter((id) => id !== categoryId)
        : [...prev, categoryId],
    );
  }

  const filteredProducts = products.filter((p) => {
    const matchCategory =
      selectedCategories.length === 0 ||
      selectedCategories.includes(p.categoryId);

    const matchSearch =
      !searchKeyword.trim() ||
      p.name.toLowerCase().includes(searchKeyword.toLowerCase()) ||
      p.description.toLowerCase().includes(searchKeyword.toLowerCase());

    return matchCategory && matchSearch;
  });

  return (
    <section className="max-w-7xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-serif tracking-widest uppercase text-brown dark:text-rose-gold text-center mb-10">
        Boutique
      </h1>

      <div className="max-w-md mx-auto mb-10">
        <input
          type="text"
          placeholder="Rechercher un produit..."
          value={searchKeyword}
          onChange={(e) => setSearchKeyword(e.target.value)}
          className="w-full px-4 py-2 border border-beige-dark dark:border-zinc-700 rounded bg-white dark:bg-zinc-900 text-brown dark:text-rose-gold placeholder:text-brown/40 dark:placeholder:text-rose-gold/40 outline-none focus:border-gold dark:focus:border-rose-gold transition-colors"
        />
      </div>

      <div className="flex gap-10">
        <aside className="w-48 shrink-0">
          <h2 className="font-serif text-sm tracking-widest uppercase text-brown dark:text-rose-gold mb-4">
            Catégories
          </h2>
          {categories.length === 0 && !loading ? (
            <p className="text-sm text-brown/40 dark:text-rose-gold/40">
              Aucune catégorie
            </p>
          ) : (
            <div className="space-y-2">
              {categories.map((cat) => (
                <label
                  key={cat.id}
                  className="flex items-center gap-2 cursor-pointer text-sm text-brown/80 dark:text-rose-gold/80 hover:text-brown dark:hover:text-rose-gold transition-colors"
                >
                  <input
                    type="checkbox"
                    checked={selectedCategories.includes(cat.id)}
                    onChange={() => toggleCategory(cat.id)}
                    className="accent-gold dark:accent-rose-gold"
                  />
                  {cat.name}
                </label>
              ))}
            </div>
          )}
        </aside>

        <div className="flex-1">
          {loading ? (
            <p className="text-center text-brown/60 dark:text-rose-gold/60 text-sm tracking-widest uppercase py-20">
              Chargement...
            </p>
          ) : error ? (
            <p className="text-center text-red-500 text-sm py-20">{error}</p>
          ) : filteredProducts.length === 0 ? (
            <p className="text-center text-brown/60 dark:text-rose-gold/60 text-sm tracking-widest uppercase py-20">
              Aucun produit trouvé
            </p>
          ) : (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
              {filteredProducts.map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          )}
        </div>
      </div>
    </section>
  );
}
