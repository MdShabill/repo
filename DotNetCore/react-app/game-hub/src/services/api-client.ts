//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916284
// Featching The Games

import axios from "axios";

export default axios.create({
  baseURL: "https://api.rawg.io/api",
  params: {
    key: "2403c50c3e354feca446226452617bbf",
  },
});