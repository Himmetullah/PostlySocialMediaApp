import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../service/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export default class Navbar {
 constructor(public authService: AuthService, private router: Router) {}

 lagout(){
  this.authService.logout();
  this.router.navigate(['/']);
 }

 openLoginModal(){
  this.authService.toggleLoginModal(true);
 }

 openRegisterModal(){
  this.authService.toggleRegisterModal(true);
 }
}
