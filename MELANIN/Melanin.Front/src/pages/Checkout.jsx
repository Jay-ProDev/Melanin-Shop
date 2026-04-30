import { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router";
import { useAtom } from "jotai";
import { tokenAtom, cartCountAtom, getMemberIdFromToken } from "../store";
import { getCart } from "../services/cartItemService";
import { getAddress, createAddress } from "../services/addressService";
import { createOrder } from "../services/orderService";

export default function Checkout() {
  const [token] = useAtom(tokenAtom);
  const [, setCartCount] = useAtom(cartCountAtom);
  const navigate = useNavigate();

  const [cartItems, setCartItems] = useState([]);
  const [address, setAddress] = useState(null);
  const [useExistingAddress, setUseExistingAddress] = useState(true);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!token) {
      navigate("/login?redirect=/checkout");
      return;
    }

    const loadCheckoutData = async () => {
      const memberId = getMemberIdFromToken(token);
      const [cartResult, addressResult] = await Promise.all([
        getCart(memberId),
        getAddress(),
      ]);

      if (cartResult.success) {
        setCartItems(cartResult.data);
        // Sécurité : si panier vide, retour au panier
        if (cartResult.data.length === 0) {
          navigate("/cart");
          return;
        }
      } else {
        setError(cartResult.error);
      }

      if (addressResult.success && addressResult.data) {
        setAddress(addressResult.data);
      }

      if (!addressResult.success || !addressResult.data) {
        setUseExistingAddress(false);
      }

      setLoading(false);
    };

    loadCheckoutData();
  }, [token, navigate]);

  const checkoutAction = async (formData) => {
    setError("");
    setSubmitting(true);

    let shippingAddressId;

    if (useExistingAddress && address) {
      // Cas A : utilise l'adresse principale existante
      shippingAddressId = address.id;
    } else {
      // Cas B : crée une nouvelle adresse (snapshot ou principale si c'est la 1ère)
      const addressData = {
        city: formData.get("city"),
        postalCode: formData.get("postalCode"),
        country: formData.get("country"),
        street: formData.get("street"),
        phone: formData.get("phone"),
        fullName: formData.get("fullName") || null,
      };
      const addressResult = await createAddress(addressData);
      if (!addressResult.success) {
        setError(addressResult.error);
        setSubmitting(false);
        return;
      }

      shippingAddressId = addressResult.data.id;
    }

    const orderResult = await createOrder(shippingAddressId);
    if (!orderResult.success) {
      setError(orderResult.error);
      setSubmitting(false);
      return;
    }

    setCartCount(0);
    navigate(`/order-confirmation/${orderResult.data.id}`, {
      state: { fromCheckout: true },
    });
  };

  const total = cartItems.reduce(
    (runningTotal, item) => runningTotal + item.unitPrice * item.quantity,
    0,
  );

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
    <section className="max-w-5xl mx-auto px-6 py-16">
      <h1 className="text-3xl font-serif tracking-widest uppercase text-brown-dark dark:text-white text-center mb-12">
        Finaliser ma commande
      </h1>

      {error && (
        <p className="text-[13px] text-center mb-6 text-red-500">{error}</p>
      )}

      <form action={checkoutAction}>
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 mb-12">
          {/* ========== ADRESSE ========== */}
          <div>
            <h2 className="font-serif text-[22px] mb-6 text-brown-dark dark:text-white">
              Adresse de livraison
            </h2>

            {/* Checkbox visible seulement si user a une adresse principale */}
            {address && (
              <label className="flex items-center gap-3 mb-6 cursor-pointer">
                <input
                  type="checkbox"
                  checked={useExistingAddress}
                  onChange={(e) => setUseExistingAddress(e.target.checked)}
                  className="w-4 h-4 cursor-pointer accent-brown dark:accent-rose-gold"
                />
                <span className="text-[13px] text-brown-dark dark:text-white">
                  Utiliser mon adresse enregistrée
                </span>
              </label>
            )}

            {useExistingAddress && address ? (
              // Mode A : affichage de l'adresse principale
              <div className="flex flex-col gap-3">
                {address.fullName && (
                  <p className="text-[14px] text-brown-dark dark:text-white">
                    {address.fullName}
                  </p>
                )}
                <p className="text-[14px] text-brown-dark dark:text-white">
                  {address.street}
                </p>
                <p className="text-[14px] text-brown-dark dark:text-white">
                  {address.postalCode} {address.city}
                </p>
                <p className="text-[14px] text-brown-dark dark:text-white">
                  {address.country}
                </p>
                <p className="text-[14px] text-brown-light dark:text-[#999]">
                  📞 {address.phone}
                </p>
                <Link
                  to="/profile?from=checkout"
                  className="text-[11px] tracking-[2px] mt-2 hover:opacity-70 text-brown-light dark:text-[#999]"
                >
                  MODIFIER L'ADRESSE
                </Link>
              </div>
            ) : (
              // Mode B : formulaire (nouvelle adresse OU pas d'adresse principale)
              <div className="flex flex-col gap-4">
                {!address && (
                  <p className="text-[13px] mb-2 text-brown-light dark:text-[#777]">
                    Aucune adresse enregistrée. Renseignez-la pour finaliser
                    votre commande.
                  </p>
                )}
                {address && (
                  <p className="text-[13px] mb-2 text-brown-light dark:text-[#777]">
                    Livrer à une autre adresse pour cette commande uniquement.
                  </p>
                )}
                <div className="flex flex-col gap-1">
                  <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    RUE ET NUMÉRO
                  </label>
                  <input
                    type="text"
                    name="street"
                    required
                    className="px-4 py-3 text-[14px] border outline-none
                      bg-white border-beige-dark text-brown-dark focus:border-gold
                      dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                  />
                </div>
                <div className="flex gap-4">
                  <div className="flex flex-col gap-1 flex-1">
                    <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                      VILLE
                    </label>
                    <input
                      type="text"
                      name="city"
                      required
                      className="px-4 py-3 text-[14px] border outline-none
                        bg-white border-beige-dark text-brown-dark focus:border-gold
                        dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                    />
                  </div>
                  <div className="flex flex-col gap-1 flex-1">
                    <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                      CODE POSTAL
                    </label>
                    <input
                      type="text"
                      name="postalCode"
                      required
                      className="px-4 py-3 text-[14px] border outline-none
                        bg-white border-beige-dark text-brown-dark focus:border-gold
                        dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                    />
                  </div>
                </div>
                <div className="flex flex-col gap-1">
                  <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    PAYS
                  </label>
                  <input
                    type="text"
                    name="country"
                    required
                    className="px-4 py-3 text-[14px] border outline-none
                      bg-white border-beige-dark text-brown-dark focus:border-gold
                      dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                  />
                </div>
                <div className="flex flex-col gap-1">
                  <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    TÉLÉPHONE
                  </label>
                  <input
                    type="text"
                    name="phone"
                    required
                    className="px-4 py-3 text-[14px] border outline-none
                      bg-white border-beige-dark text-brown-dark focus:border-gold
                      dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                  />
                </div>
                <div className="flex flex-col gap-1">
                  <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    NOM COMPLET (optionnel)
                  </label>
                  <input
                    type="text"
                    name="fullName"
                    className="px-4 py-3 text-[14px] border outline-none
                      bg-white border-beige-dark text-brown-dark focus:border-gold
                      dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                  />
                </div>
              </div>
            )}
          </div>

          {/* ========== RÉCAPITULATIF ========== */}
          <div>
            <h2 className="font-serif text-[22px] mb-6 text-brown-dark dark:text-white">
              Récapitulatif
            </h2>
            <div className="flex flex-col gap-4">
              {cartItems.map((item) => (
                <div
                  key={item.id}
                  className="flex justify-between items-start gap-4 pb-4 border-b border-beige-dark dark:border-[#2A2A2A]"
                >
                  <div className="flex-1">
                    <p className="text-[14px] text-brown-dark dark:text-white">
                      {item.productName}
                    </p>
                    <p className="text-[12px] text-brown-light dark:text-[#999] mt-1">
                      Quantité : {item.quantity} × {item.unitPrice.toFixed(2)} €
                    </p>
                  </div>
                  <p className="text-[14px] font-medium text-brown-dark dark:text-white">
                    {(item.unitPrice * item.quantity).toFixed(2)} €
                  </p>
                </div>
              ))}
              <div className="flex justify-between items-center pt-2">
                <p className="text-[14px] tracking-[2px] uppercase text-brown-dark dark:text-white">
                  Total
                </p>
                <p className="font-serif text-[22px] text-brown-dark dark:text-white">
                  {total.toFixed(2)} €
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* ========== BOUTON CONFIRMER ========== */}
        <div className="flex justify-center">
          <button
            type="submit"
            disabled={submitting}
            className="px-12 py-4 text-[12px] tracking-[2px] cursor-pointer font-medium
              text-beige bg-brown hover:bg-brown-dark
              dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark
              disabled:opacity-40 disabled:cursor-not-allowed"
          >
            {submitting
              ? "TRAITEMENT..."
              : `CONFIRMER MA COMMANDE - ${total.toFixed(2)} €`}
          </button>
        </div>
      </form>
    </section>
  );
}
