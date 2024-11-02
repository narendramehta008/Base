import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { LoginComponent } from './pages/login/login.component';
import { AdministratorComponent } from './pages/administrator/administrator.component';
import { AuthGuardService } from './shared/services/auth-guard.service';

export const routes: Routes = [
    { path: '', component: DashboardComponent, canActivate: [AuthGuardService] },
    { path: 'login', component: LoginComponent, canActivate: [AuthGuardService] },
    { path: 'administrator', component: AdministratorComponent, canActivate: [AuthGuardService] },
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuardService] },
    {
      path: '**',
      redirectTo: 'dashboard',
    }
];
