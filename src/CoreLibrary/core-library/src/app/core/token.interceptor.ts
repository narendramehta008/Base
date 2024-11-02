import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenModel } from '@app/shared/models/token-models';
import { AuthenticationService } from '@app/shared/services/authentication.service';
import { environment } from '@environments/environment.development';
import { Observable, tap } from 'rxjs';

export function tokenInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  if (request.url.includes('pstmn.io')) {
    return next(request);
  }
  const authService = inject(AuthenticationService);
if (request.url.includes('/auth/login') === false) {
    let token = '';
    const tokenDetails = localStorage.getItem(environment.tokenName) || null;
    if (tokenDetails) {
        let tempTokenDetail: TokenModel = JSON.parse(tokenDetails);
        token = tempTokenDetail.token;
    }
    request = request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`)
    });
}

return next(request).pipe(
    tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
        }
    }, (err: any) => {
        if (err instanceof HttpErrorResponse) {
            if (err.status === 401) {
                authService.logOut();
            }
        }
    })
)
};