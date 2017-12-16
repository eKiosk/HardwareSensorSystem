import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { User } from '../user';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styles: []
})
export class UserDetailComponent {
  form: FormGroup;
  private roleId: number;
  private subscription: Subscription;

  constructor(
    private router: Router,
    private userService: UserService,
    private formBuilder: FormBuilder,
    route: ActivatedRoute
  ) {
    this.subscription = route.params.subscribe(params => {
      this.roleId = params.id;
      if (params.userId === 'new') {
        this.form = this.buildCreateForm(this.roleId);
      } else {
        this.userService.getById(params.userId).subscribe(user => {
          this.form = this.buildUpdateForm(user);
        });
      }
    });
  }

  save() {
    let request: Observable<User>;

    if (!this.form.value.id) {
      request = this.userService.create(this.form.value);
    } else {
      request = this.userService.update(this.form.value);
    }

    request.subscribe(() => {
      this.router.navigate(['/roles', this.roleId, 'users']);
    });
  }

  private buildCreateForm(roleId: number): FormGroup {
    return this.formBuilder.group({
      userName: ['', [Validators.required, Validators.minLength(5)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      roleId: [roleId]
    });
  }

  private buildUpdateForm(user: User): FormGroup {
    return this.formBuilder.group({
      id: [user.id],
      userName: [user.userName, [Validators.required, Validators.minLength(5)]],
      email: [user.email, [Validators.required, Validators.email]],
      password: ['', Validators.minLength(8)],
      roleId: [undefined],
      securityStamp: [user.securityStamp],
      concurrencyStamp: [user.concurrencyStamp]
    });
  }

}
