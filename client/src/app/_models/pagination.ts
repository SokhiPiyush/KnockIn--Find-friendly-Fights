export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

export class PaginatedResult<T> {//T will be our list of things
    result?: T;
    pagination?: Pagination;
    
}