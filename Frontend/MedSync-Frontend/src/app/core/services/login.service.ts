import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class LoginService {

    private baseUrl = environment.apiUrl + '/account/login';

    constructor(private http: HttpClient) { }

    login(credentials: { email: string; password: string }): Observable<any> {
        const body = {
            Email: credentials.email,
            PasswordHash: credentials.password
        };

        return this.http.post(this.baseUrl, body);
    }
}
