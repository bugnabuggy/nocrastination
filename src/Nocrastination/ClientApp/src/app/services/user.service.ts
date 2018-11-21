import { Injectable } from '@angular/core';
import { SecurityService } from './security.service';

@Injectable()
export class UserService {
    requestsInProgress: number = 0;
    pageTitle: string;
    userId: string;

    constructor(
        private securitySvc: SecurityService
    ) {

        const userId = JSON.parse(localStorage.getItem('userId'));
        if (userId) {
            this.userId = userId;
        }
    }

    setUser(user: any) {
        this.userId = user.id;
        localStorage.setItem('userId', JSON.stringify(user.id));
    }

    canActivate(): boolean {
        return true;
    }
}
