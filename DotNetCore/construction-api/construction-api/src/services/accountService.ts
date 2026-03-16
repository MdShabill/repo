export async function login(email: string, password: string) {

  const response = await fetch(
    `https://localhost:7036/api/AccountAPI/login?email=${email}&password=${password}`,
    {
      method: "POST"
    }
  );

  const data = await response.json();

  if (!response.ok) {
    throw new Error(data.message);
  }

  localStorage.setItem("user", JSON.stringify(data));

  return data;
}