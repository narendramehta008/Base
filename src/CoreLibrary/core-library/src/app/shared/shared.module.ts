import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthGuardService } from './services/auth-guard.service';
import { AuthenticationService } from './services/authentication.service';
import { FileSaverModule } from 'ngx-filesaver';
import { RouterModule } from '@angular/router';
import { UtilsService } from './services/utils.service';

@NgModule({
  declarations: [],
  providers: [AuthGuardService, AuthenticationService, UtilsService],
  imports: [CommonModule, RouterModule, FileSaverModule],
})
export class SharedModule {}
