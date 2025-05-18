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

// function App() {
//   return (
//     <Grid
//       templateAreas={{
//         base: `"nav" "main"`,
//         lg: `"nav nav" "aside main"`,
//       }}
//     >
//       <GridItem area="nav" bg="coral">
//         Nav
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
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916280
// Building the Navigation Bar

// import { Grid, GridItem, Show } from "@chakra-ui/react";
// import NavBar from "./componets/NevBar";

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

import { Grid, GridItem, Show } from "@chakra-ui/react";
import NavBar from "./componets/NevBar";

function App() {
  return (
    <Grid
      templateAreas={{
        base: `"nav" "main"`,
        lg: `"nav nav" "aside main"`,
      }}
    >
      <GridItem area="nav">
        <NavBar />
      </GridItem>
      <Show above="lg">
        <GridItem area="aside">Nav</GridItem>
      </Show>
      <GridItem area="main">Main</GridItem>
    </Grid>
  );
}

export default App;
