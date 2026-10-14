import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Doctor, DoctorSummary } from '../../features/doctors/models/doctor.model';
import { ContactRequest } from '../../features/doctors/models/contact-request.model';
import { AutocompleteOption } from '../../features/doctors/models/autocomplete-option.model';
import { DoctorSearchParams } from '../../features/doctors/models/doctor-search-params.model';
import { PagedResult } from '../../features/doctors/models/doctor-search-response.model';

@Injectable({
  providedIn: 'root',
})
export class DoctorApiService {
  private readonly apiUrl = 'https://localhost:7058/api/v1/doctors';
  private readonly autocompleteUrl = 'https://localhost:7058/api/v1/autocomplete';
  private readonly dropdownUrl = 'https://localhost:7058/api/v1/dropdown';

  constructor(private readonly httpClient: HttpClient) {}

  searchDoctors(params: DoctorSearchParams): Observable<PagedResult<DoctorSummary>> {
    return this.httpClient.get<PagedResult<DoctorSummary>>(this.apiUrl, {
      params: this.createParams(params),
    });
  }

  getDoctorById(id: number): Observable<Doctor> {
    return this.httpClient.get<Doctor>(`${this.apiUrl}/${id}`);
  }

  searchExperts(searchTerm: string): Observable<AutocompleteOption[]> {
    return this.httpClient.get<AutocompleteOption[]>(`${this.autocompleteUrl}/expertssearch`, {
      params: { searchTerm },
    });
  }

  searchPlaces(searchTerm: string): Observable<AutocompleteOption[]> {
    return this.httpClient.get<AutocompleteOption[]>(`${this.autocompleteUrl}/areasandcitiessearch`, {
      params: { searchTerm },
    });
  }

  getLanguages(): Observable<AutocompleteOption[]> {
    return this.httpClient.get<AutocompleteOption[]>(`${this.dropdownUrl}/languages`);
  }

  submitContactRequest(doctorId: number, request: ContactRequest): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/${doctorId}/contact`, request);
  }

  private createParams(params: DoctorSearchParams): HttpParams {
    let httpParams = new HttpParams();

    if (params.name?.trim()) {
      httpParams = httpParams.set('name', params.name.trim());
    }

    if (params.place?.trim()) {
      httpParams = httpParams.set('place', params.place.trim());
    }

    if (params.languageId !== null && params.languageId !== undefined) {
      httpParams = httpParams.set('languageId', params.languageId);
    }

    httpParams = httpParams.set('activeOnly', params.activeOnly ?? false);
    httpParams = httpParams.set('paidOnly', params.paidOnly ?? false);
    httpParams = httpParams.set('sortByRating', params.sortByRating ?? false);
    httpParams = httpParams.set('page', params.page ?? 1);
    httpParams = httpParams.set('pageSize', params.pageSize ?? 20);

    return httpParams;
  }
}
