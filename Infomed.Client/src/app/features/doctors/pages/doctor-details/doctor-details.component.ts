import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Subject, switchMap, takeUntil } from 'rxjs';
import { DoctorApiService } from '../../../../core/services/doctor-api.service';
import { Doctor } from '../../models/doctor.model';

@Component({
  selector: 'app-doctor-details',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './doctor-details.component.html',
  styleUrl: './doctor-details.component.css',
})
export class DoctorDetailsComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();

  protected doctor: Doctor | null = null;
  protected isLoading = false;
  protected errorMessage = '';

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly doctorApiService: DoctorApiService,
  ) {}

  ngOnInit(): void {
    this.activatedRoute.paramMap
      .pipe(
        switchMap((paramMap) => {
          this.isLoading = true;
          this.errorMessage = '';

          const id = Number(paramMap.get('id'));
          return this.doctorApiService.getDoctorById(id);
        }),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (doctor) => {
          this.doctor = doctor;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'לא ניתן לטעון את פרטי הרופא';
          this.isLoading = false;
        },
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
