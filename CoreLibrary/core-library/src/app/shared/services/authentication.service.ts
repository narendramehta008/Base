import { Injectable } from '@angular/core';
import { LoginModel } from '../models/authentication-models';
import { TokenModel } from '../models/token-models';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { catchError, map } from 'rxjs/operators';
import { of, throwError } from 'rxjs';

@Injectable()
export class AuthenticationService {
  tokenDetails: TokenModel;
  isLoggedIn = false;
  constructor(private router: Router, private httpClient: HttpClient) { //private httpClient: HttpClient,
    let storageToken = localStorage.getItem(environment.tokenName);
    if (storageToken) {
      this.tokenDetails = JSON.parse(storageToken);
      if (this.tokenDetails.token) this.isLoggedIn = true;
    }
  }
  login(loginModel: LoginModel) {
    const params = {
      username: loginModel.username,
      password: loginModel.password
    }
    return this.httpClient.post(environment.apiEndPoint.auth.login, loginModel)
      .pipe(
        map((res) => res),
        map((body) => body),
        catchError((err) => throwError(err))
      );
  }

  getRequest(url, params?) {
    return this.httpClient.get(url, params)
      .pipe(
        map((res) => res),
        map((body) => body),
        catchError((body) => of(body))
      );
  }

  logOut() {
    localStorage.removeItem(environment.tokenName);
    this.isLoggedIn = false;
    this.router.navigate(['/login']);
  }
}
