import { NgClass } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ViewEncapsulation } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { initializeRegister, RegisterModel } from '../../models/register';
import { HttpClient } from '@angular/common/http';
import { Result } from '../../models/result.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, NgClass, RouterLink],
  templateUrl: './register.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Register {
  user: RegisterModel = {...initializeRegister};
  message: string = '';
  messageType: 'success' | 'error' | '' = '';

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {}

  register(){
    this.http.post<Result<string>>("https://localhost:7107/users/register", this.user)
    .subscribe({
      next: (res) => {
        if (res.isSuccessful) {
          this.message = res.data ?? 'Kayıt başarılı. Giriş yapabilirsiniz.';
          this.messageType = 'success';
          this.user = {...initializeRegister};
        } else {
          this.message = res.errorMessages?.[0] ?? 'Bilinmeyen hata';
          this.messageType = 'error';
        }
        this.cdr.detectChanges();
      },
      error: () => {
        this.message = 'Sunucuya bağlanılamadı';
        this.messageType = 'error';
        this.cdr.detectChanges();
      }
    });
  }
}
