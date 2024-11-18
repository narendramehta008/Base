import { ChangeDetectorRef, QueryList } from '@angular/core';
import { ColComponent } from '@app/shared/btsp/table/table.component';

export interface ITableTemplate {
  tableClass?: string;
  // headers?: string[];
  keys?: string[];
  cols?: QueryList<ColComponent>;
  dataSource?: any[];
}

export class TableTemplate implements ITableTemplate {
  constructor(tableTemplate: ITableTemplate, private cdr: ChangeDetectorRef) {
    let current: any = this;
    let template: any = tableTemplate;
    if (tableTemplate != null)
      Object.keys(tableTemplate).forEach((key: string) => {
        current[key] = template[key];
      });

    tableTemplate?.dataSource && this.populateCols();
  }

  populateCols() {
    this.cols && this.cols.reset([]);
    Object.keys(this.dataSource[0]).forEach((key: string) => {
      let component = new ColComponent(this.cdr);
      component.dataField = key;
      this.cols?.reset([...this.cols.toArray(), component]);
    });
  }
  tableClass = 'table  table-hover';
  // headers = [];
  // keys: string[] = [];
  cols?: QueryList<ColComponent>;
  dataSource = [];
}

export interface IDoc {
  name: string;
  param: string;
  dataType: string;
  required: string;
  link?: string;
  description?: string;
}
