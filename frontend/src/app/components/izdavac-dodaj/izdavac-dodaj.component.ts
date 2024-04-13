import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
import {IzdavacDodajRequest} from "./izdavac-dodaj-request";

@Component({
  selector: 'app-izdavac-dodaj',
  templateUrl: './izdavac-dodaj.component.html',
  styleUrls: ['./izdavac-dodaj.component.css']
})
export class IzdavacDodajComponent implements OnInit {

  constructor(private router:Router,
              private httpClient:HttpClient
  ) { }
  izdavacDodajRequest:IzdavacDodajRequest={
    naziv:"",
    email:"",
    telefon:"",
    adresa:"",
    slikaPutanja:"/assets/images/default.jpg"
  }

  ngOnInit(): void {
  }
  potvrdi() {
    this.httpClient.post(MojConfig.server+"/izdavac", this.izdavacDodajRequest).subscribe((x)=>{
      alert("Uspješno dodan izdavač!");
      this.odustani();
    })
  }

  odustani() {
    window.history.back();
  }
}
