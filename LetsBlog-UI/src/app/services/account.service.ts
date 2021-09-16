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
    private currentUserSubject$: BehaviorSubject<ApplicationUser | null>;

    constructor(private http: HttpClient) {
        this.currentUserSubject$ = new BehaviorSubject<ApplicationUser | null>(JSON.parse(localStorage.getItem('letsBlog-currentUser') || 'null'));
    }

    login(model: ApplicationUserLogin): Observable<ApplicationUser> {
        return this.http.post<ApplicationUser>(`${environment.webApi}/Account/login`, model).pipe(
            map((user: ApplicationUser): ApplicationUser => {
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
            map((user: ApplicationUser): ApplicationUser => {
                if (user) {
                    localStorage.setItem('letsBlog-currentUser', JSON.stringify(user));
                    this.setCurrentUser(user);
                }

                return user;
            })
        );
    }

    setCurrentUser(user: ApplicationUser) {
        this.currentUserSubject$.next(user);
    }

    public get currentUserValue(): ApplicationUser | null {
        return this.currentUserSubject$.value;
    }

    public givenUserIsLoggedIn(username: string) {
        if (this.currentUserValue != null) {
            return this.isLoggedIn() && this.currentUserValue.username === username;
        }
        return false;
    }

    public isLoggedIn() {
        const currentUser = this.currentUserValue;
        const isLoggedIn = !!currentUser && !!currentUser.token;
        return isLoggedIn;
    }

    logout() {
        localStorage.removeItem('letsBlog-currentUser');
        this.currentUserSubject$.next(null);
    }
}
