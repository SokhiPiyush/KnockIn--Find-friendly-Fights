import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResult<T>(url: string, params: HttpParams, http: HttpClient) {
    // if(this.members.length > 0) return of(this.members);//of bcz we need to return observable
const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;
// return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
  return http.get<T>(url, { observe: 'response', params }).pipe(
  map(response => {
    if (response.body) {
      paginatedResult.result = response.body;
    }
    const pagination = response.headers.get('Pagination');
    if (pagination) {
      paginatedResult.pagination = JSON.parse(pagination);
    }
    return paginatedResult;

  })
);
}

export function getPaginationHeaders(pageNumber: number, pageSize: number) {
let params = new HttpParams();

  params = params.append('pageNumber', pageNumber);
  params = params.append('pageSize', pageSize);

return params;
}