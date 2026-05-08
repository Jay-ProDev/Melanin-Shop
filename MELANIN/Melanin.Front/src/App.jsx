import { Route, Routes, Navigate } from "react-router";
import { useAtom } from "jotai";
import { roleAtom } from "./store";
import Navbar from "./components/Navbar";
import Footer from "./components/Footer";
import RequireAuth from "./components/RequireAuth";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Shop from "./pages/Shop";
import ProductDetail from "./pages/ProductDetail";
import Cart from "./pages/Cart";
import Dashboard from "./pages/admin/Dashboard";
import CreateProduct from "./pages/admin/CreateProduct";
import EditProduct from "./pages/admin/EditProduct";
import Categories from "./pages/admin/Categories";
import Profile from "./pages/Profile";
import Checkout from "./pages/Checkout";
import OrderConfirmation from "./pages/OrderConfirmation";
import OrderDetail from "./pages/OrderDetail";
import NotFound from "./pages/NotFound";

function App() {
  const [role] = useAtom(roleAtom);
  return (
    <div className="min-h-screen bg-beige-light dark:bg-[#0A0A0A]">
      <Navbar />
      <main>
        <Routes>
          {/* Routes publiques */}
          <Route index element={<Home />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
          <Route path="shop" element={<Shop />} />
          <Route path="product/:id" element={<ProductDetail />} />
          <Route path="cart" element={<Cart />} />

          {/* Routes admin (protégées par rôle, déjà gérées) */}
          <Route
            path="admin"
            element={role === "Admin" ? <Dashboard /> : <Navigate to="/" />}
          />
          <Route
            path="admin/products/create"
            element={role === "Admin" ? <CreateProduct /> : <Navigate to="/" />}
          />
          <Route
            path="admin/products/edit/:id"
            element={role === "Admin" ? <EditProduct /> : <Navigate to="/" />}
          />
          <Route
            path="admin/categories"
            element={role === "Admin" ? <Categories /> : <Navigate to="/" />}
          />

          {/* Routes protégées (auth requis) */}
          <Route
            path="/profile"
            element={
              <RequireAuth>
                <Profile />
              </RequireAuth>
            }
          />
          <Route
            path="/checkout"
            element={
              <RequireAuth>
                <Checkout />
              </RequireAuth>
            }
          />
          <Route
            path="/order-confirmation/:id"
            element={
              <RequireAuth>
                <OrderConfirmation />
              </RequireAuth>
            }
          />
          <Route
            path="/orders/:id"
            element={
              <RequireAuth>
                <OrderDetail />
              </RequireAuth>
            }
          />

          <Route path="*" element={<NotFound />} />
        </Routes>
      </main>
      <Footer />
    </div>
  );
}

export default App;
