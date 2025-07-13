//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915914
// Extracting the user service

// import apiClient from "./api-client";

// export interface User {
//   id: number;
//   name: string;
// }

// class UserService {
//     getAllUsers() {
//         const controller = new AbortController();
//         const request = apiClient.get<User[]>("/users", {
//             signal: controller.signal,
//         });
//         return { request, cancel: () => controller.abort() }
//     }

//     deleteUser(id: number) {
//         return apiClient.delete("/users" + id)
//     }

//     createUser(user: User) {
//         return apiClient.post("/users", user)
//     }

//     updateUser(user: User) {
//         return apiClient.patch("/users/" + user.id, user)
//     }
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915918
// Creating a Generic HTTP service

import create from "./http-service";

export interface User {
  id: number;
  name: string;
}



export default create('/users');