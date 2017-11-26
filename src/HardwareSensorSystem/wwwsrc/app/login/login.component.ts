import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: [`
  .container {
    background-color: #303030;
    height: 100%;
  }`]
})
export class LoginComponent {
  form: FormGroup;

  constructor(
    fb: FormBuilder,
    private router: Router
  ) {
    this.form = fb.group({
      userName: ['', [Validators.required, Validators.minLength(6)]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  login() {
    this.router.navigate(['/charts']);
  }

}
