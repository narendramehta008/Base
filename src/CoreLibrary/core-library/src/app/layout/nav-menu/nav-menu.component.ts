import { CommonModule } from '@angular/common';
import { AfterViewInit, Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthenticationService } from '../../shared/services/authentication.service';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './nav-menu.component.html',
  styleUrl: './nav-menu.component.scss',
})
export class NavMenuComponent {
  //implements AfterViewInit
  isLoggedIn = true;
  constructor(private authService: AuthenticationService) {}
  // ngAfterViewInit() {
  //   this.authService.checkLoggedIn();
  //   this.isLoggedIn = this.authService.isLoggedIn;
  // }
  ngOnInit() {}

  signOut() {
    this.authService.logOut();
  }
}
