import {
  ChangeDetectorRef,
  Component,
  ContentChild,
  ContentChildren,
  Input,
  OnInit,
  QueryList,
  TemplateRef,
} from '@angular/core';
import { ITableTemplate } from '@app/core/models/TableTemplate';

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
  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {}

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
  @Input() tableTemplate?: ITableTemplate;
  @ContentChildren(ColComponent) cols?: QueryList<ColComponent>;

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {}

  ngAfterContentInit() {
    this.tableTemplate && (this.tableTemplate.cols = this.cols);
    // console.log(this.cols);
    this.cdr.detectChanges();
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
  }

  display(data: any): string {
    console.log(data);
    return 'temp';
  }
}
