import { Component, OnInit } from '@angular/core';
import {KnjigaDodajRequest} from "./knjiga-dodaj-request";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
import {Router} from "@angular/router";

@Component({
  selector: 'app-knjiga-dodaj',
  templateUrl: './knjiga-dodaj.component.html',
  styleUrls: ['./knjiga-dodaj.component.css']
})
export class KnjigaDodajComponent implements OnInit {

  constructor(private httpClient:HttpClient,
              private router:Router) { }

  knjigaDodajRequest:KnjigaDodajRequest={
    naslov: "",
    isbn: "",
    opis: "",
    brojStranica:null,
    cijena:null,
    datumIzdavanja:null,
    naStanju:null,
    slikaPutanja: "/assets/images/default.jpg",
    izdavacID: "",
    zanrID: "",
    autorIDs: []
  }

  autoriResponse:any[]=[];
  izdavaciResponse:any[]=[];
  zanroviResponse:any[]=[];


  ngOnInit(): void {
    this.httpClient.get(MojConfig.server+"/autor").subscribe((x:any)=>this.autoriResponse=x);
    this.httpClient.get(MojConfig.server+"/izdavac").subscribe((x:any)=>this.izdavaciResponse=x);
    this.httpClient.get(MojConfig.server+"/zanr").subscribe((x:any)=>this.zanroviResponse=x);
  }

  potvrdi() {
    this.knjigaDodajRequest.izdavacID = this.knjigaDodajRequest.izdavacID == "" ? null : this.knjigaDodajRequest.izdavacID;
    this.knjigaDodajRequest.zanrID = this.knjigaDodajRequest.zanrID == "" ? null : this.knjigaDodajRequest.zanrID;
    this.knjigaDodajRequest.datumIzdavanja = this.knjigaDodajRequest.datumIzdavanja?.toString() === "" ? null : this.knjigaDodajRequest.datumIzdavanja;
    this.httpClient.post(MojConfig.server+"/knjiga", this.knjigaDodajRequest).subscribe((x:any)=>{
      alert("Uspješno dodana knjiga!");
      this.odustani();
    })
  }

  odustani() {
    this.router.navigate(["/knjige"])
  }
}
