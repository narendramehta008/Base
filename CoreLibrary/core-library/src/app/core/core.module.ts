import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { APIPrefixInterceptor } from './interceptors/api-prefix.interceptor';



@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ], providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: APIPrefixInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ]
})
export class CoreModule { }
