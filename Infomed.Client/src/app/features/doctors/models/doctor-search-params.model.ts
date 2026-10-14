export interface DoctorSearchParams {
  name?: string;
  place?: string;
  languageId?: number | null;
  activeOnly?: boolean;
  paidOnly?: boolean;
  sortByRating?: boolean;
  page?: number;
  pageSize?: number;
}
