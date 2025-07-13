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

// import { Game } from "../hooks/useGames";
// import { Card, CardBody, Heading, HStack, Image } from "@chakra-ui/react";
// import PlatformIconList from "./PlatformIconList";
// import CriticScore from "./CriticScore";
// import getCroppedImageUrl from "../services/image-url";

// interface Props {
//   game: Game;
// }

// const GameCard = ({ game }: Props) => {
//   return (
//     <Card borderRadius={10} overflow={"hidden"}>
//       <Image src={getCroppedImageUrl(game.background_image ?? "")} />
//       <CardBody>
//         <Heading fontSize="2xl">{game.name}</Heading>
//         <HStack justifyContent="space-between">
//           <PlatformIconList
//             platform={game.parent_platforms.map((p) => p.platform)}
//           />
//           <CriticScore score={game.metacritic} />
//         </HStack>
//       </CardBody>
//     </Card>
//   );
// };

//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916331
// Refactor - Removing Duplicated Style

import { Game } from "../hooks/useGames";
import { Card, CardBody, Heading, HStack, Image } from "@chakra-ui/react";
import PlatformIconList from "./PlatformIconList";
import CriticScore from "./CriticScore";
import getCroppedImageUrl from "../services/image-url";
import Emoji from "./Emoji";

interface Props {
  game: Game;
}

const GameCard = ({ game }: Props) => {
  return (
    <Card>
      <Image src={getCroppedImageUrl(game.background_image ?? "")} />
      <CardBody>
        <Heading fontSize="2xl">
          {game.name}
          <Emoji rating={game.rating_top} />
        </Heading>
        <HStack justifyContent="space-between" marginBottom={3}>
          <PlatformIconList
            platform={game.parent_platforms.map((p) => p.platform)}
          />
          <CriticScore score={game.metacritic} />
        </HStack>
      </CardBody>
    </Card>
  );
};

export default GameCard;
