import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-layout',
  templateUrl: 'layout.component.html',
  styles: [`
  mat-sidenav-container {
    height: 100%;
  }

  .content {
    padding: 1em;
  }`]
})
export class LayoutComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
