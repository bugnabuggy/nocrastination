import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Endpoints } from '../enums/Endpoints';
import { StoreItemContract } from '../contracts/store-item-contract';
import { OutfitContract } from '../contracts';


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

	buy(item: StoreItemContract): Observable<StoreItemContract[]> {
		return this.http.post<StoreItemContract[]>(
			Endpoints.api.store.buy,
			item);
	}

	select(item: OutfitContract): Observable<any> {
		return this.http.put(
			Endpoints.api.store.select + `/${item.purchaseId}`,
			{}
		);
	}

	myOutfits(): Observable<OutfitContract[]> {
		return this.http.get<OutfitContract[]>(
			Endpoints.api.store.outfits
		);
	}
}
