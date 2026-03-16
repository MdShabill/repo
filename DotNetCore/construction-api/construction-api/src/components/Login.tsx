import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../services/accountService";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e: any) => {
    e.preventDefault();

    try {
      await login(email, password);

      navigate("/"); // login success → home page
    } catch (error: any) {
      alert(error.message);
    }
  };

  return (
    <div className="container mt-5">
      <h3>Login</h3>

      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <input
            type="email"
            placeholder="Enter Email"
            className="form-control"
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <input
            type="password"
            placeholder="Enter Password"
            className="form-control"
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>

        <button className="btn btn-primary">Login</button>
      </form>
    </div>
  );
}

export default Login;
