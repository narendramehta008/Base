import { HttpClient, HttpHandler, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Routes, Router, Route } from '@angular/router';
import { FileSaverService } from 'ngx-filesaver';
import { Observable, catchError, map, of } from 'rxjs';
import * as _ from 'lodash';
import { environment } from '@environments/environment';
import { TokenModel } from '../models/token-models';

export interface IRoutes {
  parent: Route;
  childs?: Routes | undefined;
}

@Injectable({
  providedIn: 'root',
})
export class UtilsService extends HttpClient {
  constructor(
    private router: Router,
     private httpClient: HttpClient,
    private fileSaver: FileSaverService,
      handler: HttpHandler
  ) {
    super(handler);
  }

  routes: Routes = [];
  downloadWithResponseFileName(
    url: string,
    postData?: any,
    params?: any
  ): Observable<HttpResponse<Blob>> {
    return this.httpClient
      .post(url, postData, { responseType: 'blob', observe: 'response' })
      .pipe(
        map((result: HttpResponse<Blob>) => {
          const contentDisposition = result.headers.get('content-disposition');
          let filename: any =
            this.getFilenameFromContentDisposition(contentDisposition);
          filename = filename.replaceAll('"', '');
          this.fileSaver.save(result.body, filename);
          return result;
        })
      );
  }

  downloadWithFileName(url: string): Observable<HttpResponse<Blob>> {
    return this.httpClient
      .get(url, { responseType: 'blob', observe: 'response' })
      .pipe(
        map((result: HttpResponse<Blob>) => {
          const contentDisposition = result.headers.get('content-disposition');
          let filename: any =
            this.getFilenameFromContentDisposition(contentDisposition) ||
            this.getFileNameFromUrl(url);
          this.fileSaver.save(result.body, filename);
          return result;
        })
      );
  }
  getRequest(url: string, params?: any) {
    return this.httpClient.get(url, { params: params }).pipe(
      map((res) => res),
      map((body) => body),
      catchError((body) => of(body))
    );
  }
  postRequest(url: string, postData?: any) {
    return this.httpClient.post(url, postData).pipe(
      map((res) => res),
      map((body) => body),
      catchError((body) => of(body))
    );
  }
  private getFilenameFromContentDisposition(
    contentDisposition?: string | null
  ) {
    if (!contentDisposition) return '';
    const regex = /filename=(?<filename>[^,;]+);/g;
    const match = regex.exec(contentDisposition);
    let filename = match?.groups?.['filename'];
    filename = filename?.replaceAll('"', '');
    return filename;
  }

  getFileNameFromUrl(url: string) {
    let reg = new RegExp('\\S+/');
    return url.replace(reg, '');
  }

  format(input: string, ...args: any[]): string {
    return input.replace(/{(\d+)}/g, function (match, number) {
      return typeof args[number] != 'undefined' ? args[number] : match;
    });
  }

  makeSummaryTree(nodes: any[], parentId: any): any {
    return nodes
      .filter((node) => node.parentId === parentId)
      .reduce(
        (tree, node) => [
          ...tree,
          {
            ...node,
            summaries: this.makeSummaryTree(nodes, node.id),
          },
        ],
        []
      );
  }

  getTokenDetails():any{
    let token='';
    const tokenDetails = localStorage.getItem(environment.tokenName) || null;
    if (tokenDetails) {
        let tempTokenDetail: TokenModel = JSON.parse(tokenDetails);
        token = tempTokenDetail.token;
    }
   return {Authorization: `Bearer ${token}`};
  }
}
