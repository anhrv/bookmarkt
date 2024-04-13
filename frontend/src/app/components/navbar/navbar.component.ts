import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../services/authService";
import {Router} from "@angular/router";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private router: Router,
              public authService:AuthService) { }
  ngOnInit(): void {
  }
  logOut()
  {
    let result = window.confirm("Da li ste sigurni da se želite odjaviti?")
    if(result) {
      this.authService.logOut();
      this.router.navigate(["/"]);
    }
  }
}
