import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {KnjigaDodajRequest} from "../knjiga-dodaj/knjiga-dodaj-request";
import {MojConfig} from "../../moj-config";
@Component({
  selector: 'app-knjiga-update',
  templateUrl: './knjiga-update.component.html',
  styleUrls: ['./knjiga-update.component.css']
})
export class KnjigaUpdateComponent implements OnInit {
  constructor(private httpClient:HttpClient,
              private router:Router,
              private route:ActivatedRoute) { }

  public id:string=this.route.snapshot.paramMap.get("id")??"";
  knjiga:any;

  knjigaUpdateRequest:KnjigaDodajRequest={
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
    this.httpClient.get(MojConfig.server+`/knjiga/${this.id}`).subscribe((x:any)=>{
      this.knjigaUpdateRequest={
        naslov: x.naslov,
        isbn: x.isbn,
        opis: x.opis,
        brojStranica:x.brojStranica,
        cijena:x.cijena,
        datumIzdavanja:x.datumIzdavanja,
        naStanju:x.naStanju,
        slikaPutanja: x.slika.slikaPutanja,
        izdavacID: x.izdavac?.izdavacID??"",
        zanrID: x.zanr?.zanrID??"",
        autorIDs: x.autori.map((autor:any) => autor.autor.autorID)
      }
    })
  }

  potvrdi() {
    this.knjigaUpdateRequest.izdavacID = this.knjigaUpdateRequest.izdavacID == "" ? null : this.knjigaUpdateRequest.izdavacID;
    this.knjigaUpdateRequest.zanrID = this.knjigaUpdateRequest.zanrID == "" ? null : this.knjigaUpdateRequest.zanrID;
    this.knjigaUpdateRequest.datumIzdavanja = this.knjigaUpdateRequest.datumIzdavanja?.toString() === "" ? null : this.knjigaUpdateRequest.datumIzdavanja;
    this.httpClient.put(MojConfig.server+`/knjiga/${this.id}`, this.knjigaUpdateRequest).subscribe((x:any)=>{
      alert("Podaci uspješno izmijenjeni!");
      this.odustani();
    })
  }

  odustani() {
    window.history.back();
  }
}
