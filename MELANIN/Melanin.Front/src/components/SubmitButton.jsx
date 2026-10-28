import { useFormStatus } from "react-dom";

/* Bloque le bouton pendant l'envoi du formulaire pour éviter le double clic
 (double connexion, double commande). useFormStatus doit être utilisé dans un composant enfant du <form>. */
 
export default function SubmitButton({ children, pendingText, className }) {
  const { pending } = useFormStatus();

  return (
    <button type="submit" disabled={pending} className={className}>
      {pending ? pendingText : children}
    </button>
  );
}