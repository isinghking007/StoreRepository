import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { CommonservicesService } from './commonservices.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: CommonservicesService, private router: Router) {}

  canActivate(): boolean {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }
}
