import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
 user$ = this.auth.user$;
 
 

  constructor(private auth: AuthService, private router: Router) {}


  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
