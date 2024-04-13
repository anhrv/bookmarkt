import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from './authService';
@Injectable()
export class UposlenikGuardService implements CanActivate {
  constructor(public auth: AuthService, public router: Router) {}
  canActivate(): boolean {
    if (!this.auth.isUloga("Uposlenik")) {
      alert("Nemate pristup ovoj akciji!");
      this.router.navigate(['/']);
      return false;
    }
    return true;
  }
}
