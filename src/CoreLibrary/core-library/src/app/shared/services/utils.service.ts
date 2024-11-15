import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Routes, Router, Route } from '@angular/router';
import { FileSaverService } from 'ngx-filesaver';
import { Observable, catchError, map, of } from 'rxjs';

export interface IRoutes {
  parent: Route;
  childs?: Routes | undefined;
}

@Injectable({
  providedIn: 'root',
})
export class UtilsService {
  constructor(
    private router: Router,
    private httpClient: HttpClient,
    private fileSaver: FileSaverService
  ) {}

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
    return this.httpClient.get(url, params).pipe(
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

  private getFileNameFromUrl(url: string) {
    let reg = new RegExp('\\S+/');
    return url.replace(reg, '');
  }
}
