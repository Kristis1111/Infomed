import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { ChatApiService, ChatReply } from '../../core/services/chat-api.service';

@Component({
  selector: 'app-doctor-chat',
  standalone: true,
  imports: [FormsModule],
  styleUrl: './doctor-chat.component.css',
  template: `
    @if (open()) {
      <section class="chat-panel" dir="rtl" lang="he" aria-label="עוזר חכם לחיפוש רופאים">
        <header class="chat-header">
          <h2>עוזר חכם לחיפוש רופאים</h2>
          <button
            class="close-button"
            type="button"
            aria-label="סגירת הצ׳אט"
            (click)="open.set(false)"
          >
            ×
          </button>
        </header>

        <div class="chat-content">
          @if (error()) {
            <p class="error" role="alert">{{ error() }}</p>
          }

          @if (result(); as response) {
            @if (response.doctors.length > 0) {
              <div class="reply" aria-live="polite">
                <p class="result-summary">
                  מצאתי {{ response.doctors.length }} רופאים מתאימים:
                </p>
                <ul class="doctor-results">
                  @for (doctor of response.doctors; track doctor.id) {
                    <li>
                      <strong>{{ doctor.fullName }}</strong>
                      <span class="separator" aria-hidden="true">–</span>
                      <span>{{ doctor.clinics[0]?.cityName || 'עיר לא צוינה' }}</span>
                      <span class="separator" aria-hidden="true">–</span>
                      @if (doctor.phones[0]; as phone) {
                        <a [href]="'tel:' + phone">{{ phone }}</a>
                      } @else {
                        <span>טלפון לא צוין</span>
                      }
                    </li>
                  }
                </ul>
              </div>
            } @else {
              <p class="reply" aria-live="polite">{{ response.reply }}</p>
            }
          }
        </div>

        <form class="chat-form" (ngSubmit)="send()">
          <label for="chat-message">איך אוכל לעזור לכם למצוא רופא?</label>
          <div class="input-row">
            <input
              id="chat-message"
              name="message"
              [(ngModel)]="message"
              maxlength="2000"
              [disabled]="busy()"
              required
            />
            <button type="submit" [disabled]="busy() || !message.trim()">
              {{ busy() ? 'מחפש…' : 'שליחה' }}
            </button>
          </div>
        </form>
      </section>
    } @else {
      <button
        class="chat-launcher"
        type="button"
        aria-label="פתיחת הצ׳אט לחיפוש רופאים"
        [attr.aria-expanded]="open()"
        (click)="open.set(true)"
      >
        שאלו אותי על רופאים
      </button>
    }
  `,
})
export class DoctorChatComponent {
  private readonly api = inject(ChatApiService);

  protected readonly open = signal(false);
  protected message = '';
  protected readonly busy = signal(false);
  protected readonly error = signal('');
  protected readonly result = signal<ChatReply | null>(null);

  protected async send(): Promise<void> {
    const text = this.message.trim();

    if (!text || this.busy()) {
      return;
    }

    this.busy.set(true);
    this.error.set('');
    this.result.set(null);

    try {
      this.result.set(await firstValueFrom(this.api.ask(text)));
    } catch {
      this.error.set('העוזר אינו זמין כרגע. נסו שוב.');
    } finally {
      this.busy.set(false);
    }
  }
}
