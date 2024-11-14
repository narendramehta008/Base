import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { LoginComponent } from './pages/login/login.component';
import { AdministratorComponent } from './pages/administrator/administrator.component';
import { AuthGuardService } from './shared/services/auth-guard.service';
import { ServicesComponent } from './services/services.component';

export const routes: Routes = [
  { path: '', component: DashboardComponent, canActivate: [AuthGuardService] },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuardService] },
  {
    path: 'administrator',
    component: AdministratorComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'services',
    component: ServicesComponent,
    canActivate: [AuthGuardService],
    loadChildren: () =>
      import('../app/services/services-routing.module').then(
        (m) => m.ServicesRoutingModule
      ),
  },
  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
