import { Injectable } from '@angular/core';
import { ParentRegistration } from '../contracts/parent-registreation';
import { HttpClient } from '@angular/common/http';
import { Endpoints } from '../enums/Endpoints';
import { ChildRegistration } from '../contracts/child-registration';

@Injectable()
export class RegistrationService {

	constructor(
		private http: HttpClient,
	) { }

	registerParent(parent: ParentRegistration) {
		return this.http.post(
			Endpoints.api.registration.registerParent,
			parent
		);
	}

	registerChild(child: ChildRegistration) {
		return this.http.post(
			Endpoints.api.registration.registerChild,
			child
		);
	}
}
