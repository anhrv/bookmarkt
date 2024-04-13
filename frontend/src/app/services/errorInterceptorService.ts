import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable, of, throwError} from 'rxjs';
import { catchError } from 'rxjs/operators';
import {Router} from "@angular/router";
import {AuthService} from "./authService";

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService implements HttpInterceptor {

  constructor(private router:Router, private authService:AuthService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler):Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        catchError((error) => {
          switch (error.status) {
            case 400: {
              if (typeof error.error === 'string') {
                alert(error.error);
              } else if (error.error instanceof Object && 'type' in error.error) {
                const errorData: ErrorData = {
                  type: error.error.type,
                  title: error.error.title,
                  status: error.error.status,
                  errors: error.error.errors,
                  traceId: error.error.traceId,
                };
                let message: string = this.concatenateErrorMessages(errorData)
                alert(message);
              }
              return of(error);
              break;
            }
            case 401: {
              if (typeof error.error === 'string') {
                alert(error.error);
              }
              else {
                alert("Morate se logirati!");
              }
              this.authService.logOut();
              this.router.navigate(["/login"]);
              return of(error);
              break;
            }
            case 403: {
              alert("Nemate pristup ovoj akciji!");
              return of(error);
              break;
            }
            case 500: {
              alert("Nepoznata greška.");
              return of(error);
              break;
            }
            case 0:{
              alert("Greška na serveru.");
              return of(error);
              break;
            }
            default: {
              return of(error);
              break;
            }
          }
        }))
  }

  private concatenateErrorMessages(errorData: ErrorData) {
    const errorMessages: string[] = [];

    for (const field in errorData.errors) {
      if (errorData.errors.hasOwnProperty(field)) {
        const messages = errorData.errors[field];
        errorMessages.push(...messages);
      }
    }
    return errorMessages.join('\n');
  }
}

  interface ErrorData {
  type: string;
  title: string;
  status: number;
  errors: Record<string, string[]>;
  traceId: string;
}
