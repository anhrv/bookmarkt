import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AutoriComponent } from './components/autori/autori.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import { IzdavaciComponent } from './components/izdavaci/izdavaci.component';
import {RouterModule} from "@angular/router";
import { KnjigeComponent } from './components/knjige/knjige.component';
import {FormsModule} from "@angular/forms";
import { AutorDetaljiComponent } from './components/autor-detalji/autor-detalji.component';
import { IzdavacDetaljiComponent } from './components/izdavac-detalji/izdavac-detalji.component';
import { KnjigaDetaljiComponent } from './components/knjiga-detalji/knjiga-detalji.component';
import { LoginComponent } from './components/login/login.component';
import {CookieService} from "ngx-cookie-service";
import { RegistracijaComponent } from './components/registracija/registracija.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import {AuthInterceptor} from "./services/authInterceptorService";
import { RecenzijeComponent } from './components/recenzije/recenzije.component';
import { MojNalogComponent } from './components/moj-nalog/moj-nalog.component';
import {AuthGuardService} from "./services/authGuardService";
import {ErrorInterceptorService} from "./services/errorInterceptorService";
import { KnjigaDodajComponent } from './components/knjiga-dodaj/knjiga-dodaj.component';
import {UposlenikGuardService} from "./services/uposlenikGuardService";
import { AutorDodajComponent } from './components/autor-dodaj/autor-dodaj.component';
import { IzdavacDodajComponent } from './components/izdavac-dodaj/izdavac-dodaj.component';
import { AutorUpdateComponent } from './components/autor-update/autor-update.component';
import {DatePipe} from "@angular/common";
import { IzdavacUpdateComponent } from './components/izdavac-update/izdavac-update.component';
import { KnjigaUpdateComponent } from './components/knjiga-update/knjiga-update.component';
import { LoginTwofactorComponent } from './components/login-twofactor/login-twofactor.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    AutoriComponent,
    IzdavaciComponent,
    KnjigeComponent,
    AutorDetaljiComponent,
    IzdavacDetaljiComponent,
    KnjigaDetaljiComponent,
    LoginComponent,
    RegistracijaComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    RecenzijeComponent,
    MojNalogComponent,
    KnjigaDodajComponent,
    AutorDodajComponent,
    IzdavacDodajComponent,
    AutorUpdateComponent,
    IzdavacUpdateComponent,
    KnjigaUpdateComponent,
    LoginTwofactorComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot([
      {path: '', redirectTo: 'knjige', pathMatch: 'full'},
      {path: 'knjige', component: KnjigeComponent},
      {path: 'autori', component: AutoriComponent},
      {path: 'izdavaci', component: IzdavaciComponent},
      {path: 'autori/:id', component: AutorDetaljiComponent},
      {path: 'izdavaci/:id', component: IzdavacDetaljiComponent},
      {path: 'knjige/:id', component: KnjigaDetaljiComponent},
      {path: 'login', component: LoginComponent},
      {path: 'login/2fa/:id', component: LoginTwofactorComponent},
      {path: 'registracija', component: RegistracijaComponent},
      {path: 'forgot-password', component: ForgotPasswordComponent},
      {path: 'reset-password/:token', component: ResetPasswordComponent},
      {path: 'moj-nalog', component: MojNalogComponent, canActivate:[AuthGuardService]},
      {path: 'knjiga/dodaj', component: KnjigaDodajComponent, canActivate:[UposlenikGuardService]},
      {path: 'knjige/:id/update', component: KnjigaUpdateComponent, canActivate:[UposlenikGuardService]},
      {path: 'autor/dodaj', component: AutorDodajComponent, canActivate:[UposlenikGuardService]},
      {path: 'autori/:id/update', component: AutorUpdateComponent, canActivate:[UposlenikGuardService]},
      {path: 'izdavac/dodaj', component: IzdavacDodajComponent, canActivate:[UposlenikGuardService]},
      {path: 'izdavaci/:id/update', component: IzdavacUpdateComponent, canActivate:[UposlenikGuardService]},
    ]),
    FormsModule
  ],
  providers: [
    CookieService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true },
    AuthGuardService,
    UposlenikGuardService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
