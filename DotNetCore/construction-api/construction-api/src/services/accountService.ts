export interface UserVmResponse {
  name: string;
  email: string;
}

export async function login(email: string, password: string): Promise<UserVmResponse> {
  const response = await fetch("https://localhost:7036/api/AccountAPI/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ email, password }),
  });

  const data = await response.json();

  if (!response.ok) {
    throw new Error(data.message || "Login failed");
  }

  localStorage.setItem("user", JSON.stringify(data));

  return data;
}

export function logout() {
  localStorage.removeItem("user");
  localStorage.removeItem("selectedSite");
}