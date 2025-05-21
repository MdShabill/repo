//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916327
// Building Game Card

// import { Game } from "../hooks/useGames";
// import { Card, CardBody, Heading, Image } from "@chakra-ui/react";

// interface Props {
//   game: Game;
// }

// const GameCard = ({ game }: Props) => {
//   return (
//     <Card borderRadius={10} overflow={"hidden"}>
//       <Image src={game.background_image} />
//       <CardBody>
//         <Heading fontSize="2xl">{game.name}</Heading>
//       </CardBody>
//     </Card>
//   );
// };

//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916325
// Displaying Platform Icon

import { Game } from "../hooks/useGames";
import { Card, CardBody, Heading, Image, Text } from "@chakra-ui/react";
import PlatformIconList from "./PlatformIconList";

interface Props {
  game: Game;
}

const GameCard = ({ game }: Props) => {
  return (
    <Card borderRadius={10} overflow={"hidden"}>
      <Image src={game.background_image} />
      <CardBody>
        <Heading fontSize="2xl">{game.name}</Heading>
        <PlatformIconList
          platform={game.parent_platforms.map((p) => p.platform)}
        />
      </CardBody>
    </Card>
  );
};

export default GameCard;
