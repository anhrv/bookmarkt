import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {IzdavacDodajRequest} from "../izdavac-dodaj/izdavac-dodaj-request";
import {MojConfig} from "../../moj-config";

@Component({
  selector: 'app-izdavac-update',
  templateUrl: './izdavac-update.component.html',
  styleUrls: ['./izdavac-update.component.css']
})
export class IzdavacUpdateComponent implements OnInit {
  constructor(private httpClient:HttpClient,
              private route:ActivatedRoute
              ) { }

  public id:string=this.route.snapshot.paramMap.get("id")??"";
  izdavac:any

  izdavacUpdateRequest:IzdavacDodajRequest={
    naziv:"",
    email:"",
    telefon:"",
    adresa:"",
    slikaPutanja:"/assets/images/default.jpg"
  }

  ngOnInit(): void {
    this.httpClient.get(MojConfig.server+`/izdavac/${this.id}`).subscribe((x:any)=>
      this.izdavacUpdateRequest={
        naziv:x.naziv,
        email:x.email,
        telefon:x.telefon,
        adresa:x.adresa,
        slikaPutanja:x.slika.slikaPutanja
      });
  }
  potvrdi() {
    this.httpClient.put(MojConfig.server+`/izdavac/${this.id}`, this.izdavacUpdateRequest).subscribe((x)=>{
      alert("Podaci uspješno izmijenjeni!");
      this.odustani();
    })
  }

  odustani() {
    window.history.back();
  }

}
