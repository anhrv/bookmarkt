import { Component, OnInit } from '@angular/core';
import {AutorDodajRequest} from "./autor-dodaj-request";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";

@Component({
  selector: 'app-autor-dodaj',
  templateUrl: './autor-dodaj.component.html',
  styleUrls: ['./autor-dodaj.component.css']
})
export class AutorDodajComponent implements OnInit {

  constructor(private router:Router,
              private httpClient:HttpClient
              ) { }

  autorDodajRequest:AutorDodajRequest={
    ime:"",
    prezime:"",
    datumRodjenja:null,
    drzava:"",
    biografija:"",
    slikaPutanja:"/assets/images/default.jpg"
  }

  ngOnInit(): void {
  }
  potvrdi() {
    this.autorDodajRequest.datumRodjenja = this.autorDodajRequest.datumRodjenja?.toString() === "" ? null : this.autorDodajRequest.datumRodjenja;
    this.httpClient.post(MojConfig.server+"/autor", this.autorDodajRequest).subscribe((x)=>{
      alert("Uspješno dodan autor!");
      this.odustani();
    })
  }

  odustani() {
    window.history.back();
  }
}
