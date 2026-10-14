import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { AutocompleteOption } from '../../models/autocomplete-option.model';
import { DoctorSummary } from '../../models/doctor.model';
import { ContactModalComponent } from '../contact-modal/contact-modal.component';

@Component({
  selector: 'app-doctor-card',
  standalone: true,
  imports: [CommonModule, ContactModalComponent],
  templateUrl: './doctor-card.component.html',
  styleUrl: './doctor-card.component.css',
})
export class DoctorCardComponent {
  @Input({ required: true }) doctor!: DoctorSummary;
  @Input() languages: AutocompleteOption[] = [];

  protected readonly stars = [1, 2, 3, 4, 5];
  protected contactOpen = false;

  protected get languageNames(): string {
    return (this.doctor.languageIds ?? [])
      .map((id) => this.languages.find((l) => String(l.id) === id)?.name)
      .filter((name): name is string => !!name)
      .join(', ');
  }

  protected get location(): string {
    const clinic = this.doctor.clinics[0];
    return [clinic?.street, clinic?.cityName].filter(Boolean).join(', ');
  }
}
