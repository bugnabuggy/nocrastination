import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Endpoints } from '../enums/Endpoints';
import { StoreItemContract } from '../contracts/store-item-contract';


@Injectable({
	providedIn: 'root'
})
export class StoreService {

	constructor(
		private http: HttpClient
	) { }

	get(): Observable<StoreItemContract[]> {
		return this.http.get<StoreItemContract[]>(Endpoints.api.store.items);
	}
}
