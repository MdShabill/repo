//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916284
// Featching The Games

// import React, { useEffect, useState } from "react";
// import apiClient from "../services/api-client";
// import { Text } from "@chakra-ui/react";

// interface Game {
//   id: number;
//   name: string;
// }

// interface FetchGamesResponse {
//   count: number;
//   results: Game[];
// }

// const GameGrid = () => {
//   const [games, setGames] = useState<Game[]>([]);
//   const [error, setError] = useState("");

//   useEffect(() => {
//     apiClient
//       .get<FetchGamesResponse>("/games")
//       .then((res) => setGames(res.data.results))
//       .catch((err) => setError(err.message));
//   });

//   return (
//     <>
//       {error && <Text>{error}</Text>}
//       <ul>
//         {games.map((game) => (
//           <li key={game.id}>{game.name}</li>
//         ))}
//       </ul>
//     </>
//   );
// };

//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916276
// Creating a Custom Hook For Featching Games

import { SimpleGrid, Text } from "@chakra-ui/react";
import useGames from "../hooks/useGames";
import GameCard from "./GameCard";

const GameGrid = () => {
  const { games, error } = useGames();

  return (
    <>
      {error && <Text>{error}</Text>}
      <SimpleGrid
        columns={{ sm: 1, md: 2, lg: 3, xl: 5 }}
        padding="10px"
        spacing={10}
      >
        {games.map((game) => (
          <GameCard key={game.id} game={game} />
        ))}
      </SimpleGrid>
    </>
  );
};

export default GameGrid;
