import { Component, OnInit } from '@angular/core';
import { OutfitContract, ChildStatusContract } from '../../contracts';
import { StoreService, UserService } from '../../services';
import { share, mergeMap } from 'rxjs/operators';

@Component({
	selector: 'app-wardrobe-page',
	templateUrl: './wardrobe-page.component.html',
	styleUrls: ['./wardrobe-page.component.css']
})
export class WardrobePageComponent implements OnInit {
	status: ChildStatusContract = {score: 0, itemImageUrl: '', itemName: ''};
	items: OutfitContract[];

	constructor(
		private storeSvc: StoreService,
		private userSvc: UserService,
	) { }

	ngOnInit() {
		this.userSvc.getStatus().pipe(
			share(),
			mergeMap( x => {
				this.status = x;
				return this.storeSvc.myOutfits();
			})
		).subscribe( (val: OutfitContract[]) => {
			this.items = val.filter(x => x.purchaseId !== this.status.purchaseId);
		});
	}


	select(item: OutfitContract ) {
		this.storeSvc.select(item)
			.subscribe( val => {
				this.ngOnInit();
			});
	}
}
