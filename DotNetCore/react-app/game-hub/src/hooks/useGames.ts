//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916276
// Creating a Custom Hook For Featching Games

// import { useEffect, useState } from "react";
// import apiClient from "../services/api-client";
// import { CanceledError } from "axios";

// export interface Game {
//   background_image: string | undefined;
//   id: number;
//   name: string;
//   background: string;
// }

// interface FetchGamesResponse {
//   count: number;
//   results: Game[];
// }

// const useGames = () => {
//     const [games, setGames] = useState<Game[]>([]);
//       const [error, setError] = useState("");
    
//     useEffect(() => {
//       const controller = new AbortController();
//       apiClient
//         .get<FetchGamesResponse>("/games", {signal: controller.signal })
//         .then((res) => setGames(res.data.results))
//     .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.message)
//     });

//       return () => controller.abort();
//     }, []);
//     return { games, error};
// }

//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916325
// Displaying Platform Icon

import { useEffect, useState } from "react";
import apiClient from "../services/api-client";
import { CanceledError } from "axios";

export interface Platform {
    id: number;
    name: string;
    slug: string;
}

export interface Game {
  background_image: string | undefined;
  id: number;
  name: string;
  background: string;
  parent_platforms: { platform: Platform } []
}

interface FetchGamesResponse {
  count: number;
  results: Game[];
}

const useGames = () => {
    const [games, setGames] = useState<Game[]>([]);
      const [error, setError] = useState("");
    
    useEffect(() => {
      const controller = new AbortController();
      apiClient
        .get<FetchGamesResponse>("/games", {signal: controller.signal })
        .then((res) => setGames(res.data.results))
    .catch((err) => {
        if (err instanceof CanceledError) return;
        setError(err.message)
    });

      return () => controller.abort();
    }, []);
    return { games, error};
}

export default useGames;