//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916284
// Featching The Games

import axios from "axios";

export default axios.create({
  baseURL: "https://api.rawg.io/api",
  params: {
    key: "df50bd3798a84d5aa069b7e73304faa2",
  },
});