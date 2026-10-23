import { API_URLS } from "../constants/urls";

export interface UserVmResponse {
  fullName: string;
  email: string;
}

export async function login(
  email: string,
  password: string
): Promise<UserVmResponse> {

  const response = await fetch(API_URLS.account.login, {
  method: "POST",
  headers: {
    "Content-Type": "application/json",
  },
  body: JSON.stringify({ email, password }),
});

  const data =
    await response.json();

  if (!response.ok) {
    throw new Error(
      data.message ||
      "Invalid Email Or Password"
    );
  }

  localStorage.setItem(
  "user",
    JSON.stringify({
      fullName: data.fullName,
      email: data.email,
    })
  );

  return data;
}

export function logout() {
  localStorage.removeItem("user");
  localStorage.removeItem("selectedSite");
}