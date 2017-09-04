import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  template: ``
})
export class LogoutComponent {

  constructor(router: Router) {
    router.navigate(['login']);
  }

}
