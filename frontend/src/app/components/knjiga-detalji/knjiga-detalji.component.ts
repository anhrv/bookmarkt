import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {MojConfig} from "../../moj-config";
import {AuthService} from "../../services/authService";
@Component({
  selector: 'app-knjiga-detalji',
  templateUrl: './knjiga-detalji.component.html',
  styleUrls: ['./knjiga-detalji.component.css']
})
export class KnjigaDetaljiComponent implements OnInit {

  constructor(private httpClient:HttpClient,
              private route:ActivatedRoute,
              public authService:AuthService,
              private router:Router) { }

  knjiga:any;

  public id:string=this.route.snapshot.paramMap.get("id")??"";
  public urlRecenzije:string = MojConfig.server+`/knjigaRecenzija/byKnjiga/${this.id}`
  public urlPost:string=MojConfig.server+`/knjigaRecenzija`;
  private url:string=MojConfig.server+`/knjiga/${this.id}`;

  ngOnInit(): void {
    this.httpClient.get(this.url).subscribe((x:any)=>this.knjiga=x);
  }

  izmijeni() {
    this.router.navigate([`/knjige/${this.id}/update`])
  }

  obrisi() {
    let result=window.confirm("Da li ste sigurni da želite obrisati ovu knjigu?");
    if(result)
    {
      this.httpClient.delete(this.url).subscribe((x)=> {
          alert("Knjiga uspješno obrisana!");
          this.router.navigate(["/knjige"])
        }
      )
    }
  }
}
