import { Component, OnInit, Input, EventEmitter, Output} from '@angular/core';
import { OutfitContract } from '../../contracts';


@Component({
	selector: 'app-outfit-item',
	templateUrl: './outfit-item.component.html',
	styleUrls: ['./outfit-item.component.css']
})
export class OutfitItemComponent implements OnInit {
	@Input() item: OutfitContract;
	@Output() select: EventEmitter<any> = new EventEmitter();

	constructor() { }

	ngOnInit() {
	}

	onSelect() {
		this.select.emit(this.item);
	}
}
