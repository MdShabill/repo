//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915912
// Extracting a Reusable API Clint

import axios, { CanceledError } from "axios";

export default axios.create({
    baseURL: 'https://jsonplaceholder.typicode.com',
})

export{ CanceledError };