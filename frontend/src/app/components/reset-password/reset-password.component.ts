import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../services/authService";
import {MojConfig} from "../../moj-config";
import {ResetRequest} from "./reset-request";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  public resetRequest: ResetRequest = {
    novaLozinka:"",
    novaLozinkaPotvrda:""
  };
  constructor(private httpClient:HttpClient,
              private router: Router,
              private authService:AuthService,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  potvrdi() {
    let url=MojConfig.server+`/auth/resetLozinka/${this.route.snapshot.paramMap.get("token")}`;

    this.httpClient.post(url, this.resetRequest).subscribe((x)=>{
        this.authService.setLogiraniKorisnik(x);
        this.router.navigate(["/"])
      }
    )}

}
