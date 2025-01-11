import { HttpErrorResponse, HttpRequest } from '@angular/common/http';
import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Summary } from '@app/core/models/Funcs';
import { TableTemplate } from '@app/shared/btsp/core/TableTemplate';
import { UtilsService } from '@app/shared/services/utils.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-json-handler',
  templateUrl: './json-handler.component.html',
  styleUrl: './json-handler.component.scss',
})
export class JsonHandlerComponent implements AfterViewInit, OnInit {
  parents: Summary[] = [];
  tableTemplate: TableTemplate;
  jsonData: any = '';
  showJsonData = true;
  dataFormGroup: FormGroup = new FormGroup({});

  constructor(private utils: UtilsService, private cdr: ChangeDetectorRef) {
    this.tableTemplate = new TableTemplate(this.cdr);
    this.tableTemplate.childRequestInterceptor = (data: Summary) =>
      new HttpRequest('GET', this.utils.format(environment.apiEndPoint.summary.getChilds, data.id));
    this.tableTemplate.childResponseInterceptor = (response) => (response?.length > 0 && response[0].summaries) ?? [];
  }

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
          this.populateChilds(this.getControlValue('parent'));
          this.jsonData = response;
        },
        error: (error: HttpErrorResponse) => {
          this.parents = [];
        },
      });
  }

  populateChilds(id?: string) {
    this.getChilds(id).subscribe({
      next: (response) => {
        this.tableTemplate.dataSource.set(response[0].summaries ?? []);
      },
      error: (error: HttpErrorResponse) => {
       console.log(error);
      },
    });
  }

  getChilds(id?: string) {
    return this.utils
      .get<Summary[]>(
        this.utils.format(environment.apiEndPoint.summary.getChilds, id)
      );
  }


  errorMessage = '';
  onSubmit() { }

  getControlValue(controlName: string) {
    return this.getControl(controlName).value;
  }
  getControl(controlName: string) {
    return this.dataFormGroup.controls[controlName];
  }
}
