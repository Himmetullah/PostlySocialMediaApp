import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../service/auth';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './profile.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Profile implements OnInit {
  user: any = null;

  constructor(private authService: AuthService, private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    const storedUserId = this.authService.getUserId();

    if (!storedUserId) {
      console.log("User yok, giriş yapılmamış");
      return;
    }

    this.http.get(`https://localhost:7107/users/${storedUserId}`).subscribe({
      next: res => {
        console.log("API cevabı:", res);
        this.user = res;
        this.cdr.detectChanges();
      },
      error: err => {
        console.error("API hatası:", err);
        this.user = null;
        this.cdr.detectChanges();
      }
    });
  }
}
