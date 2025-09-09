import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginModalSource = new BehaviorSubject<boolean>(false);
  private registerModalSource = new BehaviorSubject<boolean>(false);
  private loggedInSource = new BehaviorSubject<boolean>(!!this.getUserIdFromStorage());

  loginModals$ = this.loggedInSource.asObservable();
  registerModals$ = this.registerModalSource.asObservable();
  isLoggedIn$ = this.loggedInSource.asObservable();
  private userIdSource = new BehaviorSubject<string | null>(this.getUserIdFromStorage());
  userId$ = this.userIdSource.asObservable();

  login(userId: string) {
    this.userIdSource.next(userId);
    this.loggedInSource.next(true);
    sessionStorage.setItem('userId', userId);
  }

  logout() {
    this.userIdSource.next(null);
    this.loggedInSource.next(false);
    sessionStorage.removeItem('userId');
  }

  toggleLoginModal(state: boolean) {
    this.loginModalSource.next(state);
  }

  toggleRegisterModal(state: boolean) {
    this.registerModalSource.next(state);
  }

  private getUserIdFromStorage(): string | null {
    return sessionStorage.getItem('userId');
  }

    getUserId(): string | null {
    return this.getUserIdFromStorage();
  }
}
