import { Component, OnInit } from '@angular/core';

import { UserService, StoreService } from '../../services';
import { mergeMap } from 'rxjs/operators';
import { ChildStatusContract, StoreItemContract } from '../../contracts';

@Component({
	selector: 'app-store-page',
	templateUrl: './store-page.component.html',
	styleUrls: ['./store-page.component.css']
})
export class StorePageComponent implements OnInit {
	status: ChildStatusContract = { score: 0, itemImageUrl: '', itemName: '' };
	items: StoreItemContract[] = [];

	constructor(
		private storeSvc: StoreService,
		private userSvc: UserService
	) { }

	ngOnInit() {
		this.items = [];
		const result = this.userSvc.getStatus().pipe(
			mergeMap(x => {
				this.status = x;
				return this.storeSvc.get();
			})
		);

		result.subscribe((val: StoreItemContract[]) => {
			// tslint:disable-next-line:forin
			for (const index in val) {
				const item = val[index];
				if (item.points <= this.status.score) {
					item.isEnabled = true;
				}
				this.items.push(item);
			}
			console.log(this.items);
		});
	}

	buy(item: StoreItemContract) {
		this.storeSvc.buy(item)
			.subscribe(
				val => {
					console.log(val);
					this.ngOnInit();

				}
			);

	}
}
