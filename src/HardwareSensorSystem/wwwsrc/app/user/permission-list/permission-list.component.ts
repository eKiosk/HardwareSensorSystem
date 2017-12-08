import { Component, OnInit } from '@angular/core';
import { MatSlideToggleChange } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import { Permission } from '../permission';
import { PermissionService } from '../permission.service';

@Component({
  selector: 'app-permission-list',
  templateUrl: './permission-list.component.html',
  styles: []
})
export class PermissionListComponent implements OnInit {
  permissions: Observable<Permission[]>;
  rolePermissionIds: number[];
  private subscription: Subscription;
  private roleId: number;

  constructor(
    private permissionService: PermissionService,
    route: ActivatedRoute
  ) {
    this.subscription = route.params.subscribe(params => {
      this.roleId = params.id;
      this.permissionService.getIdsByRole(this.roleId).subscribe(ids => {
        this.rolePermissionIds = ids;
      });
    });
  }

  ngOnInit() {
    this.permissions = this.permissionService.getAll();
  }

  save(event: MatSlideToggleChange) {
    console.log(event);
    let request: Observable<void>;

    if (event.checked) {
      request = this.permissionService.addToRole(this.roleId, Number(event.source.id));
    } else {
      request = this.permissionService.removeFromRole(this.roleId, Number(event.source.id));
    }

    request.subscribe(() => { });
  }

}
