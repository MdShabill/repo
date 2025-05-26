//---------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916282
// Installing chakar Ui

// import { Button, ButtonGroup } from "@chakra-ui/react";

// function App() {
//   return <Button colorScheme="blue">Button</Button>;
// }

//---------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916285
// Creating a Responsive Layout

// import { Grid, GridItem, Show } from "@chakra-ui/react";
// import NavBar from "./components/NavBar";

// function App() {
//   return (
//     <Grid
//       templateAreas={{
//         base: `"nav" "main"`,
//         lg: `"nav nav" "aside main"`,
//       }}
//     >
//       <GridItem area="nav">
//         <NavBar />
//       </GridItem>
//       <Show above="lg">
//         <GridItem area="aside" bg="gold">
//           Nav
//         </GridItem>
//       </Show>
//       <GridItem area="main" bg="dodgerblue">
//         Main
//       </GridItem>
//     </Grid>
//   );
// }

//---------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916281
// Building the Color Mode Switch

// import { Grid, GridItem, Show } from "@chakra-ui/react";
// import NavBar from "./components/NavBar";
// import GameGrid from "./components/GameGrid";
// import GenreList from "./components/GenreList";

// function App() {
//   return (
//     <Grid
//       templateAreas={{
//         base: `"nav" "main"`,
//         lg: `"nav nav" "aside main"`,
//       }}
//     >
//       <GridItem area="nav">
//         <NavBar />
//       </GridItem>
//       <Show above="lg">
//         <GridItem area="aside">
//           <GenreList />
//         </GridItem>
//       </Show>
//       <GridItem area="main">
//         <GameGrid />
//       </GridItem>
//     </Grid>
//   );
// }

//---------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916280
// Building the Navigation Bar

import { Grid, GridItem, Show } from "@chakra-ui/react";
import NavBar from "./components/NavBar";
import GameGrid from "./components/GameGrid";
import GenreList from "./components/GenreList";
import { useState } from "react";
import { Genre } from "./hooks/useGenres";
import { Platform } from "./hooks/useGames";
import PlatformSelector from "./components/PlatformSelector";

export interface GameQuery {
  genre: Genre | null;
  platform: Platform | null;
}

function App() {
  const [gameQuery, setGameQuery] = useState<GameQuery>({} as GameQuery);

  return (
    <Grid
      templateAreas={{
        base: `"nav" "main"`,
        lg: `"nav nav" "aside main"`,
      }}
      templateColumns={{
        base: "1fr",
        lg: "200px 1fr",
      }}
    >
      <GridItem area="nav">
        <NavBar />
      </GridItem>
      <Show above="lg">
        <GridItem area="aside" paddingX={5}>
          <GenreList
            selectedGenre={gameQuery.genre}
            onSelectedGenre={(genre) => setGameQuery({ ...gameQuery, genre })}
          />
        </GridItem>
      </Show>
      <GridItem area="main">
        <PlatformSelector
          selectedPlatform={gameQuery.platform}
          onSelectPlatform={(platform) =>
            setGameQuery({ ...gameQuery, platform })
          }
        />
        <GameGrid gameQuery={gameQuery} />
      </GridItem>
    </Grid>
  );
}

export default App;
