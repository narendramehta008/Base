import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { NavMenuComponent } from './layout/nav-menu/nav-menu.component';
import { FooterComponent } from './layout/footer/footer.component';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AdministratorComponent } from './pages/administrator/administrator.component';
import { SharedModule } from './shared/shared.module';
import { BtspModule } from './shared/btsp/btsp.module';
import { ServicesModule } from './services/services.module';
import { AuthenticationService } from './shared/services/authentication.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    NavMenuComponent,
    FooterComponent,
    CommonModule,
    LoginComponent,
    DashboardComponent,
    AdministratorComponent,
    SharedModule,
    BtspModule,
    ServicesModule,
  ],

  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthenticationService) { }

  isAuthorized = false;
  title = 'ang-app';

  ngOnInit(): void {
    this.isAuthorized = this.authService.isLoggedIn
  }
}
