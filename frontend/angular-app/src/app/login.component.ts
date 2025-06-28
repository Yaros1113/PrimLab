import { Component } from "@angular/core";
import { AuthService } from "./auth/auth.service";
import { Router } from "@angular/router";

@Component({
  template: `
    <form (ngSubmit)="onSubmit()">
      <mat-form-field>
        <input matInput placeholder="Username" [(ngModel)]="credentials.username">
      </mat-form-field>
      <mat-form-field>
        <input matInput type="password" placeholder="Password" [(ngModel)]="credentials.password">
      </mat-form-field>
      <button mat-raised-button color="primary">Login</button>
    </form>
  `
})
export class LoginComponent {
  credentials = { username: '', password: '' };

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.login(this.credentials).subscribe(() => {
      this.router.navigate(['/dashboard']);
    });
  }
}
