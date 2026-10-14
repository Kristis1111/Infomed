export interface DoctorClinic {
  cityName: string | null;
  street: string | null;
}

export interface Doctor {
  id: number;
  fullName: string;
  promotionLevel: number;
  isActive: boolean;
  averageRating: number;
  totalRatings: number;
  languageIds: string[];
  phones: string[];
  clinics: DoctorClinic[];
}

export type DoctorSummary = Doctor;
