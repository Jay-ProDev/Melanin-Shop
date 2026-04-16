import { useState, useEffect } from "react";
import { useAtom } from "jotai";
import { tokenAtom, getMemberIdFromToken } from "../store";
import { getMemberById, updateMember } from "../services/memberService";
import {
  getAddress,
  createAddress,
  updateAddress,
} from "../services/addressService";

export default function Profile() {
  const [token] = useAtom(tokenAtom);
  const [member, setMember] = useState(null);
  const [address, setAddress] = useState(null);
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
      const [memberResult, addressResult] = await Promise.all([
        getMemberById(memberId),
        getAddress(),
      ]);
      if (memberResult.success) setMember(memberResult.data);
      if (addressResult.success && addressResult.data)
        setAddress(addressResult.data);
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
      {/* Section Mes informations */}
      <div className="w-full max-w-[440px]">
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
          // Mode lecture
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
          // Mode édition
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
      <div className="w-full max-w-[440px]">
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
          // Mode lecture
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
          // Mode édition
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
  );
}
