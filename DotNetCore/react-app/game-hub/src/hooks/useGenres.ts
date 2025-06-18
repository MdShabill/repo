//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916324
// Featching The Genres

import genres from "../data/genres";

export interface Genre {
    image_background: string | undefined;
    id: number;
    name: string;
}

const useGenres = () => ({ data: genres, isLoading: false, error: null})

export default useGenres;