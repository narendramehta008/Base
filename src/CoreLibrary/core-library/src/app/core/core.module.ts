import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { apiPrefixInterceptor } from './api-prefix.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';



@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers:[]
})
export class CoreModule { }
