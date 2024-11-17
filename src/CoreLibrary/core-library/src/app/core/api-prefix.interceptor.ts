import {
  HttpEvent,
  HttpHandler,
  HttpHandlerFn,
  HttpInterceptor,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { environment } from '@environments/environment';
import { request } from 'http';
import { Observable } from 'rxjs';

export function apiPrefixInterceptor(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  if (request.url.includes('pstmn.io') || request.url.includes('http')) {
    return next(request);
  }
  request = request.clone({ url: environment.serverUrl + request.url });

  return next(request);
}
