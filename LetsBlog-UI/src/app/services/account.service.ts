import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApplicationUser } from '../models/account/application-user.model';
import { ApplicationUserLogin } from '../models/account/application-user-login.model';
import { environment } from 'src/environments/environment';
import { ApplicationUserCreate } from '../models/account/application-user-create.model';

@Injectable({
    providedIn: 'root',
})
export class AccountService {
    private currentUserSubject$: BehaviorSubject<ApplicationUser> | null;
    constructor(private http: HttpClient) {
        this.currentUserSubject$ = new BehaviorSubject<ApplicationUser>(JSON.parse(localStorage.getItem('letsBlog-currentUser') || ''));
    }

    login(model: ApplicationUserLogin): Observable<ApplicationUser> {
        return this.http.post<ApplicationUser>(`${environment.webApi}/Account/login`, model).pipe(
            map((user: ApplicationUser) => {
                if (user) {
                    localStorage.setItem('letsBlog-currentUser', JSON.stringify(user));
                    this.setCurrentUser(user);
                }
                return user;
            })
        );
    }

    register(model: ApplicationUserCreate): Observable<ApplicationUser> {
        return this.http.post<ApplicationUser>(`${environment.webApi}/Account/register`, model).pipe(
            map((user: ApplicationUser) => {
                if (user) {
                    localStorage.setItem('letsBlog-currentUser', JSON.stringify(user));
                    this.setCurrentUser(user);
                }
                return user;
            })
        );
    }

    setCurrentUser(user: ApplicationUser) {
        if (this.currentUserSubject$ != null) {
            this.currentUserSubject$.next(user);
        } else {
            this.currentUserSubject$ = new BehaviorSubject<ApplicationUser>(user);
        }
    }

    public get currentUserValue(): ApplicationUser | null {
        if (this.currentUserSubject$ != null) {
            return this.currentUserSubject$.value;
        } else {
            return null;
        }
    }

    logout() {
        localStorage.removeItem('letsBlog-currentUser');
        if (this.currentUserSubject$ != null) {
            this.currentUserSubject$ = null;
        }
    }
}
