import { ChangeDetectorRef, QueryList, signal, Signal } from '@angular/core';
import { ColComponent } from '@app/shared/btsp/table/table.component';
import { toObservable } from '@angular/core/rxjs-interop';
import { HttpHeaders, HttpContext, HttpParams, HttpEvent, HttpClient, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

// export interface ITableTemplate {
//   tableClass?: string;
//   // headers?: string[];
//   keys?: string[];
//   cols?: QueryList<ColComponent>;
//   dataSource?: Signal<any[]>;
//   primaryKey: string | 'id';
// }


export class TableTemplate {
  constructor(private cdr: ChangeDetectorRef) {
    this.dataSourceSubs$.subscribe({
      next: (dataSource) => {
        this.keys || this.populateKeys(dataSource)
      }
    });
  }
  keys?: string[] | undefined;

  populateKeys(dataSource: any[]) {
    let currentKeys = this.cols?.map(a => a.dataField) ?? [];
    if (this.keys) {
      this.compareKeys(this.keys, currentKeys) || this.populateCols(this.keys);
    }
    else {
      let updateKeys = dataSource.length == 0 ? [] : Object.keys(dataSource[0]);
      this.compareKeys(updateKeys, currentKeys) || this.populateCols(updateKeys);
    }
  }

  compareKeys(currentKeys: string[], updatedKeys: string[]) {
    return updatedKeys.length == currentKeys?.length && updatedKeys.every((val, index) => val == currentKeys[index]);
  }

  populateCols(keys: string[]) {
    this.cols && this.cols.reset([]);
    keys.forEach((key: string) => {
      if (key === 'treeLevel') return;
      let component = new ColComponent(this.cdr);
      component.dataField = key;
      this.cols?.reset([...this.cols.toArray(), component]);
    });
  }

  tableClass = 'table  table-hover';
  cols?: QueryList<ColComponent>;
  dataSource = signal(new Array<any>());
  primaryKey: string = 'id';
  dataSourceSubs$ = toObservable(this.dataSource);
  childRequestInterceptor?: ((data: any) => HttpRequest<any>) | undefined;
  childResponseInterceptor?: ((response: any) => any) | undefined;
}


export interface IDoc {
  name: string;
  param: string;
  dataType: string;
  required: string;
  link?: string;
  description?: string;
}
