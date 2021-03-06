import { environment } from '../../environments/environment';

class Endpoints {

	static baseApiUrl = environment.apiServerUrl + 'api/';


	static forntend = {
	};

	static api = {
		authorization: {
			login: environment.apiServerUrl + 'connect/token',
			logout: environment.apiServerUrl + 'connect/revocation'
		},
		registration: {
			get registerParent() { return Endpoints.baseApiUrl + 'account/register'; },
			get registerChild() { return Endpoints.baseApiUrl + 'account/child'; }
		},
		get task() { return Endpoints.baseApiUrl + 'tasks'; },
		get latestTask() { return Endpoints.baseApiUrl + 'tasks/latest'; },
		get child() { return Endpoints.baseApiUrl + 'account/child'; },
		get user() { return Endpoints.baseApiUrl + 'account/user'; },

		get childStatus() { return Endpoints.baseApiUrl + 'item/status'; },

		store : {
			get items() {return Endpoints.baseApiUrl + 'store';  },
			get buy() { return Endpoints.baseApiUrl + 'store/buy'; },
			get outfits() {return Endpoints.baseApiUrl + 'item'; },
			get select() {return Endpoints.baseApiUrl + 'item'; }
		}

	};
}

export { Endpoints };
