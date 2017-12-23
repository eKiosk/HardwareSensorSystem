import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { Device } from '../device';
import { DeviceService } from '../device.service';

@Component({
  selector: 'app-device-detail',
  templateUrl: './device-detail.component.html',
  styles: []
})
export class DeviceDetailComponent {
  form: FormGroup;
  private subscription: Subscription;

  constructor(
    private router: Router,
    private deviceService: DeviceService,
    private formBuilder: FormBuilder,
    route: ActivatedRoute
  ) {
    this.subscription = route.params.subscribe(params => {
      if (params.id === 'new') {
        this.form = this.buildForm();
      } else {
        this.deviceService.getById(params.id).subscribe(device => {
          this.form = this.buildForm(device);
        });
      }
    });
  }

  save() {
    let request: Observable<Device>;

    if (!this.form.value.id) {
      request = this.deviceService.create(this.form.value);
    } else {
      request = this.deviceService.update(this.form.value);
    }

    request.subscribe(() => {
      this.router.navigate(['/devices']);
    });
  }

  private buildForm(device?: Device): FormGroup {
    return this.formBuilder.group({
      id: [device ? device.id : undefined],
      name: [device ? device.name : '', Validators.required]
    });
  }

}
