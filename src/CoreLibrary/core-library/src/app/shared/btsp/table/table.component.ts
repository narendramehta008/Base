import {
  ChangeDetectorRef,
  Component,
  ContentChild,
  ContentChildren,
  EventEmitter,
  Input,
  OnInit,
  Output,
  QueryList,
  TemplateRef,
} from '@angular/core';
import { TableTemplate } from '../core/TableTemplate';
import { map } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'btsp-col',
  template: ``,
  styles: ``,
})
export class ColComponent implements OnInit {
  @Input() dataField: string = '';
  @Input() caption?: string;
  @Input() link?: string;
  @Input() dataClass?: string;
  @Input() headerClass?: string;
  @ContentChild('customItem', { read: TemplateRef })
  customItem?: TemplateRef<any>;
  constructor(private cdr: ChangeDetectorRef) { }

  ngOnInit(): void { }

  ngAfterContentInit() {
    this.cdr.detectChanges();
  }
}

@Component({
  selector: 'btsp-table',
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
})
export class TableComponent implements OnInit {
  @Input() type: 'DataGrid' | 'TreeGrid' = 'DataGrid';
  @Input() tableTemplate: TableTemplate;
  @ContentChildren(ColComponent) cols?: QueryList<ColComponent>;

  constructor(private cdr: ChangeDetectorRef, private http: HttpClient) {
    this.tableTemplate = new TableTemplate(cdr);
  }

  ngOnInit(): void { }

  ngAfterContentInit() {
    this.tableTemplate && (this.tableTemplate.cols = this.cols);
    this.cdr.detectChanges();
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
  }

  display(data: any): string {
    return 'temp';
  }

  expandChild(data: any) {
    let key = this.tableTemplate.primaryKey;
    let ds = this.tableTemplate.dataSource();
    if (data.expanded) {
      data.expanded = false;
      if (ds) {
        let index = ds.findIndex((val) => val[key] == data[key]);
        if (index) ds.splice(index + 1, data.dataSource.length);
      }
    }
    else {
      let requestInterceptor = this.tableTemplate.childRequestInterceptor;

      if (requestInterceptor) {
        let request = requestInterceptor(data);
        let childReq = this.http.request(request);
        let responseInterceptor = this.tableTemplate.childResponseInterceptor;

        if (responseInterceptor)
          childReq = childReq.pipe(
            map((res:any) => res?.body),
            map((body) => responseInterceptor(body)),
          );

        childReq.subscribe({
          next(response: any) {
            if(response){
              data.dataSource = response;
              let index = ds.findIndex((val) => val[key] == data[key]);
              if (index) {
                ds.splice(index + 1, 0, ...data.dataSource)
              }
            }
           
          },
        });


      }
      data.expanded = true;
    }
  }

  loadChilds(child: any) {
    let key = this.tableTemplate.primaryKey;
    let ds = this.tableTemplate.dataSource();
    if (ds) {
      let index = ds.findIndex((val) => val[key] == child[key]);
      if (index) ds.splice(index + 1, 0, ...ds);
    }
  }
  array(count?: number) {
    return count == null ? [] : [...Array(count)]
  }

  
}

export function IsBodyNull(value: any): value is NonNullable<any> {
  return value?.body != null;
}