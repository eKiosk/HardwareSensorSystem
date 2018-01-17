import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Validators } from '@angular/forms';
import { FormArray } from '@angular/forms/src/model';
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

  get propertiesForm(): FormArray {
    return this.form.get('properties') as FormArray;
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

  addEmptyProperty() {
    this.addProperty(this.form, '', '');
  }

  deleteProperty(propertyIndex: number) {
    this.propertiesForm.removeAt(propertyIndex);
  }

  private buildForm(deviceId: number, sensor?: Sensor): FormGroup {
    const form = this.formBuilder.group({
      id: [sensor ? sensor.id : undefined],
      name: [sensor ? sensor.name : '', Validators.required],
      deviceId: [deviceId],
      properties: this.formBuilder.array([])
    });

    if (sensor) {
      for (const property of sensor.properties) {
        this.addProperty(form, property.name, property.value);
      }
    }

    return form;
  }

  private addProperty(form: FormGroup, name: String, value: String) {
    const properties = <FormArray>form.get('properties');
    properties.push(this.formBuilder.group({
      name: [name, Validators.required],
      value: [value]
    }));
  }

}
