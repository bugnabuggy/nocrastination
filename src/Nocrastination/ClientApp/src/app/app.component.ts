import { Component, DoCheck } from '@angular/core';
import { title } from 'process';
import { UserService } from './services';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements DoCheck {
	title = 'Nocrastination app';
	isRequestInProgress: boolean;

	constructor(
		public userSvc: UserService,
	) {	}

	ngDoCheck() {
		this.isRequestInProgress = this.userSvc.requestsInProgress > 0;
	}

}
