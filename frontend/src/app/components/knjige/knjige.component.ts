import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
import {AuthService} from "../../services/authService";

@Component({
  selector: 'app-knjige',
  templateUrl: './knjige.component.html',
  styleUrls: ['./knjige.component.css']
})
export class KnjigeComponent implements OnInit {

  constructor(private httpClient:HttpClient, public authService:AuthService) { }

  knjige:any[]=[];
  pretragaKnjige="";
  pretragaPoAutoru="";
  pretragaPoIzdavacu="";
  pretragaPoISBNu="";
  pretragaPoZanru="";
  cijenaOd:number|null=null;
  cijenaDo:number|null=null;
  brojStranicaOd:number|null=null;
  brojStranicaDo:number|null=null;
  ngOnInit(): void {
    this.httpClient.get(MojConfig.server+"/knjiga").subscribe((x:any)=>
      this.knjige = x
    );
  }
  getFiltriraneKnjige()
  {
    return this.knjige.filter(x=>{
        return (x.naslov.toLowerCase().startsWith(this.pretragaKnjige.toLowerCase())
        && (x.izdavac==null || x.izdavac.naziv.toLowerCase().startsWith(this.pretragaPoIzdavacu.toLowerCase()))
        && (x.zanr==null || x.zanr.naziv.toLowerCase().startsWith(this.pretragaPoZanru.toLowerCase()))
        && x.isbn.startsWith(this.pretragaPoISBNu)
        && ((this.cijenaOd==null || this.cijenaDo==null)||(x.cijena >= this.cijenaOd && x.cijena <= this.cijenaDo))
        && ((this.brojStranicaOd==null || this.brojStranicaDo==null || x.brojStranica==null)||x.brojStranica >= this.brojStranicaOd && x.brojStranica <= this.brojStranicaDo))
        && (this.pretragaPoAutoru=="" || this.sadrziAutora(x.autori, this.pretragaPoAutoru));
      }
    )
  }
  sadrziAutora(autori: any[], pretraga: string): boolean {
    return autori.some((autor:any) => (autor.autor.ime+" "+autor.autor.prezime).toLowerCase().startsWith(pretraga.toLowerCase()) || (autor.autor.prezime+" "+autor.autor.ime).toLowerCase().startsWith(pretraga.toLowerCase())) || autori.length==0;
  }
}
