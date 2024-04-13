import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
import {AuthService} from "../../services/authService";

@Component({
  selector: 'app-autori',
  templateUrl: './autori.component.html',
  styleUrls: ['./autori.component.css']
})
export class AutoriComponent implements OnInit {

  constructor(private httpClient:HttpClient, public authService:AuthService) { }

  autori:any[]=[];
  pretragaImePrezime="";
  ngOnInit(): void {
    let url=MojConfig.server+"/autor";
    this.httpClient.get(url).subscribe((x:any)=>this.autori=x);
  }

  getFiltriraniAutori()
  {
    return this.autori.filter(x=>(x.ime + ' ' + x.prezime).toLowerCase().startsWith(this.pretragaImePrezime.toLowerCase()) || (x.prezime + ' ' + x.ime).toLowerCase().startsWith(this.pretragaImePrezime.toLowerCase()))
  }
}
