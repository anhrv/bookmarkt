import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {CookieService} from "ngx-cookie-service";
import {MojConfig} from "../moj-config";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({providedIn: 'root'})
export class AuthService{
  constructor(private cookieService: CookieService,
              public httpClient:HttpClient) {
  }

  private jwtService=new JwtHelperService();

  isLogiran():boolean{
    return this.getJWT() != "" && !this.jwtService.isTokenExpired(this.getJWT());
  }

  isUloga(uloga: string): boolean {
    const token = this.getJWT();
    const decodedToken = this.jwtService.decodeToken(token);
    return decodedToken && decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === uloga;
  }

  getJWT():any | null {
    const token = this.cookieService.get('jwt');
    return token;
  }
  setLogiraniKorisnik(x:any | null) {
    const expirationDate = new Date();
    expirationDate.setDate(expirationDate.getDate() + 1);
    if(x==null)
    {
      this.cookieService.set('jwt', "", expirationDate, '/', '', true);
    }
    else
    {
      this.cookieService.set('jwt', x, expirationDate, '/', '', true);
    }
  }
  logOut()
  {
    this.cookieService.delete('jwt', "/");
  }
}
