import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, NgModule, OnInit, ViewEncapsulation } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../service/auth';
import { finalize } from 'rxjs';
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
  selector: 'app-home',
  imports: [FormsModule, CommonModule],
  templateUrl: './home.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.Default
})
export default class Home implements OnInit {
  shares: Share[] = [];
  newShareText: string = '';
  selectedFile: File | null = null;
  isLoading: boolean = false;

  constructor(private http: HttpClient, private authService: AuthService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.getShares();
  }

  getShares(): void {
    this.http.get<any[]>('https://localhost:7107/shares')
      .subscribe({
        next: (data) => {
          this.shares = [...data];
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Failed to fetch shares:', err);
        }
      });
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  createShare(): void {
    const userId = this.authService.getUserId();

    if (!userId) {
      alert('Paylaşım yapabilmek için lütfen giriş yapınız.');
      return;
    }
    if (!this.newShareText && !this.selectedFile) {
      alert('Lütfen bir içerik veya resim ekleyin.');
      return;
    }
    this.isLoading = true;

    const formData = new FormData();
    formData.append('userId', userId);
    formData.append('icerik', this.newShareText);

    if (this.selectedFile) {
      formData.append('icerikResim', this.selectedFile, this.selectedFile.name);
    }

    this.http.post('https://localhost:7107/shares', formData)
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (response) => {
          this.newShareText = '';
          this.selectedFile = null;
          this.getShares();
          alert('Paylaşım başarıyla oluşturuldu!');
        },
        error: (err) => {
          alert('Paylaşım oluşturulurken bir hata oluştu.');
        }
      });
  }
}
