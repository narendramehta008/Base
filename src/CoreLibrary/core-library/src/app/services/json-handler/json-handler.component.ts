import { HttpErrorResponse } from '@angular/common/http';
import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Summary } from '@app/core/models/Funcs';
import { ITableTemplate, TableTemplate } from '@app/core/models/TableTemplate';
import { UtilsService } from '@app/shared/services/utils.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-json-handler',
  templateUrl: './json-handler.component.html',
  styleUrl: './json-handler.component.scss',
})
export class JsonHandlerComponent implements AfterViewInit, OnInit {
  parents: Summary[] = [];
  tableTemplate: ITableTemplate = { dataSource: [] };
  jsonData: any = '';
  showJsonData = true;
  dataFormGroup: FormGroup = new FormGroup({});

  constructor(private utils: UtilsService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    // this.dataFormGroup.addControl('parent', new FormControl(1));
    // this.dataFormGroup.addControl('showjsonData', new FormControl(false));
    this.dataFormGroup = new FormGroup({
      parent: new FormControl(1),
      showParentJsonData: new FormControl(false),
      showChildJsonData: new FormControl(false),
    });
  }
  ngAfterViewInit(): void {
    this.cdr.detectChanges();
    this.loadData();
  }

  //https://localhost:7050/api/Summary/GetParents
  //https://localhost:7050/api/Summary/GetParents
  loadData() {
    this.utils
      .getRequest(environment.apiEndPoint.summary.getParents)
      .subscribe({
        next: (response: Summary[]) => {
          this.parents = response; //[0].summaries ?? [];
          this.populateChilds();
          this.jsonData = response;
        },
        error: (error: HttpErrorResponse) => {
          this.parents = [];
          console.log(error);
        },
      });
  }

  populateChilds() {
    console.log('test');
    let id = this.getControlValue('parent');
    this.utils
      .getRequest(
        this.utils.format(environment.apiEndPoint.summary.getChilds, id)
      )
      .subscribe({
        next: (response: Summary[]) => {
          this.tableTemplate.dataSource = response[0].summaries ?? [];
          this.tableTemplate = new TableTemplate(this.tableTemplate, this.cdr);
        },
        error: (error: HttpErrorResponse) => {
          this.parents = [];
          console.log(error);
        },
      });
  }

  errorMessage = '';
  onSubmit() {}

  getControlValue(controlName: string) {
    return this.getControl(controlName).value;
  }
  getControl(controlName: string) {
    return this.dataFormGroup.controls[controlName];
  }
}
