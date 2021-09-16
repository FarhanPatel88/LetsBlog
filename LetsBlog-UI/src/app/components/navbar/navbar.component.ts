import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
    public isCollapsed: boolean = true;

    constructor(public accountService: AccountService, private router: Router) {}

    ngOnInit(): void {}

    public logout() {
        this.accountService.logout();
        this.router.navigate(['/']);
    }

    public getCurrentUser() {
        if (this.accountService.currentUserValue) {
            return this.accountService.currentUserValue.username;
        } else {
            return;
        }
    }
}
