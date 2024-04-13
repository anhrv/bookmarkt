import { Component, OnInit } from '@angular/core';
import {BrisanjeRequest} from "./brisanje-request";
import {AuthService} from "../../services/authService";
import {Router} from "@angular/router";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
import {LozinkaRequest} from "./lozinka-request";
import {PodaciRequest} from "./podaci-request";

@Component({
  selector: 'app-moj-nalog',
  templateUrl: './moj-nalog.component.html',
  styleUrls: ['./moj-nalog.component.css']
})
export class MojNalogComponent implements OnInit {

  isPodaciVidljivi:boolean=false;
  isLozinkaVidljivi:boolean=false;
  isBrisanjeVidljivi:boolean=false;

  brisanjeRequest:BrisanjeRequest={
    lozinka:""
  }

  lozinkaRequest:LozinkaRequest={
    trenutnaLozinka: "",
    novaLozinka: "",
    novaLozinkaPotvrda: ""
  }

  podaciReuest:PodaciRequest={
    korisnickoIme:"",
    email:"",
    twoFactorEnabled:false
  }

  constructor(private authService:AuthService,
              private router:Router,
              private  httpClient:HttpClient) { }


  logiraniKorisnik:any;

  ngOnInit(): void {
    if(this.authService.isLogiran())
    {
      this.httpClient.get(MojConfig.server+"/nalog/me").subscribe((x:any)=> {
        this.logiraniKorisnik = x
        this.podaciReuest={
          korisnickoIme:x.korisnickoIme,
          email:x.email,
          twoFactorEnabled:x.twoFactorEnabled
        }
      });
    }
  }

  setPodaciVidljivost() {
    this.isPodaciVidljivi = !this.isPodaciVidljivi;
  }

  setLozinkaVidljivost() {
    this.isLozinkaVidljivi=!this.isLozinkaVidljivi;
  }

  setBrisanjeVidljivost() {
    this.isBrisanjeVidljivi=!this.isBrisanjeVidljivi;
  }

  brisanjePotvrdi() {
    let urlDelete = MojConfig.server+"/nalog/deleteMe";
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    const options = {
      headers: headers,
      body: this.brisanjeRequest
    };

    this.httpClient.delete(urlDelete, options).subscribe((x)=>{
      alert("Uspješno obrisan nalog!");
      this.brisanjeOdustani();
      this.authService.logOut();
      this.router.navigate(["/"]);
    })
  }

  brisanjeOdustani() {
    this.brisanjeRequest.lozinka="";
    this.isBrisanjeVidljivi=false;
  }

  lozinkaPotvrdi() {
    let urlLozinka = MojConfig.server+"/auth/updateLozinka";
    this.httpClient.put(urlLozinka, this.lozinkaRequest).subscribe((x)=>{
      alert("Uspješno promijenjena lozinka!\nMorate se ponovo logirati.");
      this.lozinkaOdustani();
      this.authService.logOut();
      this.router.navigate(["login"]);
    })
  }

  lozinkaOdustani() {
    this.lozinkaRequest={
      trenutnaLozinka: "",
      novaLozinka: "",
      novaLozinkaPotvrda: ""
    };
    this.isLozinkaVidljivi=false;
  }

  podaciPotvrdi() {
    let urlPodaci = MojConfig.server+"/nalog/updateMe";
    this.httpClient.put(urlPodaci, this.podaciReuest).subscribe((x)=>{
      alert("Uspješno promijenjeni podaci!");
      this.ngOnInit();
    })
  }

  podaciOdustani() {
    this.podaciReuest={
      korisnickoIme:this.logiraniKorisnik.korisnickoIme,
      email:this.logiraniKorisnik.email,
      twoFactorEnabled:this.logiraniKorisnik.twoFactorEnabled
    }
    this.isPodaciVidljivi=false;
  }

  clickablePodaci(){
    if(this.podaciReuest.korisnickoIme == this.logiraniKorisnik.korisnickoIme &&
       this.podaciReuest.email == this.logiraniKorisnik.email &&
       this.podaciReuest.twoFactorEnabled == this.logiraniKorisnik.twoFactorEnabled)
    {
      return {"pointer-events": "none", "opacity": "0.5"};
    }
    return;
  }
  clickableLozinka()
  {
    if(this.lozinkaRequest.trenutnaLozinka == "" ||
      this.lozinkaRequest.novaLozinka == "" ||
      this.lozinkaRequest.novaLozinkaPotvrda == "")
    {
      return {"pointer-events": "none", "opacity": "0.5"};
    }
    return;
  }

  clickableBrisanje(){
    if(this.brisanjeRequest.lozinka == "")
    {
      return {"pointer-events": "none", "opacity": "0.5"};
    }
    return;
  }
}


