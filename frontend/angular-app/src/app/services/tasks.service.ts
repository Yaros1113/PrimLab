import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedList } from '../models/paged-list.model';
import { Task } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TasksService {
  private apiUrl = 'http://localhost:5000/api/tasks';

  constructor(private http: HttpClient) {}

  getTasks(
    page = 1,
    pageSize = 10,
    search = '',
    sortBy = 'CreatedDate',
    ascending = false
  ): Observable<PagedList<Task>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('search', search)
      .set('sortBy', sortBy)
      .set('ascending', ascending.toString());

    return this.http.get<PagedList<Task>>(this.apiUrl, { params });
  }

  updateTaskStatus(id: number, status: boolean): Observable<Task> {
    return this.http.patch<Task>(`${this.apiUrl}/${id}/status`, { status });
  }
}