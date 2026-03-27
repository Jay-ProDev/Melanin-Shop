/*
throw new Error('Ce code n\'est plus utilisé !!!')


import { createContext, useContext, useState } from "react";

// 1. On crée la boîte vide
const AuthContext = createContext();

// 2. Le composant qui va envelopper toute l'app
//    "children" = tout ce qui est à l'intérieur (tes pages, ta navbar...)
export function AuthProvider({ children }) {

    // 3. Le token est stocké dans le state
    //    Au démarrage, on regarde si un token existe déjà dans le navigateur
    const [token, setToken] = useState(localStorage.getItem("token"));

    // 4. Fonction appelée après le login
    //    Elle sauvegarde le token à 2 endroits :
    //    - localStorage = survit au refresh de la page
    //    - setToken = met à jour l'interface React
    const saveToken = (newToken) => {
        localStorage.setItem("token", newToken);
        setToken(newToken);
    };

    // 5. Fonction de déconnexion
    //    Supprime le token partout
    const logout = () => {
        localStorage.removeItem("token");
        setToken(null);
    };

    // 6. Lire le rôle depuis le token
    //    Un JWT c'est 3 parties séparées par des points : header.payload.signature
    //    Le payload contient le rôle, on le décode
    const getRole = () => {
        if (!token) return null;
        try {
            const payload = JSON.parse(atob(token.split(".")[1]));
            return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        } catch {
            return null;
        }
    };

    // 7. Raccourcis pratiques
    const isAuthenticated = !!token;  // true si token existe, false sinon
    const role = getRole();

    // 8. On rend tout accessible aux enfants via value={...}
    return (
        <AuthContext.Provider value={{ token, saveToken, logout, isAuthenticated, role }}>
            {children}
        </AuthContext.Provider>
    );
}

// 9. Le hook pour utiliser le contexte dans n'importe quel composant
//    Exemple : const { isAuthenticated, role, logout } = useAuth();
export const useAuth = () => useContext(AuthContext);


*/