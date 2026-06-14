export async function login(
  email: string,
  password: string
) {
  const response =
    await fetch(
      "https://localhost:7036/api/AccountAPI/login",
      {
        method: "POST",

        headers: {
          "Content-Type":
            "application/json"
        },

        body:
          JSON.stringify({
            email,
            password
          })
      }
    );

  const data =
    await response.json();

  if (!response.ok) {
    throw new Error(
      data.message
    );
  }

  localStorage.setItem(
    "user",
    JSON.stringify(data)
  );

  return data;
}