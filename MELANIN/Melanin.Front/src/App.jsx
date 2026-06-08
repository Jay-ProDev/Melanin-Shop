import { Route, Routes, Navigate } from "react-router";
import Navbar from "./components/Navbar";
import Footer from "./components/Footer";
import RequireAuth from "./components/RequireAuth";
import RequireAdmin from "./components/RequireAdmin";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Shop from "./pages/Shop";
import ProductDetail from "./pages/ProductDetail";
import Cart from "./pages/Cart";
import Dashboard from "./pages/admin/Dashboard";
import CreateProduct from "./pages/admin/CreateProduct";
import EditProduct from "./pages/admin/EditProduct";
import EditCategories from "./pages/admin/EditCategories";
import AdminOrders from "./pages/admin/AdminOrders";
import AdminOrderDetail from "./pages/admin/AdminOrderDetail";
import Profile from "./pages/Profile";
import Checkout from "./pages/Checkout";
import PaymentSuccess from "./pages/PaymentSuccess";
import PaymentCancel from "./pages/PaymentCancel";
import OrderDetail from "./pages/OrderDetail";
import Livraison from "./pages/legal/Livraison";
import Retours from "./pages/legal/Retours";
import CGV from "./pages/legal/CGV";
import Confidentialite from "./pages/legal/Confidentialite";
import NotFound from "./pages/NotFound";

function App() {
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

          {/* Routes admin protégées (auth requis) */}
          <Route
            path="admin"
            element={
              <RequireAdmin>
                <Dashboard />
              </RequireAdmin>
            }
          />
          <Route
            path="admin/products/create"
            element={
              <RequireAdmin>
                <CreateProduct />
              </RequireAdmin>
            }
          />
          <Route
            path="admin/products/edit/:id"
            element={
              <RequireAdmin>
                <EditProduct />
              </RequireAdmin>
            }
          />
          <Route
            path="admin/categories"
            element={
              <RequireAdmin>
                <EditCategories />
              </RequireAdmin>
            }
          />
          <Route
            path="admin/orders"
            element={
              <RequireAdmin>
                <AdminOrders />
              </RequireAdmin>
            }
          />
          <Route
            path="admin/orders/:id"
            element={
              <RequireAdmin>
                <AdminOrderDetail />
              </RequireAdmin>
            }
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
            path="/payment/success/:id"
            element={
              <RequireAuth>
                <PaymentSuccess />
              </RequireAuth>
            }
          />
          <Route
            path="/payment/cancel/:id"
            element={
              <RequireAuth>
                <PaymentCancel />
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
          
          <Route path="livraison" element={<Livraison />} />
          <Route path="retours" element={<Retours />} />
          <Route path="cgv" element={<CGV />} />
          <Route path="confidentialite" element={<Confidentialite />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </main>
      <Footer />
    </div>
  );
}

export default App;
