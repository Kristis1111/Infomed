import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Doctor } from '../../features/doctors/models/doctor.model';

export interface ChatReply {
  reply: string;
  doctors: Doctor[];
}

@Injectable({ providedIn: 'root' })
export class ChatApiService {
  private readonly http = inject(HttpClient);
  private readonly url = 'https://localhost:7058/api/v1/chat/messages';

  ask(message: string): Observable<ChatReply> {
    return this.http.post<ChatReply>(this.url, { message });
  }
}
