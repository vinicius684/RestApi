import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { User } from './user';
import { BaseService } from '../base/baseService';


@Injectable()
export class UserService extends BaseService {

    constructor(private http: HttpClient) { super() }

    login(user: User): Observable<User> {
        //Imprimir Rota/Fazer algum teste de Rota
        const url = this.UrlServiceV1 + 'conta/entrar'; // Aqui você pode definir a URL completa
        console.log('URL da solicitação:', url); // Imprime a URL antes de fazer a chamada HTTP
        //
        return this.http
            .post(this.UrlServiceV1 + 'conta/entrar', user, super.ObterHeaderJson())
            .pipe(
                map(super.extractData),
                catchError(super.serviceError)
            );
    }

    persistirUserApp(response: any){ //Pega o Response
        //Armazenando Token de Autenticação
        localStorage.setItem('app.token', response.accessToken);//Token
        localStorage.setItem('app.user', JSON.stringify(response.userToken));//Outras Infos que esão no JWT
    }
}