import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatSort } from '@angular/material';

import { Role } from '../role';
import { RoleService } from '../role.service';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styles: [`
  .bar {
    padding-bottom: 1em;
  }`]
})
export class RoleListComponent implements OnInit, AfterViewInit {
  displayedColumns = ['name', 'actions'];
  dataSource: MatTableDataSource<Role>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private roleService: RoleService
  ) {
    this.dataSource = new MatTableDataSource<Role>([]);
  }

  ngOnInit() {
    this.roleService.getAll().subscribe(roles => {
      this.dataSource.data = roles;
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

}
