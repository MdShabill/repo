//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916324
// Featching The Genres

// import useGenres from "../hooks/useGenres";

// const GenreList = () => {
//   const { genres } = useGenres();

//   return (
//     <ul>
//       {genres.map((genre) => (
//         <li key={genre.id}>{genre.name}</li>
//       ))}
//     </ul>
//   );
// };

//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916324
// Featching The Genres

import {
  Button,
  Heading,
  HStack,
  Image,
  List,
  ListItem,
  Spinner,
  Text,
} from "@chakra-ui/react";
import useGenres, { Genre } from "../hooks/useGenres";
import getCroppedImageUrl from "../services/image-url";
import { is } from "immer/dist/internal";

interface Props {
  onSelectedGenre: (genre: Genre) => void;
  selectedGenre: Genre | null;
}

const GenreList = ({ selectedGenre, onSelectedGenre }: Props) => {
  const { data, isLoading, error } = useGenres();

  if (error) return null;

  if (isLoading) return <Spinner />;

  return (
    <>
      <Heading fontSize="2xl" marginTop={9} marginBottom={3}>
        Genres
      </Heading>
      <List>
        {data.map((genre) => (
          <ListItem key={genre.id} paddingY="5px">
            <HStack>
              <Image
                boxSize="32px"
                borderRadius={8}
                objectFit="cover"
                src={getCroppedImageUrl(genre.image_background ?? "")}
              />
              <Button
                whiteSpace="normal"
                textAlign="left"
                fontWeight={genre.id === selectedGenre?.id ? "bold" : "normal"}
                onClick={() => onSelectedGenre(genre)}
                fontSize="md"
                variant="link"
              >
                {genre.name}
              </Button>
            </HStack>
          </ListItem>
        ))}
      </List>
    </>
  );
};

export default GenreList;
