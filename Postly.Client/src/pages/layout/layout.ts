import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import Navbar from './navbar/navbar';
import Login from '../login/login';
import { RouterOutlet } from '@angular/router';
import { AuthService } from '../../service/auth';

@Component({
  imports: [Navbar, RouterOutlet],
  templateUrl: './layout.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Layout implements OnInit {
  isLoginModalOpen = false;
  isRegisterModalOpen = false;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.loginModals$.subscribe((state: boolean) => {
      this.isLoginModalOpen = state;
    });
    
    this.authService.registerModals$.subscribe((state: boolean) => {
      this.isRegisterModalOpen = state;
    });
  }

  closeModal(){
    this.authService.toggleLoginModal(false);
    this.authService.toggleRegisterModal(false);
  }
}
