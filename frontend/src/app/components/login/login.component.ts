import { Component, OnInit } from '@angular/core';
import {LoginRequest} from "./login-request";
import {MojConfig} from "../../moj-config";
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../../services/authService";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public loginRequest: LoginRequest = {
    email:"",
    lozinka:""
  };
  constructor(private httpClient:HttpClient,
              private authService:AuthService,
              private router: Router) { }

  ngOnInit(): void {
  }

  logIn() {
    let url=MojConfig.server+`/auth/login`;

    this.httpClient.post(url, this.loginRequest).subscribe((x:any)=> {
        if (!x.is2faEnabled) {
          this.authService.setLogiraniKorisnik(x.token);
          window.history.back();
        }
        else
        {
          this.router.navigate([`/login/2fa/${x.token}`]);
        }

      }
    )}
}
