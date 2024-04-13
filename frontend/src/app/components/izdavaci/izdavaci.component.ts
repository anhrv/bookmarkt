import { Component, OnInit } from '@angular/core';
import {MojConfig} from "../../moj-config";
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../../services/authService";

@Component({
  selector: 'app-izdavaci',
  templateUrl: './izdavaci.component.html',
  styleUrls: ['./izdavaci.component.css']
})
export class IzdavaciComponent implements OnInit {

  constructor(private httpClient:HttpClient, public authService:AuthService) { }

  izdavaci:any[]=[];
  pretragaNaziv="";
  ngOnInit(): void {
    let url=MojConfig.server+"/izdavac";
    this.httpClient.get(url).subscribe((x:any)=>this.izdavaci=x);

  }

  getFiltriraniIzdavaci()
  {
    return this.izdavaci.filter(x=>x.naziv.toLowerCase().startsWith(this.pretragaNaziv.toLowerCase()) || x.naziv.toLowerCase().startsWith(this.pretragaNaziv.toLowerCase()))
  }
}
