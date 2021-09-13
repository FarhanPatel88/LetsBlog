export class ApplicationUser {
    constructor(public applicationUserId: number, public username: string, public email: string, public fullname: string, public token: string) {}
}
