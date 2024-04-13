import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
import {ActivatedRoute} from "@angular/router";
import {AutorDodajRequest} from "../autor-dodaj/autor-dodaj-request";

@Component({
  selector: 'app-autor-update',
  templateUrl: './autor-update.component.html',
  styleUrls: ['./autor-update.component.css']
})
export class AutorUpdateComponent implements OnInit {

  constructor(private httpClient:HttpClient,
              private route:ActivatedRoute) { }

  public id:string=this.route.snapshot.paramMap.get("id")??"";
  autor:any

  autorUpdateRequest:AutorDodajRequest={
    ime:"",
    prezime:"",
    datumRodjenja:null,
    drzava:"",
    biografija:"",
    slikaPutanja:"/assets/images/default.jpg"
  }
  ngOnInit(): void {
    this.httpClient.get(MojConfig.server+`/autor/${this.id}`).subscribe((x:any)=>
      this.autorUpdateRequest={
      ime:x.ime,
      prezime:x.prezime,
      drzava:x.drzava,
      datumRodjenja:x.datumRodjenja,
      biografija:x.biografija,
      slikaPutanja:x.slika.slikaPutanja
    });
  }

  potvrdi() {
    this.autorUpdateRequest.datumRodjenja = this.autorUpdateRequest.datumRodjenja?.toString() === "" ? null : this.autorUpdateRequest.datumRodjenja;
    this.httpClient.put(MojConfig.server+`/autor/${this.id}`, this.autorUpdateRequest).subscribe(x=>
    {
      alert("Podaci uspješno izmijenjeni!");
      this.odustani();
    })
  }

  odustani() {
    window.history.back();
  }
}
