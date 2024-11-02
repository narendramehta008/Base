import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthGuardService } from './services/auth-guard.service';
import { AuthenticationService } from './services/authentication.service';
import { HttpClient, HttpClientModule, provideHttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [],
  providers: [AuthGuardService, AuthenticationService],
  imports: [
    CommonModule,
    RouterModule
  ]
})
export class SharedModule { }
