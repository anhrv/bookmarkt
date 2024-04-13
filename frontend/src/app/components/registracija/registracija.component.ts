import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../services/authService";
import {MojConfig} from "../../moj-config";
import {RegRequest} from "./reg-request";

@Component({
  selector: 'app-registracija',
  templateUrl: './registracija.component.html',
  styleUrls: ['./registracija.component.css']
})
export class RegistracijaComponent implements OnInit {

  public regRequest: RegRequest = {
    korisnickoIme:"",
    email:"",
    lozinka:""
  };
  constructor(private httpClient:HttpClient,
              private router: Router,
              private authService:AuthService,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  registracija() {
    let url=MojConfig.server+`/auth/registracija`;

    this.httpClient.post(url, this.regRequest).subscribe((x)=> {
        this.authService.setLogiraniKorisnik(x);
        this.router.navigate(["/"]);
      }
    )}

}
