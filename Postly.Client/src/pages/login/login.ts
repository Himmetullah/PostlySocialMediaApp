import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ViewEncapsulation } from '@angular/core';
import { initializeUser, LoginModel } from '../../models/login';
import { FormsModule } from '@angular/forms';
import { Result } from '../../models/result.model';
import { NgClass } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, NgClass, RouterLink],
  templateUrl: './login.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Login {
  showLogin: boolean = false;
  showRegister: boolean = false;
  user: LoginModel = { ...initializeUser };
  message: string = '';
  messageType: 'success' | 'error' | '' = '';

  constructor(
    private http: HttpClient,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private authService: AuthService
  ) { }

  login() {
    this.http.post<Result<string>>("https://localhost:7107/users/login", this.user)
      .subscribe({
        next: (res) => {
          if (res.isSuccessful && res.data) {
            localStorage.setItem('userId', res.data);
            this.authService.login(res.data);
            this.message = 'Giriş başarılı';
            this.messageType = 'success';
            this.cdr.detectChanges();
            this.router.navigate(['profile']);
          } else {
            this.message = res.errorMessages?.[0] ?? 'Bilinmeyen hata';
            this.messageType = 'error';
            this.cdr.detectChanges();
          }
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.message = 'Sunucuya bağlanılamadı';
          this.messageType = 'error';
          this.cdr.detectChanges();
        }
      });
  }

  openLogin() {
    this.showLogin = true;
    this.showRegister = false;
  }

  openRegister() {
    this.showRegister = true;
    this.showLogin = false;
  }

  close() {
    this.showLogin = false;
    this.showRegister = false;
  }
}
