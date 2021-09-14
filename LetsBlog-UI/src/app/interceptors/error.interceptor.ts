import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private toastr: ToastrService, private accountService: AccountService) {}

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        return next.handle(request).pipe(
            catchError((error) => {
                if (error) {
                    switch (error.status) {
                        case 400:
                            this.handle400Error(error);
                            break;
                        case 401:
                            this.handle401Error(error);
                            break;
                        case 500:
                            this.handle500Error(error);
                            break;
                        default:
                            this.handleUnexpectedError(error);
                    }
                }
                return throwError(error);
            })
        );
    }
    handle400Error(error: any) {
        if (!!error.error && Array.isArray(error.error)) {
            let errorMessage = '';
            for (const key in error.error) {
                if (!!error.error[key]) {
                    const errorElement = error.error[key];
                    errorMessage = `${errorMessage}${errorElement.code} - ${errorElement.Description}\n`;
                }
            }
            this.toastr.error(errorMessage, error.statusText);
            console.log(error.error);
        } else if (!!error?.error.errors?.Content && typeof error.error.errors.Content === 'object') {
            let errorObject = error.error.errors.Content;
            let errorMessage = '';
            for (const key in errorObject) {
                const errorElement = errorObject[key];
                errorMessage = `${errorMessage}${errorElement}\n`;
            }
            this.toastr.error(errorMessage, error.statusCode);
            console.log(error.error);
        } else if (!!error.error) {
            let errorMessage = typeof error.error === 'string' ? error.error : 'There was a validation error.';
            this.toastr.error(errorMessage, error.statusCode);
            console.log(error.error);
        } else {
            this.toastr.error(error.statusText, error.status);
            console.log(error);
        }
    }
    handle401Error(error: any) {
        let errorMessage = 'Please login into your account.';
        this.accountService.logout();
        this.toastr.error(errorMessage, error.statusText);
    }
    handle500Error(error: any) {
        this.toastr.error('Please contact your administrator. An error has happened in the server.');
        console.log(error);
    }
    handleUnexpectedError(error: any) {
        this.toastr.error('Something unexpected happened');
        console.log(error);
    }
}