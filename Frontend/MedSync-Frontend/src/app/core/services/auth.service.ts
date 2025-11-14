import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

interface LoginResponse {
  success: boolean;
  message: string;
  data: {
    Token: string;
    User: any;
  }

}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.apiUrl + '/Account/Login';
  private currentUserSubject = new BehaviorSubject<any>(this.getUserFromStorage());
  currentUser$ = this.currentUserSubject.asObservable();


  constructor(private http: HttpClient) { }



  login(credentials: { email: string; password: string }): Observable<LoginResponse> {
    const body = {
      Email: credentials.email,
      PasswordHash: credentials.password
    };
    return this.http.post<LoginResponse>(this.baseUrl, body).pipe(
      tap(response => {
        if (response.data && response.data.Token) {
          localStorage.setItem('token', response.data.Token);
          localStorage.setItem('user', JSON.stringify(response.data.User));
          this.currentUserSubject.next(response.data.User);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
  }


  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }


  getToken(): string | null {
    return localStorage.getItem('token');
  }


  getUserRole(): string | null {
    const user = this.getUserFromStorage();
    return user ? user.RoleName || user.roleName || null : null;
  }

  isAdmin(): boolean {
    return this.getUserRole()?.toLowerCase() === 'admin';
  }

  isDoctor(): boolean {
    return this.getUserRole()?.toLowerCase() === 'doctor';
  }

  getUserFromStorage(): any {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
}
