import { HttpHeaders } from '@angular/common/http';
import { throwError } from 'rxjs';

export abstract class BaseService { //Todos os Métodos que estão sendo utilizados em mais de um Serviço

    protected UrlServiceV1: string = "https://localhost:7222/api/";
    //protected UrlServiceV1: string = "https://devioapi.azurewebsites.net/api/v1/";

    protected ObterHeaderFormData() {//Upload Alternativo, passando autorização
        return {
            headers: new HttpHeaders({
                'Content-Disposition': 'form-data; name="produto"',
                'Authorization': `Bearer ${this.obterTokenUsuario()}`
            })
        };
    }

    protected ObterHeaderJson() {//Upload Normal sem Authorization
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
    }

    protected ObterAuthHeaderJson(){ //Upload Normal passando Autorização (Token do user)
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.obterTokenUsuario()}`
            })
        };
    }

    protected extractData(response: any) { //Extrair .data do response
        return response.data || {};
    }

    public obterUsuario() {
        return JSON.parse(localStorage.getItem('app.user'));
    }

    protected obterTokenUsuario(): string {
        return localStorage.getItem('app.token');
    }

    protected serviceError(error: Response | any) {
        let errMsg: string;

        if (error instanceof Response) {

            errMsg = `${error.status} - ${error.statusText || ''}`;
        }
        else {
            errMsg = error.message ? error.message : error.toString();
        }

        console.error(errMsg);
        return throwError(errMsg);
    }
}