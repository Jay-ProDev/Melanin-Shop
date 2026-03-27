import { atom, createStore } from "jotai";
import { atomWithStorage } from "jotai/utils";
import { jwtDecode } from "jwt-decode";

export const store = createStore();

export const tokenAtom = atomWithStorage("token", null);

export const roleAtom = atom((get) => {
    const token = get(tokenAtom);
    if (!token) return null;
    try {
        const payload = jwtDecode(token);
        return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    } catch {
        return null;
    }
});