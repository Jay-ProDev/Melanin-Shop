import { useState, useEffect } from "react";
import { Link, useSearchParams, useNavigate } from "react-router";
import { useAtom } from "jotai";
import { tokenAtom, getMemberIdFromToken } from "../store";
import { getMemberById, updateMember } from "../services/memberService";
import {
  getAddress,
  createAddress,
  updateAddress,
} from "../services/addressService";
import { getMyOrders } from "../services/orderService";
import { formatOrderStatus, getOrderStatusStyle } from "../utils/orderStatus";

export default function Profile() {
  const [token] = useAtom(tokenAtom);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const [member, setMember] = useState(null);
  const [address, setAddress] = useState(null);
  const [orders, setOrders] = useState([]);
  const [visibleCount, setVisibleCount] = useState(6);
  const [editingMember, setEditingMember] = useState(false);
  const [editingAddress, setEditingAddress] = useState(false);
  const [memberError, setMemberError] = useState("");
  const [memberSuccess, setMemberSuccess] = useState("");
  const [addressError, setAddressError] = useState("");
  const [addressSuccess, setAddressSuccess] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      const memberId = getMemberIdFromToken(token);
      const [memberResult, addressResult, ordersResult] = await Promise.all([
        getMemberById(memberId),
        getAddress(),
        getMyOrders(),
      ]);
      if (memberResult.success) setMember(memberResult.data);
      if (addressResult.success && addressResult.data)
        setAddress(addressResult.data);
      if (ordersResult.success) setOrders(ordersResult.data);
      setLoading(false);
    };
    fetchData();
  }, [token]);

  const memberAction = async (formData) => {
    setMemberError("");
    setMemberSuccess("");
    const memberId = getMemberIdFromToken(token);
    const result = await updateMember(
      memberId,
      formData.get("firstName"),
      formData.get("lastName"),
      formData.get("email"),
    );
    if (result.success) {
      setMemberSuccess("Informations mises à jour !");
      const updated = await getMemberById(memberId);
      if (updated.success) setMember(updated.data);
      setEditingMember(false);
    } else {
      setMemberError(result.error);
    }
  };

  const addressAction = async (formData) => {
    setAddressError("");
    setAddressSuccess("");
    const addressData = {
      city: formData.get("city"),
      postalCode: formData.get("postalCode"),
      country: formData.get("country"),
      street: formData.get("street"),
      phone: formData.get("phone"),
      fullName: formData.get("fullName") || null,
    };
    let result;
    if (address) {
      result = await updateAddress(address.id, addressData);
    } else {
      result = await createAddress(addressData);
    }
    if (result.success) {
      setAddressSuccess(
        address ? "Adresse mise à jour !" : "Adresse enregistrée !",
      );
      const updated = await getAddress();
      if (updated.success) setAddress(updated.data);
      setEditingAddress(false);

      // Si l'user vient du checkout, on l'y renvoie
      if (searchParams.get("from") === "checkout") {
        navigate("/checkout");
      }
    } else {
      setAddressError(result.error);
    }
  };

  if (loading)
    return (
      <div className="flex justify-center py-28">
        <p className="text-[13px] text-brown-light dark:text-[#777]">
          Chargement...
        </p>
      </div>
    );

  return (
    <div className="flex flex-col items-center py-28 px-8 gap-16">
      {/* ============================================ */}
      {/* Grid 2 colonnes : Mes infos + Mon adresse    */}
      {/* ============================================ */}
      <div className="w-full max-w-[920px] grid grid-cols-1 md:grid-cols-2 gap-16">
        {/* Section Mes informations */}
        <div>
          <div className="flex items-center justify-between mb-2">
            <h2 className="font-serif text-[26px] text-brown-dark dark:text-white">
              Mes informations
            </h2>
            {!editingMember && (
              <button
                onClick={() => {
                  setEditingMember(true);
                  setMemberSuccess("");
                }}
                className="text-[11px] tracking-[2px] hover:opacity-70 cursor-pointer
                  text-brown-light dark:text-[#999]"
              >
                MODIFIER
              </button>
            )}
          </div>
          <p className="text-[13px] mb-8 text-brown-light dark:text-[#777]">
            Vos informations personnelles
          </p>

          {memberError && (
            <p className="text-[13px] mb-4 text-red-500">{memberError}</p>
          )}
          {memberSuccess && (
            <p className="text-[13px] mb-4 text-green-500">{memberSuccess}</p>
          )}

          {!editingMember ? (
            <div className="flex flex-col gap-4">
              <div className="flex gap-4">
                <div className="flex flex-col gap-1 flex-1">
                  <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    PRÉNOM
                  </span>
                  <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                    {member?.firstName}
                  </span>
                </div>
                <div className="flex flex-col gap-1 flex-1">
                  <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    NOM
                  </span>
                  <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                    {member?.lastName}
                  </span>
                </div>
              </div>
              <div className="flex flex-col gap-1">
                <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                  EMAIL
                </span>
                <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                  {member?.email}
                </span>
              </div>
            </div>
          ) : (
            <form action={memberAction} className="flex flex-col gap-4">
              <div className="flex gap-4">
                <div className="flex flex-col gap-1 flex-1">
                  <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    PRÉNOM
                  </label>
                  <input
                    type="text"
                    name="firstName"
                    defaultValue={member?.firstName ?? ""}
                    className="px-4 py-3 text-[14px] border outline-none
                      bg-white border-beige-dark text-brown-dark focus:border-gold
                      dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                  />
                </div>
                <div className="flex flex-col gap-1 flex-1">
                  <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    NOM
                  </label>
                  <input
                    type="text"
                    name="lastName"
                    defaultValue={member?.lastName ?? ""}
                    className="px-4 py-3 text-[14px] border outline-none
                      bg-white border-beige-dark text-brown-dark focus:border-gold
                      dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                  />
                </div>
              </div>
              <div className="flex flex-col gap-1">
                <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                  EMAIL
                </label>
                <input
                  type="email"
                  name="email"
                  defaultValue={member?.email ?? ""}
                  className="px-4 py-3 text-[14px] border outline-none
                    bg-white border-beige-dark text-brown-dark focus:border-gold
                    dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                />
              </div>
              <div className="flex gap-4">
                <button
                  type="submit"
                  className="flex-1 mt-2 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium
                    text-beige bg-brown hover:bg-brown-dark
                    dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark"
                >
                  SAUVEGARDER
                </button>
                <button
                  type="button"
                  onClick={() => {
                    setEditingMember(false);
                    setMemberError("");
                  }}
                  className="flex-1 mt-2 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium border
                    text-brown border-brown hover:bg-beige
                    dark:text-rose-gold dark:border-rose-gold dark:hover:bg-[#1A1A1A]"
                >
                  ANNULER
                </button>
              </div>
            </form>
          )}
        </div>

        {/* Section Mon adresse */}
        <div>
          <div className="flex items-center justify-between mb-2">
            <h2 className="font-serif text-[26px] text-brown-dark dark:text-white">
              Mon adresse
            </h2>
            {!editingAddress && (
              <button
                onClick={() => {
                  setEditingAddress(true);
                  setAddressSuccess("");
                }}
                className="text-[11px] tracking-[2px] hover:opacity-70 cursor-pointer
                  text-brown-light dark:text-[#999]"
              >
                {address ? "MODIFIER" : "AJOUTER"}
              </button>
            )}
          </div>
          <p className="text-[13px] mb-8 text-brown-light dark:text-[#777]">
            Votre adresse de livraison
          </p>

          {addressError && (
            <p className="text-[13px] mb-4 text-red-500">{addressError}</p>
          )}
          {addressSuccess && (
            <p className="text-[13px] mb-4 text-green-500">{addressSuccess}</p>
          )}

          {!editingAddress ? (
            address ? (
              <div className="flex flex-col gap-4">
                <div className="flex flex-col gap-1">
                  <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    RUE ET NUMÉRO
                  </span>
                  <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                    {address.street}
                  </span>
                </div>
                <div className="flex gap-4">
                  <div className="flex flex-col gap-1 flex-1">
                    <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                      VILLE
                    </span>
                    <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                      {address.city}
                    </span>
                  </div>
                  <div className="flex flex-col gap-1 flex-1">
                    <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                      CODE POSTAL
                    </span>
                    <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                      {address.postalCode}
                    </span>
                  </div>
                </div>
                <div className="flex flex-col gap-1">
                  <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    PAYS
                  </span>
                  <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                    {address.country}
                  </span>
                </div>
                <div className="flex flex-col gap-1">
                  <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                    TÉLÉPHONE
                  </span>
                  <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                    {address.phone}
                  </span>
                </div>
                {address.fullName && (
                  <div className="flex flex-col gap-1">
                    <span className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                      NOM COMPLET
                    </span>
                    <span className="py-2 text-[14px] text-brown-dark dark:text-white">
                      {address.fullName}
                    </span>
                  </div>
                )}
              </div>
            ) : (
              <p className="text-[13px] text-brown-light dark:text-[#777]">
                Aucune adresse enregistrée.
              </p>
            )
          ) : (
            <form action={addressAction} className="flex flex-col gap-4">
              <div className="flex flex-col gap-1">
                <label className="text-[11px] tracking-[2px] text-brown-light dark:text-[#999]">
                  RUE ET NUMÉRO
                </label>
                <input
                  type="text"
                  name="street"
                  defaultValue={address?.street ?? ""}
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
                    defaultValue={address?.city ?? ""}
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
                    defaultValue={address?.postalCode ?? ""}
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
                  defaultValue={address?.country ?? ""}
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
                  defaultValue={address?.phone ?? ""}
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
                  defaultValue={address?.fullName ?? ""}
                  className="px-4 py-3 text-[14px] border outline-none
                    bg-white border-beige-dark text-brown-dark focus:border-gold
                    dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white dark:focus:border-rose-gold"
                />
              </div>
              <div className="flex gap-4">
                <button
                  type="submit"
                  className="flex-1 mt-2 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium
                    text-beige bg-brown hover:bg-brown-dark
                    dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark"
                >
                  SAUVEGARDER
                </button>
                <button
                  type="button"
                  onClick={() => {
                    setEditingAddress(false);
                    setAddressError("");
                  }}
                  className="flex-1 mt-2 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium border
                    text-brown border-brown hover:bg-beige
                    dark:text-rose-gold dark:border-rose-gold dark:hover:bg-[#1A1A1A]"
                >
                  ANNULER
                </button>
              </div>
            </form>
          )}
        </div>
      </div>

      {/* ============================================ */}
      {/* Section Mes commandes (pleine largeur)       */}
      {/* ============================================ */}
      <div className="w-full max-w-[920px]">
        <h2 className="font-serif text-[26px] text-brown-dark dark:text-white mb-2">
          Mes commandes
        </h2>
        <p className="text-[13px] mb-8 text-brown-light dark:text-[#777]">
          Historique de vos commandes
        </p>

        {orders.length === 0 ? (
          <div className="flex flex-col items-start gap-4">
            <p className="text-[13px] text-brown-light dark:text-[#777]">
              Aucune commande pour l'instant.
            </p>
            <Link
              to="/shop"
              className="text-[11px] tracking-[2px] hover:opacity-70 text-brown-light dark:text-[#999]"
            >
              DÉCOUVRIR LA BOUTIQUE →
            </Link>
          </div>
        ) : (
          <>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              {orders.slice(0, visibleCount).map((order) => (
                <div
                  key={order.id}
                  className="border border-beige-dark dark:border-[#2A2A2A] p-5 flex flex-col gap-3"
                >
                  {/* En-tête : numéro + badge statut */}
                  <div className="flex justify-between items-center">
                    <p className="font-serif text-[16px] text-brown-dark dark:text-white">
                      Commande #{order.id}
                    </p>
                    <span
                      className={`text-[10px] tracking-[1px] px-2 py-1 ${getOrderStatusStyle(order.status)}`}
                    >
                      {formatOrderStatus(order.status).toUpperCase()}
                    </span>
                  </div>

                  {/* Date */}
                  <p className="text-[12px] text-brown-light dark:text-[#999]">
                    {new Date(order.createdAt).toLocaleDateString("fr-FR")}
                  </p>

                  {/* Articles (3 max + "et X autres") */}
                  <div className="flex flex-col gap-1 pt-2 border-t border-beige-dark dark:border-[#2A2A2A]">
                    {order.orderItems.slice(0, 3).map((item) => (
                      <p
                        key={item.id}
                        className="text-[12px] text-brown-dark dark:text-white"
                      >
                        • {item.productName}{" "}
                        <span className="text-brown-light dark:text-[#999]">
                          × {item.quantity}
                        </span>
                      </p>
                    ))}
                    {order.orderItems.length > 3 && (
                      <p className="text-[12px] text-brown-light dark:text-[#999] italic">
                        et {order.orderItems.length - 3}
                        autre {order.orderItems.length - 3 > 1 && "s"}
                        article {order.orderItems.length - 3 > 1 && "s"}
                      </p>
                    )}
                  </div>

                  {/* Total + lien voir détail */}
                  <div className="flex justify-between items-center pt-2 border-t border-beige-dark dark:border-[#2A2A2A]">
                    <p className="font-serif text-[16px] text-brown-dark dark:text-white">
                      {order.totalPrice.toFixed(2)} €
                    </p>
                    <Link
                      to={`/orders/${order.id}`}
                      className="text-[11px] tracking-[2px] hover:opacity-70 text-brown-light dark:text-[#999]"
                    >
                      VOIR DÉTAIL →
                    </Link>
                  </div>
                </div>
              ))}
            </div>

            {/* Bouton VOIR PLUS */}
            {visibleCount < orders.length && (
              <div className="flex justify-center mt-8">
                <button
                  onClick={() => setVisibleCount((current) => current + 6)}
                  className="px-8 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium border
                    text-brown border-brown hover:bg-beige
                    dark:text-rose-gold dark:border-rose-gold dark:hover:bg-[#1A1A1A]"
                >
                  VOIR PLUS ({orders.length - visibleCount} restante{" "}
                  {orders.length - visibleCount > 1 && "s"})
                </button>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
}
