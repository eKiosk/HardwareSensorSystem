import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { User } from '../user';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styles: [`
  .bar {
    padding-bottom: 1em;
  }`]
})
export class UserListComponent implements OnInit, AfterViewInit {
  displayedColumns = ['userName', 'email', 'actions'];
  dataSource: MatTableDataSource<User>;
  roleId: number;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  private subscription: Subscription;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute
  ) {
    this.dataSource = new MatTableDataSource<User>([]);
  }

  ngOnInit() {
    this.subscription = this.route.params.subscribe(params => {
      this.roleId = params.id;
      this.userService.getAllByRoleId(this.roleId).subscribe(users => {
        this.dataSource.data = users;
      });
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

}
