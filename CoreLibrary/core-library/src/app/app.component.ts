import { Component } from '@angular/core';
import { AuthenticationService } from './shared/services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  isAuthorized = false;
  constructor(private authService: AuthenticationService) {
    this.isAuthorized = authService.isLoggedIn;
  }
  title = 'core-library';


}
