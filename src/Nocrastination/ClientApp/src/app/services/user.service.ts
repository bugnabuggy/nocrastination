import { Injectable } from '@angular/core';

@Injectable()
export class UserService {
    requestsInProgress: number = 0;
    pageTitle: string;
}
