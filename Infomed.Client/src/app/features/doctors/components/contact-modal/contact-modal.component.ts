import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Input, Output, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DoctorApiService } from '../../../../core/services/doctor-api.service';

const PHONE_PATTERN = /^0(5\d|[23489]|7\d)-?\d{7}$/;

@Component({
  selector: 'app-contact-modal',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './contact-modal.component.html',
  styleUrl: './contact-modal.component.css',
})
export class ContactModalComponent {
  @Input({ required: true }) doctorId!: number;
  @Input({ required: true }) doctorName!: string;
  @Output() closed = new EventEmitter<void>();

  private readonly fb = inject(FormBuilder);
  private readonly api = inject(DoctorApiService);

  protected readonly submitting = signal(false);
  protected readonly sent = signal(false);
  protected readonly error = signal<string | null>(null);

  protected readonly form = this.fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
    phone: ['', [Validators.required, Validators.pattern(PHONE_PATTERN)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(254)]],
  });

  protected invalid(name: 'fullName' | 'phone' | 'email'): boolean {
    const control = this.form.controls[name];
    return control.invalid && (control.touched || control.dirty);
  }

  protected submit(): void {
    this.error.set(null);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    this.api.submitContactRequest(this.doctorId, this.form.getRawValue()).subscribe({
      next: () => {
        this.submitting.set(false);
        this.sent.set(true);
      },
      error: (response: HttpErrorResponse) => {
        this.submitting.set(false);
        const messages = response.status === 400 && response.error?.errors
          ? (Object.values(response.error.errors) as string[][]).flat()
          : ['שליחת הפרטים נכשלה, נסו שוב מאוחר יותר.'];
        this.error.set(messages.join(' '));
      },
    });
  }

  protected close(): void {
    this.closed.emit();
  }
}
