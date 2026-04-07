import { useState, useEffect } from "react";
import { useParams, Link } from "react-router";
import { getProductById } from "../services/productService";

export default function ProductDetail() {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchProduct() {
      setLoading(true);
      setError(null);

      const result = await getProductById(id);
      if (result.success) {
        setProduct(result.data);
      } else {
        setError(result.error);
      }
      setLoading(false);
    }

    fetchProduct();
  }, [id]);

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
          to="/shop"
          className="text-sm text-gold dark:text-rose-gold underline"
        >
          Retour à la boutique
        </Link>
      </div>
    );
  }

  return (
    <section className="max-w-5xl mx-auto px-6 py-10">
      {/* Bouton retour */}
      <Link
        to="/shop"
        className="inline-block mb-8 text-sm text-brown/60 dark:text-rose-gold/60 hover:text-brown dark:hover:text-rose-gold transition-colors"
      >
        ← Retour à la boutique
      </Link>

      <div className="flex flex-col md:flex-row gap-10">
        {/* Image */}
        <div className="md:w-1/2 aspect-square bg-beige dark:bg-zinc-800 rounded-lg flex items-center justify-center overflow-hidden">
          {product.imageUrl ? (
            <img
              src={`${import.meta.env.VITE_API_URL.replace("/api", "")}${product.imageUrl}`}
              alt={product.name}
              className="w-full h-full object-cover"
            />
          ) : (
            <span className="text-brown/40 dark:text-rose-gold/40 text-sm tracking-widest uppercase">
              Photo
            </span>
          )}
        </div>

        {/* Infos produit */}
        <div className="md:w-1/2 space-y-6">
          {/* Catégorie */}
          <p className="text-xs tracking-widest uppercase text-gold dark:text-gold-light">
            {product.categoryName}
          </p>

          {/* Nom */}
          <h1 className="text-2xl font-serif tracking-widest uppercase text-brown dark:text-rose-gold">
            {product.name}
          </h1>

          {/* Prix */}
          <p className="text-xl font-semibold text-gold dark:text-gold-light">
            {product.unitPrice.toFixed(2)} €
          </p>

          {/* Description */}
          <p className="text-sm text-brown/70 dark:text-rose-gold/70 leading-relaxed">
            {product.description}
          </p>

          {/* Attributs cheveux */}
          {(product.hairColor ||
            product.hairLength ||
            product.hairTexture ||
            product.capSize) && (
            <div className="space-y-2 border-t border-beige-dark dark:border-zinc-700 pt-4">
              {product.hairColor && (
                <div className="flex justify-between text-sm">
                  <span className="text-brown/50 dark:text-rose-gold/50">
                    Couleur
                  </span>
                  <span className="text-brown dark:text-rose-gold">
                    {product.hairColor}
                  </span>
                </div>
              )}
              {product.hairLength && (
                <div className="flex justify-between text-sm">
                  <span className="text-brown/50 dark:text-rose-gold/50">
                    Longueur
                  </span>
                  <span className="text-brown dark:text-rose-gold">
                    {product.hairLength}
                  </span>
                </div>
              )}
              {product.hairTexture && (
                <div className="flex justify-between text-sm">
                  <span className="text-brown/50 dark:text-rose-gold/50">
                    Texture
                  </span>
                  <span className="text-brown dark:text-rose-gold">
                    {product.hairTexture}
                  </span>
                </div>
              )}
              {product.capSize && (
                <div className="flex justify-between text-sm">
                  <span className="text-brown/50 dark:text-rose-gold/50">
                    Taille bonnet
                  </span>
                  <span className="text-brown dark:text-rose-gold">
                    {product.capSize}
                  </span>
                </div>
              )}
            </div>
          )}

          {/* Stock */}
          <p
            className={`text-xs tracking-wide ${product.stockQuantity > 0 ? "text-green-600 dark:text-green-400" : "text-red-500"}`}
          >
            {product.stockQuantity > 0 ? "En stock" : "Rupture de stock"}
          </p>

          {/* Bouton panier */}
          <button
            onClick={() => {
              // TODO : logique panier (localStorage)
            }}
            disabled={product.stockQuantity === 0}
            className="w-full py-3 text-xs tracking-widest uppercase bg-brown dark:bg-rose-gold text-white dark:text-black rounded hover:opacity-90 transition-opacity disabled:opacity-40 disabled:cursor-not-allowed"
          >
            Ajouter au panier
          </button>
        </div>
      </div>
    </section>
  );
}
