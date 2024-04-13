import {Component, Input, OnInit} from '@angular/core';
import {RecenzijaRequest} from "./recenzija-request";
import {ActivatedRoute, Router} from "@angular/router";
import {MojConfig} from "../../moj-config";
import {AuthService} from "../../services/authService";
import {HttpClient} from "@angular/common/http";
import {RecenzijaUpdateRequest} from "./recenzija-update-request";

@Component({
  selector: 'app-recenzije',
  templateUrl: './recenzije.component.html',
  styleUrls: ['./recenzije.component.css']
})
export class RecenzijeComponent implements OnInit {

  recenzijeResponse: any[] = [];
  isDodajVidljiv:boolean=false;
  isUpdateVidljiv:boolean=false;
  userID:string="";

  get recenzije() {
    return this.recenzijeResponse.slice().sort((a, b) => {
      if (this.userID && a.nalog.nalogID === this.userID) {
        return -1;
      } else if (this.userID && b.nalog.nalogID === this.userID) {
        return 1;
      } else {
        return 0;
      }
    });
  }

  @Input() id:any="";
  @Input() urlRecenzije:any="";
  @Input() urlPost:any="";
  @Input() isKnjigaRecenzija:boolean=false;
  @Input() isAutorRecenzija:boolean=false;
  @Input() isIzdavacRecenzija:boolean=false;


  recRequest:RecenzijaRequest={
    ocjena:null,
    tekst:"",
    stvarID: this.route.snapshot.paramMap.get("id")??""
  }
  recUpdateRequest:RecenzijaUpdateRequest={
    ocjena:0,
    tekst:""
  }
  recenzijaID:string="";
  constructor(private route:ActivatedRoute,
              public authService:AuthService,
              private router:Router,
              private httpClient:HttpClient,) { }
  ngOnInit(): void {
    this.httpClient.get(this.urlRecenzije).subscribe((x:any)=>this.recenzijeResponse=x);
    if(this.authService.isLogiran())
    {
      this.httpClient.get(MojConfig.server+"/nalog/me").subscribe((x:any)=>this.userID=x.nalogID);
    }
  }
  dodaj() {
    if(!this.authService.isLogiran())
    {
      this.router.navigate(["/login"])
    }
    else {
      this.isDodajVidljiv=true;
    }
  }

  odustani()
  {
    this.recRequest= {
      ocjena: null,
      tekst: "",
      stvarID: this.id
    }
    this.isDodajVidljiv=false;
  }

  dodajRecenziju()
  {
    this.httpClient.post(this.urlPost, this.recRequest).subscribe((x)=>{
        alert("Recenzija uspješno dodana!")
        this.odustani();
        this.httpClient.get(this.urlRecenzije).subscribe((x:any)=>this.recenzijeResponse=x);
      }
    )}

  snimiRecenziju() {
    this.httpClient.put(this.urlPost+"/"+this.recenzijaID, this.recUpdateRequest).subscribe((x)=> {
      alert("Recenzija uspješno izmijenjena!");
      this.odustaniUpdate();
      this.httpClient.get(this.urlRecenzije).subscribe((x:any)=>this.recenzijeResponse=x);
    }
    )}

  odustaniUpdate() {
    this.recUpdateRequest={
      ocjena:null,
      tekst:""
    }
    this.isUpdateVidljiv=false;
  }

  izmjeni(recenzija:any) {
    this.recUpdateRequest={
      ocjena:recenzija.ocjena,
      tekst:recenzija.tekst
    }
    if(this.isKnjigaRecenzija) this.recenzijaID=recenzija.knjigaRecenzijaID
    else if(this.isIzdavacRecenzija) this.recenzijaID=recenzija.izdavacRecenzijaID
    else if(this.isAutorRecenzija) this.recenzijaID=recenzija.autorRecenzijaID
    this.isUpdateVidljiv=true;
  }

  obrisi(recenzija:any) {
    if(this.isKnjigaRecenzija) this.recenzijaID=recenzija.knjigaRecenzijaID
    else if(this.isIzdavacRecenzija) this.recenzijaID=recenzija.izdavacRecenzijaID
    else if(this.isAutorRecenzija) this.recenzijaID=recenzija.autorRecenzijaID
    let result=window.confirm("Da li ste sigurni da želite obrisati ovu recenziju?");
    if(result)
    {
      this.httpClient.delete(this.urlPost+"/"+this.recenzijaID).subscribe((x)=> {
          alert("Recenzija uspješno obrisana!");
          this.httpClient.get(this.urlRecenzije).subscribe((x:any)=>this.recenzijeResponse=x);
        }
      )
    }
  }
}
