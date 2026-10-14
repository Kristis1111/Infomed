import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'doctors',
  },
  {
    path: 'doctors',
    loadComponent: () =>
      import('./features/doctors/pages/doctor-search/doctor-search.component').then(
        (m) => m.DoctorSearchComponent,
      ),
  },
  {
    path: 'doctors/:id',
    loadComponent: () =>
      import('./features/doctors/pages/doctor-details/doctor-details.component').then(
        (m) => m.DoctorDetailsComponent,
      ),
  },
  {
    path: '**',
    redirectTo: 'doctors',
  },
];
