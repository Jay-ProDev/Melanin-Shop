import { useState } from "react";
import { useNavigate } from "react-router";
import { authRegister } from "../services/memberService";

export default function Register() {
  const navigate = useNavigate();
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const registerAction = async (formData) => {
    if (isSubmitting) return;
    setError("");
    setIsSubmitting(true);
    const result = await authRegister(
      formData.get("firstName"),
      formData.get("lastName"),
      formData.get("email"),
      formData.get("password"),
    );

    if (result.success) {
      navigate("/login");
      setIsSubmitting(false);
    } else {
      setError(result.error);
      setIsSubmitting(false);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center py-28 px-8">
      <h1
        className="font-serif text-[32px] mb-2
                text-brown-dark dark:text-white"
      >
        Inscription
      </h1>
      <p
        className="text-[13px] mb-8
                text-brown-light dark:text-[#777]"
      >
        Rejoignez l'univers Melanin
      </p>

      {error && <p className="text-[13px] mb-4 text-red-500">{error}</p>}

      <form
        action={registerAction}
        className="flex flex-col gap-4 w-full max-w-[440px]"
      >
        <div className="flex gap-4">
          <div className="flex flex-col gap-1 flex-1">
            <label
              htmlFor="reg-firstName"
              className="text-[11px] tracking-[2px]
                                text-brown-light dark:text-[#999]"
            >
              PRÉNOM
            </label>
            <input
              id="reg-firstName"
              type="text"
              name="firstName"
              className="px-4 py-3 text-[14px] border outline-none
                                bg-white border-beige-dark text-brown-dark
                                focus:border-gold
                                dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white
                                dark:focus:border-rose-gold"
            />
          </div>
          <div className="flex flex-col gap-1 flex-1">
            <label
              htmlFor="reg-lastName"
              className="text-[11px] tracking-[2px]
                                text-brown-light dark:text-[#999]"
            >
              NOM
            </label>
            <input
              id="reg-lastName"
              type="text"
              name="lastName"
              className="px-4 py-3 text-[14px] border outline-none
                                bg-white border-beige-dark text-brown-dark
                                focus:border-gold
                                dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white
                                dark:focus:border-rose-gold"
            />
          </div>
        </div>
        <div className="flex flex-col gap-1">
          <label
            htmlFor="reg-email"
            className="text-[11px] tracking-[2px]
                            text-brown-light dark:text-[#999]"
          >
            EMAIL
          </label>
          <input
            id="reg-email"
            type="email"
            name="email"
            className="px-4 py-3 text-[14px] border outline-none
                            bg-white border-beige-dark text-brown-dark
                            focus:border-gold
                            dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white
                            dark:focus:border-rose-gold"
          />
        </div>
        <div className="flex flex-col gap-1">
          <label
            htmlFor="reg-pwd"
            className="text-[11px] tracking-[2px]
                            text-brown-light dark:text-[#999]"
          >
            MOT DE PASSE
          </label>
          <input
            id="reg-pwd"
            type="password"
            name="password"
            className="px-4 py-3 text-[14px] border outline-none
                            bg-white border-beige-dark text-brown-dark
                            focus:border-gold
                            dark:bg-[#111] dark:border-[#2A2A2A] dark:text-white
                            dark:focus:border-rose-gold"
          />
        </div>
        <button
          type="submit"
          disabled={isSubmitting}
          className="mt-2 py-3 text-[12px] tracking-[2px] cursor-pointer font-medium 
                        text-beige bg-brown hover:bg-brown-dark
                        dark:text-[#0A0A0A] dark:bg-rose-gold dark:hover:bg-rose-gold-dark"
        >
          {isSubmitting ? "INSCRIPTION EN COURS..." : "S'INSCRIRE"}
        </button>
      </form>
    </div>
  );
}
