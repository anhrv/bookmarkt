import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {MojConfig} from "../../moj-config";
import {AuthService} from "../../services/authService";

@Component({
  selector: 'app-izdavac-detalji',
  templateUrl: './izdavac-detalji.component.html',
  styleUrls: ['./izdavac-detalji.component.css']
})
export class IzdavacDetaljiComponent implements OnInit {

  constructor(private httpClient:HttpClient,
              private route:ActivatedRoute,
              private router:Router,
              public authService:AuthService) { }

  izdavac:any;

  public id:string=this.route.snapshot.paramMap.get("id")??"";
  public urlRecenzije:string = MojConfig.server+`/izdavacRecenzija/byIzdavac/${this.id}`
  public urlPost:string=MojConfig.server+`/izdavacRecenzija`;
  private url:string=MojConfig.server+`/izdavac/${this.id}`;

  ngOnInit(): void {
    this.httpClient.get(this.url).subscribe((x:any)=>this.izdavac=x);
  }
  izmijeni() {
    this.router.navigate([`/izdavaci/${this.id}/update`])
  }

  obrisi() {
    let result=window.confirm("Da li ste sigurni da želite obrisati ovog izdavača?");
    if(result)
    {
      this.httpClient.delete(this.url).subscribe((x)=> {
          alert("Izdavač uspješno obrisan!");
          this.router.navigate(["/izdavaci"])
        }
      )
    }
  }
}
