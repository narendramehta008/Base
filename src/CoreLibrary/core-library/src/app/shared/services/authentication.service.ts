import { Injectable } from '@angular/core';
import { TokenModel } from '../models/token-models';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoginModel } from '../models/authentication-models';
import { catchError, map, of, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  tokenDetails: TokenModel | undefined;
  isLoggedIn = false;
  constructor(private router: Router, private httpClient: HttpClient) {
    let storageToken = localStorage?.getItem(environment.tokenName);
    if (storageToken) {
      this.tokenDetails = JSON.parse(storageToken);
      if (this.tokenDetails?.token) this.isLoggedIn = true;
    }
  }

  login(loginModel: LoginModel) {
    return this.httpClient
      .post(environment.apiEndPoint.auth.login, loginModel)
      .pipe(
        map((res) => res),
        map((body) => body),
        catchError((err) => of(err))
      );
  }

  logOut() {
    localStorage.removeItem(environment.tokenName);
    this.isLoggedIn = false;
    this.router.navigate(['/login']);
  }
}
