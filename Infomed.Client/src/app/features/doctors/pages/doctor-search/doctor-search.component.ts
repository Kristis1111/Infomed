import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Subject, catchError, debounceTime, distinctUntilChanged, of, switchMap, takeUntil } from 'rxjs';
import { DoctorApiService } from '../../../../core/services/doctor-api.service';
import { DoctorCardComponent } from '../../components/doctor-card/doctor-card.component';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import { AutocompleteOption } from '../../models/autocomplete-option.model';
import { DoctorSummary } from '../../models/doctor.model';

@Component({
  selector: 'app-doctor-search',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    DoctorCardComponent,
    PaginationComponent,
  ],
  templateUrl: './doctor-search.component.html',
  styleUrl: './doctor-search.component.css',
})
export class DoctorSearchComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject<void>();
  private readonly searchTrigger$ = new Subject<void>();
  private readonly nameAutocomplete$ = new Subject<string>();
  private readonly placeAutocomplete$ = new Subject<string>();
  private readonly formBuilder = new FormBuilder();

  protected readonly form = this.formBuilder.nonNullable.group({
    name: '',
    place: '',
    languageId: null as number | null,
    activeOnly: false,
    paidOnly: false,
    sortByRating: true,
    page: 1,
    pageSize: 12,
  });

  protected readonly doctors = signal<DoctorSummary[]>([]);
  protected readonly totalCount = signal(0);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal('');
  protected readonly languages = signal<AutocompleteOption[]>([]);
  protected readonly nameSuggestions = signal<AutocompleteOption[]>([]);
  protected readonly placeSuggestions = signal<AutocompleteOption[]>([]);
  protected readonly filterOptionsError = signal('');
  private appliedParams = { ...this.form.getRawValue(), name: '' };
  protected readonly openList = signal<'name' | 'place' | null>(null);

  constructor(
    private readonly doctorApiService: DoctorApiService,
    private readonly router: Router,
  ) {}

  ngOnInit(): void {
    this.doctorApiService
      .getLanguages()
      .pipe(
        catchError(() => {
          this.filterOptionsError.set('לא ניתן לטעון את רשימת השפות');
          return of([]);
        }),
        takeUntil(this.destroy$),
      )
      .subscribe((options) => this.languages.set(options));

    this.nameAutocomplete$
      .pipe(
        debounceTime(250),
        distinctUntilChanged(),
        switchMap((term) => {
          if (term.trim().length < 2) {
            return of([]);
          }
          return this.doctorApiService.searchExperts(term).pipe(
            catchError(() => {
              this.filterOptionsError.set('לא ניתן לטעון הצעות חיפוש');
              return of([]);
            }),
          );
        }),
        takeUntil(this.destroy$),
      )
      .subscribe((options) => this.nameSuggestions.set(options));

    this.placeAutocomplete$
      .pipe(
        debounceTime(250),
        distinctUntilChanged(),
        switchMap((term) => {
          if (term.trim().length < 2) {
            return of([]);
          }
          return this.doctorApiService.searchPlaces(term).pipe(
            catchError(() => {
              this.filterOptionsError.set('לא ניתן לטעון הצעות חיפוש');
              return of([]);
            }),
          );
        }),
        takeUntil(this.destroy$),
      )
      .subscribe((options) => this.placeSuggestions.set(options));

    this.searchTrigger$
      .pipe(
        switchMap(() => {
          this.isLoading.set(true);
          this.errorMessage.set('');
          return this.doctorApiService.searchDoctors(this.appliedParams).pipe(
            catchError(() => {
              this.errorMessage.set('אירעה שגיאה בחיפוש רופאים');
              return of(null);
            }),
          );
        }),
        takeUntil(this.destroy$),
      )
      .subscribe((result) => {
        this.doctors.set(result?.items ?? []);
        this.totalCount.set(result?.totalCount ?? 0);
        this.isLoading.set(false);
      });

    this.triggerSearch();
  }

  protected closeList(field: 'name' | 'place'): void {
    if (this.openList() === field) {
      this.openList.set(null);
    }
  }

  protected selectSuggestion(field: 'name' | 'place', option: AutocompleteOption, event: Event): void {
    event.preventDefault();
    this.openList.set(null);
    this.nameSuggestions.set([]);
    this.placeSuggestions.set([]);
    if (field === 'name') {
      this.form.controls.name.setValue('');
      void this.router.navigate(['/doctors', option.id]);
      return;
    }
    this.form.controls.place.setValue(option.name);
  }

  protected onNameInput(value: string): void {
    this.openList.set('name');
    this.filterOptionsError.set('');
    this.nameAutocomplete$.next(value);
  }

  protected onPlaceInput(value: string): void {
    this.openList.set('place');
    this.filterOptionsError.set('');
    this.placeAutocomplete$.next(value);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  protected onSubmitSearch(): void {
    this.appliedParams = { ...this.form.getRawValue(), name: '', page: 1 };
    this.form.patchValue({ page: 1 });
    this.triggerSearch();
  }

  protected onPageChange(page: number): void {
    this.appliedParams = { ...this.appliedParams, page };
    this.form.patchValue({ page });
    this.triggerSearch();
  }

  protected get page(): number {
    return this.appliedParams.page ?? 1;
  }

  protected get pageSize(): number {
    return this.appliedParams.pageSize ?? 20;
  }

  private triggerSearch(): void {
    this.searchTrigger$.next();
  }
}
