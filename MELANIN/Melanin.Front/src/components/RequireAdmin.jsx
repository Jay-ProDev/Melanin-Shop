import { useState, useEffect } from "react";
import { useAtomValue } from "jotai";
import { Navigate } from "react-router";
import { roleAtom } from "../store";

export default function RequireAdmin({ children }) {
  const role = useAtomValue(roleAtom);
  const [hydrated, setHydrated] = useState(false);

  useEffect(() => {
    setHydrated(true);
  }, []);

  // Attend l'hydratation de Jotai avant de juger
  if (!hydrated) {
    return null;
  }

  // Pas admin → redirige vers home
  if (role !== "Admin") {
    return <Navigate to="/" replace />;
  }

  return children;
}
