import { CommonModule } from '@angular/common';
import { Component, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginModel } from '@app/shared/models/authentication-models';
import { AuthenticationService } from '@app/shared/services/authentication.service';
import { SharedModule } from '@app/shared/shared.module';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, SharedModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginModel: LoginModel = new LoginModel();
  submitted = false;
  errorMessage = '';

  constructor(private authService: AuthenticationService, private router: Router) {
  }

  ngOnInit(): void {
  }

  onSubmit() {

    this.authService.login(this.loginModel).subscribe({
      next: (res: any) => {
        if (res && res.token) {
          localStorage.setItem('tokenDetails', JSON.stringify(res));
          this.authService.isLoggedIn = true;
          this.router.navigate(['/dashboard']);
          window.location.reload();
        }
        else {
          this.errorMessage = 'Invalid Email or password';
        }
      },
      error: (error: any) => {
        this.errorMessage = error.message || error.statusText || 'Something went wrong';
      }
    });
  }
}
