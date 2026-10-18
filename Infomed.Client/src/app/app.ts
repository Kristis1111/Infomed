import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DoctorChatComponent } from './features/chat/doctor-chat.component';

@Component({
  imports: [RouterOutlet, DoctorChatComponent],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
}
