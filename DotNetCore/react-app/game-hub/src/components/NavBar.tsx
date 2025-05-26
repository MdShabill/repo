//---------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916280
// Building the Navigation Bar

// import { HStack, Image } from "@chakra-ui/react";
// import logo from "../assets/logo.webp";
// import ColorModeSwitch from "./ColorModeSwitch";

// const NavBar = () => {
//   return (
//     <HStack justifyContent="space-between" padding="10px">
//       <Image src={logo} boxSize="60px" />
//       <ColorModeSwitch />
//     </HStack>
//   );
// };

//---------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916281
// Building the Color Mode Switch

import { HStack, Image } from "@chakra-ui/react";
import logo from "../assets/logo.webp";
import ColorModeSwitch from "./ColorModeSwitch";
//import SearchInput from "./SearchInput";

// interface Props {
//   onSearch: (searchText: string) => void;
// }

//const NavBar = ({ onSearch }: Props) => {
const NavBar = () => {
  return (
    <HStack justifyContent="space-between" padding="10px">
      <Image src={logo} boxSize="60px" />
      {/* <SearchInput onSearch={onSearch} /> */}
      <ColorModeSwitch />
    </HStack>
  );
};

export default NavBar;
