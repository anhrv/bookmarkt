import { Component, OnInit } from '@angular/core';
import {MojConfig} from "../../moj-config";
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../../services/authService";
import {Login2faRequest} from "./login-2fa-request";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-login-twofactor',
  templateUrl: './login-twofactor.component.html',
  styleUrls: ['./login-twofactor.component.css']
})
export class LoginTwofactorComponent implements OnInit {

  public login2faRequest: Login2faRequest = {
    nalogId:"",
    twoFactorCode:""
  };
  constructor(private httpClient:HttpClient,
              private authService:AuthService,
              private router: Router,
              private route: ActivatedRoute) { }
  ngOnInit(): void {
    this.login2faRequest.nalogId=this.route.snapshot.paramMap.get("id")??"";
  }

  logIn() {
    console.log(this.login2faRequest);
    let url=MojConfig.server+`/auth/login2fa`;

    this.httpClient.post(url, this.login2faRequest).subscribe((x)=>{
        this.authService.setLogiraniKorisnik(x);
        this.router.navigate(["/"]);
      }
    )
    }
}
