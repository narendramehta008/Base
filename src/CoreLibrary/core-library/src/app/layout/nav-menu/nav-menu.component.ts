import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthenticationService } from '../../shared/services/authentication.service';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './nav-menu.component.html',
  styleUrl: './nav-menu.component.scss'
})
export class NavMenuComponent {
  isLoggedIn = true;
  constructor(private authService: AuthenticationService) {
    this.isLoggedIn = authService.isLoggedIn;
  }
  ngAfterViewInit() {

  }
  ngOnInit() {
  }

  signOut() {
    this.authService.logOut();
  }
}
