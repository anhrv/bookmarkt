import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
import {ForgotRequest} from "./forgot-request";

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  public forgotRequest: ForgotRequest = {
    email:""
  };
  constructor(private httpClient:HttpClient) { }

  ngOnInit(): void {
  }

  potvrdi() {
    let url=MojConfig.server+`/auth/forgotLozinka`;

    this.httpClient.post(url, this.forgotRequest).subscribe((x)=>{
        alert("Provjerite Vaš mail!")
      }
    )}

}
