import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { Role } from '../role';
import { RoleService } from '../role.service';

@Component({
  selector: 'app-role-detail',
  templateUrl: './role-detail.component.html',
  styles: []
})
export class RoleDetailComponent {
  form: FormGroup;
  private subscription: Subscription;

  constructor(
    private router: Router,
    private roleService: RoleService,
    private formBuilder: FormBuilder,
    route: ActivatedRoute
  ) {
    this.subscription = route.params.subscribe(params => {
      if (params.id === 'new') {
        this.form = this.buildForm();
      } else {
        this.roleService.getById(params.id).subscribe(role => {
          this.form = this.buildForm(role);
        });
      }
    });
  }

  save() {
    let request: Observable<Role>;

    if (!this.form.value.id) {
      request = this.roleService.create(this.form.value);
    } else {
      request = this.roleService.update(this.form.value);
    }

    request.subscribe(() => {
      this.router.navigate(['/roles']);
    });
  }

  private buildForm(role?: Role): FormGroup {
    return this.formBuilder.group({
      id: [role ? role.id : undefined],
      name: [role ? role.name : '', Validators.required],
      concurrencyStamp: [role ? role.concurrencyStamp : undefined]
    });
  }

}
