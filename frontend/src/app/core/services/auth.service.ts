import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';

import { Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';

import { User } from '../models/user.model';
import { AuthResponseDto } from '../dtos/auth/auth-response.dto';
import { GoogleLoginDto } from '../dtos/auth/google-login.dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;
  authUrl = `${this.apiUrl}/api/auth/google`;
  currentUser = signal<User | null>(null);
  currentToken = signal<string | null>(null);

  login(loginDto: GoogleLoginDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(this.authUrl, loginDto).pipe(
      tap( response => {
        this.currentUser.set(response.user);
        localStorage.setItem('user', JSON.stringify(response.user))

        this.currentToken.set(response.token);
        localStorage.setItem('token', response.token)
      } )
    );
  }

  logout(): void {
    this.currentUser.set(null);
    localStorage.removeItem('user');

    this.currentToken.set(null);
    localStorage.removeItem('token');
  }

  loadCurrentUser(): void {
    const user = localStorage.getItem('user');
    const token = localStorage.getItem('token');

    if (token && user) {
      this.currentUser.set(JSON.parse(user));
      this.currentToken.set(token);
    }
  }
}