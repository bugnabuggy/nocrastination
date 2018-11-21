import { environment } from '../../environments/environment';

class Endpoints {

    static baseApiUrl = environment.baseApiUrl + 'api/';


    static forntend = {
    };

    static api = {
        authorization: {
            login: environment.baseApiUrl + 'connect/token',
            logout: environment.baseApiUrl + 'connect/revocation'
        },
        registration : {
            get registerParent() { return  Endpoints.baseApiUrl + 'register'; },
            get registerChild() { return  Endpoints.baseApiUrl + 'register/child'; }
        },
        get task() { return Endpoints.baseApiUrl + 'tasks'; }
    };
}

export { Endpoints };
