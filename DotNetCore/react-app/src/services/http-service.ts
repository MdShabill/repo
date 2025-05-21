//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915918
// Creating a Generic HTTP service

import apiClient from "./api-client";

interface Entity {
    id: number;
}

class httpService {
    endPoint: string;

    constructor(endpoint: string) {
        this.endPoint = endpoint;
    }

    getAll<T>() {
        const controller = new AbortController();
        const request = apiClient.get<T[]>(this.endPoint, {
            signal: controller.signal,
        });
        return { request, cancel: () => controller.abort() }
    }

    delete(id: number) {
        return apiClient.delete(this.endPoint + "" + id)
    }

    create<T>(entity: T) {
        return apiClient.post(this.endPoint, entity)
    }

    update<T extends Entity>(entity: T) {
        return apiClient.patch(this.endPoint + "/" + entity.id, entity)
    }
}

const create = (endpoint: string) => new httpService(endpoint);

export default create;