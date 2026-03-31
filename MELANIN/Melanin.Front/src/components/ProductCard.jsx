import { Link } from "react-router";

export default function ProductCard({ product }) {
  return (
    <Link
      to={`/product/${product.id}`}
      className="group block bg-white dark:bg-zinc-900 border border-beige-dark dark:border-zinc-800 rounded-lg overflow-hidden hover:shadow-lg transition-shadow"
    >
      {/* Image placeholder */}
      <div className="aspect-square bg-beige dark:bg-zinc-800 flex items-center justify-center">
        <span className="text-brown/40 dark:text-rose-gold/40 text-sm tracking-widest uppercase">
          Photo
        </span>
      </div>

      {/* Infos */}
      <div className="p-4 space-y-2">
        <h3 className="text-brown dark:text-rose-gold font-serif text-sm tracking-wide uppercase truncate">
          {product.name}
        </h3>

        <p className="text-gold dark:text-gold-light font-semibold">
          {product.unitPrice.toFixed(2)} €
        </p>

        <button
          onClick={(e) => {
            e.preventDefault();
            // TODO : logique panier (localStorage)
          }}
          className="w-full py-2 text-xs tracking-widest uppercase bg-brown dark:bg-rose-gold text-white dark:text-black rounded hover:opacity-90 transition-opacity"
        >
          Ajouter au panier
        </button>
      </div>
    </Link>
  );
}
