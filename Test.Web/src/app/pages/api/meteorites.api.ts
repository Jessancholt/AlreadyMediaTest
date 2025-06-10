import { inject, Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpParamsOptions,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments';
import { MeteoriteFilter, MeteoritesContext } from '../models';

@Injectable()
export class MeteoritesApiService {
  private readonly http = inject(HttpClient);

  private url = `${environment.baseUrl}/meteorite/filter`;
  private headers = new HttpHeaders({ 'Content-Type': 'application/json' });

  getByFilter(filter: MeteoriteFilter): Observable<MeteoritesContext> {
    return this.http.post<MeteoritesContext>(this.url, filter);
  }
}
