import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../security/authentication.service';

@Component({
  selector: 'app-logout',
  template: '<div></div>'
})
export class LogoutComponent {

  constructor(
    router: Router,
    authenticationService: AuthenticationService
  ) {
    authenticationService.logout();
    router.navigate(['/login']);
  }

}
