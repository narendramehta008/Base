import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { NavMenuComponent } from './layout/nav-menu/nav-menu.component';
import { FooterComponent } from './layout/footer/footer.component';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AdministratorComponent } from './pages/administrator/administrator.component';
import { AuthGuardService } from './shared/services/auth-guard.service';
import { SharedModule } from './shared/shared.module';
import { provideHttpClient } from '@angular/common/http';
// import { NavMenuComponent } from './layout/nav-menu/nav-menu.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, NavMenuComponent, 
    FooterComponent, CommonModule, LoginComponent,DashboardComponent,
  AdministratorComponent, SharedModule],

  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  isAuthorized = true;
  title = 'ang-app';
}
