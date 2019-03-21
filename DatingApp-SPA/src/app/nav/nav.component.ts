import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.less']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(
      next => { console.log('Logged in succesfully'); },
      error => { console.log('Failed to login'); }
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token; // se la variabile è vuota ritorna false, altrimenti true
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logged out');
  }
}
