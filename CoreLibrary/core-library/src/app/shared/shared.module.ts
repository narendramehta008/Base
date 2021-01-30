import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthGuardService } from './services/auth-guard.service';
import { AuthenticationService } from './services/authentication.service';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [],
  providers: [AuthGuardService, AuthenticationService],
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule
  ]
})
export class SharedModule { }
