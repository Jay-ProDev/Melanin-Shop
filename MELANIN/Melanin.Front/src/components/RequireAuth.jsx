import { useState, useEffect } from "react";
import { useAtomValue } from "jotai";
import { Navigate, useLocation } from "react-router";
import { tokenAtom } from "../store";

export default function RequireAuth({ children }) {
  const token = useAtomValue(tokenAtom);
  const location = useLocation();
  const [hydrated, setHydrated] = useState(false);

  useEffect(() => {
    setHydrated(true);
  }, []);

  if (!hydrated) {
    return null;
  }

  if (!token) {
    return <Navigate to={`/login?redirect=${location.pathname}`} replace />;
  }

  return children;
}
