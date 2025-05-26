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

// import { SimpleGrid, Text } from "@chakra-ui/react";
// import useGames from "../hooks/useGames";
// import GameCard from "./GameCard";

// const GameGrid = () => {
//   const { games, error } = useGames();

//   return (
//     <>
//       {error && <Text>{error}</Text>}
//       <SimpleGrid
//         columns={{ sm: 1, md: 2, lg: 3, xl: 5 }}
//         padding="10px"
//         spacing={10}
//       >
//         {games.map((game) => (
//           <GameCard key={game.id} game={game} />
//         ))}
//       </SimpleGrid>
//     </>
//   );
// };

//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916329
// Improving User Experience With Loading Skeletons

import { SimpleGrid, Text } from "@chakra-ui/react";
import useGames from "../hooks/useGames";
import GameCard from "./GameCard";
import GameCardSkeleton from "./GameCardSkeleton";
import GameCardContainer from "./GameCardContainer";
import { GameQuery } from "../App";

interface Props {
  gameQuery: GameQuery;
}

const GameGrid = ({ gameQuery }: Props) => {
  const { data, error, isLoading } = useGames(gameQuery);
  const skeletons = [1, 2, 3, 4, 5, 6];

  return (
    <>
      {error && <Text>{error}</Text>}
      <SimpleGrid
        columns={{ sm: 1, md: 2, lg: 3, xl: 5 }}
        padding="10px"
        spacing={3}
      >
        {isLoading &&
          skeletons.map((Skeleton) => (
            <GameCardContainer key={Skeleton}>
              <GameCardSkeleton />
            </GameCardContainer>
          ))}
        {data.map((game) => (
          <GameCardContainer key={game.id}>
            <GameCard game={game} />
          </GameCardContainer>
        ))}
      </SimpleGrid>
    </>
  );
};

export default GameGrid;
