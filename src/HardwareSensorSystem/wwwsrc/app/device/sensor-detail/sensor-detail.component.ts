import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { Sensor } from '../sensor';
import { SensorService } from '../sensor.service';

@Component({
  selector: 'app-sensor-detail',
  templateUrl: './sensor-detail.component.html',
  styles: []
})
export class SensorDetailComponent {
  form: FormGroup;
  private deviceId: number;
  private subscription: Subscription;

  constructor(
    private router: Router,
    private sensorService: SensorService,
    private formBuilder: FormBuilder,
    route: ActivatedRoute
  ) {
    this.subscription = route.params.subscribe(params => {
      this.deviceId = params.id;
      if (params.sensorId === 'new') {
        this.form = this.buildForm(this.deviceId);
      } else {
        this.sensorService.getById(params.sensorId).subscribe(sensor => {
          this.form = this.buildForm(this.deviceId, sensor);
        });
      }
    });
  }

  save() {
    let request: Observable<Sensor>;

    if (!this.form.value.id) {
      request = this.sensorService.create(this.form.value);
    } else {
      request = this.sensorService.update(this.form.value);
    }

    request.subscribe(() => {
      this.router.navigate(['/devices', this.deviceId, 'sensors']);
    });
  }

  private buildForm(deviceId: number, sensor?: Sensor): FormGroup {
    return this.formBuilder.group({
      id: [sensor ? sensor.id : undefined],
      name: [sensor ? sensor.name : '', Validators.required],
      deviceId: [deviceId],
      properties: this.formBuilder.array([])
    });
  }

}
