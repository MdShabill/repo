//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916324
// Featching The Genres

import useData from "./useData";

export interface Genre {
    image_background: string | undefined;
    id: number;
    name: string;
}

const useGenres = () => useData<Genre>('/genres')

export default useGenres;