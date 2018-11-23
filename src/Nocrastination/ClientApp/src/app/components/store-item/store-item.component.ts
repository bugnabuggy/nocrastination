import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { StoreItemContract } from '../../contracts';


@Component({
	selector: 'app-store-item',
	templateUrl: './store-item.component.html',
	styleUrls: ['./store-item.component.css']
})
export class StoreItemComponent implements OnInit {
	@Input() item: StoreItemContract;
	@Output() buy: EventEmitter<any> = new EventEmitter();

	constructor() { }

	ngOnInit() {
	}

	onBuy() {
		this.buy.emit(this.item);
	}

}
