import {Component,OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../../moj-config";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../services/authService";

@Component({
  selector: 'app-autor-detalji',
  templateUrl: './autor-detalji.component.html',
  styleUrls: ['./autor-detalji.component.css']
})
export class AutorDetaljiComponent implements OnInit {

  constructor(private httpClient:HttpClient,
              private route:ActivatedRoute,
              private router:Router,
              public authService:AuthService
              ) { }

  autor:any;

  public id:string=this.route.snapshot.paramMap.get("id")??"";
  public urlRecenzije:string = MojConfig.server+`/autorRecenzija/byAutor/${this.id}`
  public urlPost:string=MojConfig.server+`/autorRecenzija`;
  private url:string=MojConfig.server+`/autor/${this.id}`;
  ngOnInit(): void {
    this.httpClient.get(this.url).subscribe((x:any)=>this.autor=x);
  }

  izmijeni() {
    this.router.navigate([`/autori/${this.id}/update`])
  }

  obrisi() {
    let result=window.confirm("Da li ste sigurni da želite obrisati ovog autora?");
    if(result)
    {
      this.httpClient.delete(this.url).subscribe((x)=> {
          alert("Autor uspješno obrisan!");
          this.router.navigate(["/autori"])
        }
      )
    }
  }
}
