import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../service/auth';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface User {
  ad: string;
  soyad: string;
  imageUrl: string;
}

interface Share {
  id: string;
  icerik: string;
  icerikResimUrl?: string;
  paylasimTarihi: string;
  userId: string;
  user: User;
}

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
  shares: Share[] = [];

  constructor(private authService: AuthService, private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.getShares();
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
  getShares(): void{
    this.http.get<any[]>('https://localhost:7107/shares')
    .subscribe({
      next: (data) => {
        console.log("API'den gelen veri:", data);
        this.shares = [...data];
        this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Failed to fetch shares:', err);
        }
    });
  }
}
